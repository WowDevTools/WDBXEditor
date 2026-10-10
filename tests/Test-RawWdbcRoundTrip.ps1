# Runtime smoke test for raw WDBC fallback. Uses synthetic bytes only.
# Run after building WDBXEditor.sln on Windows with .NET Framework 4.6.1.
$ErrorActionPreference = 'Stop'

$exe = Join-Path $PSScriptRoot '..\WDBXEditor\bin\Release\WDBX Editor.exe'
$exe = [IO.Path]::GetFullPath($exe)
if (-not (Test-Path $exe)) { throw "Missing built WDBX Editor.exe at $exe" }
$assembly = [Reflection.Assembly]::LoadFrom($exe)
$databaseType = $assembly.GetType('WDBXEditor.Storage.Database', $true)
$flags = [Reflection.BindingFlags]::Static -bor [Reflection.BindingFlags]::Public -bor [Reflection.BindingFlags]::NonPublic
$databaseType.GetProperty('BuildNumber', $flags).SetValue($null, [int]12340, $null)
$reader = [Activator]::CreateInstance($assembly.GetType('WDBXEditor.Reader.DBReader', $true))
$dir = Join-Path ([IO.Path]::GetTempPath()) ('WDBXEditorRaw-' + [Guid]::NewGuid().ToString('N'))
[void](New-Item -Path $dir -ItemType Directory -Force)

function Assert([bool]$condition, [string]$reason) {
    if (-not $condition) { throw $reason }
}
function SameBytes([string]$left, [string]$right) {
    $a = [IO.File]::ReadAllBytes($left)
    $b = [IO.File]::ReadAllBytes($right)
    return [Convert]::ToBase64String($a) -ceq [Convert]::ToBase64String($b)
}
function New-Writer([string]$path, [uint32]$count, [uint32]$fieldCount, [uint32]$recordSize, [byte[]]$strings) {
    $writer = [IO.BinaryWriter]::new([IO.File]::Create($path))
    $writer.Write([Text.Encoding]::ASCII.GetBytes('WDBC'))
    $writer.Write($count)
    $writer.Write($fieldCount)
    $writer.Write($recordSize)
    $writer.Write([uint32]$strings.Length)
    return $writer
}

try {
    $strings = [byte[]](0,104,101,108,108,111,0,119,111,114,108,100,0)
    $source = Join-Path $dir 'UnknownAscension.dbc'
    $w = New-Writer $source 2 179 12 $strings
    try {
        $w.Write([uint32]42); $w.Write([uint32]1); $w.Write([uint32]1065353216)
        $w.Write([uint32]42); $w.Write([uint32]7); $w.Write([uint32]1077936128)
        $w.Write($strings)
    } finally { $w.Dispose() }

    $entry = $reader.Read($source)
    Assert ([string]::IsNullOrEmpty($reader.ErrorMessage)) 'Successful raw DBC load must not generate a warning'
    Assert $entry.Header.IsRawLayout 'Unknown WDBC did not select raw mode'
    Assert ($entry.Data.Rows.Count -eq 2) 'Wrong record count'
    Assert ($entry.Data.Columns.Count -eq 4) 'Wrong raw column count'
    Assert ([uint32]$entry.Data.Rows[0]['Field_000'] -eq 42) 'Wrong first field'
    Assert ([uint32]$entry.Data.Rows[1]['Field_000'] -eq 42) 'Duplicate first field rejected'
    Assert ($entry.Header.FieldCount -eq 179) 'Original header FieldCount not preserved'

    $unchanged = Join-Path $dir 'unchanged.dbc'
    $reader.Write($entry, $unchanged)
    Assert (SameBytes $source $unchanged) 'Unchanged raw save changed file bytes'

    $entry.Data.Rows[0]['Field_001'] = [uint32]13
    $edited = Join-Path $dir 'edited.dbc'
    $reader.Write($entry, $edited)
    $before = [IO.File]::ReadAllBytes($source)
    $after = [IO.File]::ReadAllBytes($edited)
    Assert ($before.Length -eq $after.Length) 'Editing changed file length'
    $changed = 0
    for ($i = 0; $i -lt $before.Length; $i++) {
        if ($before[$i] -ne $after[$i]) {
            $changed++
            Assert ($i -ge 24 -and $i -lt 28) 'Editing changed unrelated bytes'
        }
    }
    Assert ($changed -gt 0) 'Edit was not written'
    $reopened = $reader.Read($edited)
    Assert ([uint32]$reopened.Data.Rows[0]['Field_001'] -eq 13) 'Edited field did not survive reopen'
    Write-Host 'PASS: mismatched header, repeated first column, string preservation, edit/reopen'

    $short = Join-Path $dir 'NonAligned.dbc'
    $w = New-Writer $short 2 77 6 ([byte[]](0))
    try {
        $w.Write([uint32]100); $w.Write([byte]17); $w.Write([byte]34)
        $w.Write([uint32]200); $w.Write([byte]51); $w.Write([byte]68)
        $w.Write([byte]0)
    } finally { $w.Dispose() }
    $odd = $reader.Read($short)
    Assert ($odd.Data.Columns.Count -eq 4) 'Tail byte fields missing'
    Assert ([byte]$odd.Data.Rows[1]['TailByte_01'] -eq 68) 'Tail byte was not read'
    $oddSaved = Join-Path $dir 'nonaligned-saved.dbc'
    $reader.Write($odd, $oddSaved)
    Assert (SameBytes $short $oddSaved) 'Non-4-byte record changed in round trip'
    Write-Host 'PASS: non-4-byte record size and tail-byte round trip'

    $empty = Join-Path $dir 'EmptyCustom.dbc'
    $w = New-Writer $empty 0 4 16 ([byte[]]@())
    $w.Dispose()
    $emptyEntry = $reader.Read($empty)
    Assert ($emptyEntry.Data.Rows.Count -eq 0) 'Empty WDBC rejected'
    $emptySaved = Join-Path $dir 'empty-saved.dbc'
    $reader.Write($emptyEntry, $emptySaved)
    Assert (SameBytes $empty $emptySaved) 'Empty WDBC did not round trip'
    Write-Host 'PASS: zero-row WDBC open/save'

    $mapped = Join-Path $dir 'Manastorm.dbc'
    $w = New-Writer $mapped 1 9 36 ([byte[]]@())
    try {
        $w.Write([uint32]1); $w.Write([uint32]389); $w.Write([uint32]2); $w.Write([uint32]431)
        for ($i = 4; $i -lt 9; $i++) { $w.Write([uint32]0) }
    } finally { $w.Dispose() }
    $named = $reader.Read($mapped)
    Assert ([uint32]$named.Data.Rows[0]['MapId'] -eq 389) 'Verified Manastorm MapId missing'
    Assert ([uint32]$named.Data.Rows[0]['DungeonEncounterId'] -eq 431) 'Verified encounter field missing'
    $namedSaved = Join-Path $dir 'named-saved.dbc'
    $reader.Write($named, $namedSaved)
    Assert (SameBytes $mapped $namedSaved) 'Named raw fields changed data'
    Write-Host 'PASS: verified Manastorm field labels and round-trip'

    $message = Join-Path $dir 'ManastormMessages.dbc'
    $msgBlock = [Text.Encoding]::UTF8.GetBytes("icon`0Unlocked`0Welcome to Manastorm`0")
    $w = New-Writer $message 1 39 156 $msgBlock
    try {
        for ($i = 0; $i -lt 39; $i++) {
            $value = [uint32]0
            if ($i -eq 0) { $value = 1 }
            if ($i -eq 5) { $value = 5 }
            if ($i -eq 22) { $value = 14 }
            $w.Write($value)
        }
        $w.Write($msgBlock)
    } finally { $w.Dispose() }
    $msg = $reader.Read($message)
    Assert ($msg.Data.Rows[0]['IconToken'] -eq 'icon') 'Icon text not decoded'
    Assert ($msg.Data.Rows[0]['Title_enUS'] -eq 'Unlocked') 'Title text not decoded'
    Assert ($msg.Data.Rows[0]['Text_enUS'] -eq 'Welcome to Manastorm') 'Message text not decoded'
    Assert (-not $msg.Data.Columns['Text_enUS'].ReadOnly) 'Text must be editable'
    Assert ($msg.Data.Columns['IconToken'].Ordinal -eq 6) 'Icon text is not next to its offset'
    Assert ([string]::IsNullOrEmpty($reader.ErrorMessage)) 'Verified message handler still emits raw warning'
    $messageSaved = Join-Path $dir 'message-saved.dbc'
    $reader.Write($msg, $messageSaved)
    Assert (SameBytes $message $messageSaved) 'Unedited message changed binary'
    Write-Host 'PASS: verified editable strings and byte-preserving unedited save'

    $newTitle = 'New: Caf' + [char]0x00E9 + ' unlocked!'
    $newText = 'Edited message with UTF-8 ' + [char]0x2713
    $msg.Data.Rows[0]['Title_enUS'] = $newTitle
    $msg.Data.Rows[0]['Text_enUS'] = $newText
    $editDir = Join-Path $dir 'edited-message'
    [void](New-Item -Path $editDir -ItemType Directory)
    $updated = Join-Path $editDir 'ManastormMessages.dbc'
    $reader.Write($msg, $updated)
    $editedMessage = $reader.Read($updated)
    Assert ($editedMessage.Data.Rows[0]['IconToken'] -eq 'icon') 'Unedited icon changed'
    Assert ($editedMessage.Data.Rows[0]['Title_enUS'] -eq $newTitle) 'Edited title lost'
    Assert ($editedMessage.Data.Rows[0]['Text_enUS'] -eq $newText) 'Edited message lost'
    Assert ([uint32]$editedMessage.Data.Rows[0]['Field_006'] -eq 0) 'Unrelated raw field changed'
    $again = Join-Path $dir 'message-again.dbc'
    $reader.Write($msg, $again)
    Assert (SameBytes $updated $again) 'Second save appended duplicate strings'
    Write-Host 'PASS: edit/reopen UTF-8 title and message without changing unrelated fields'

    $tagTypes = Join-Path $dir 'SpellTagTypes.dbc'
    $tagStrings = [Text.Encoding]::UTF8.GetBytes("Tag Name`0")
    $tagWriter = New-Writer $tagTypes 1 61 244 $tagStrings
    try {
        for ($i = 0; $i -lt 61; $i++) { $tagWriter.Write([uint32]0) }
        $tagWriter.Write($tagStrings)
    } finally { $tagWriter.Dispose() }
    $tagEntry = $reader.Read($tagTypes)
    Assert ($tagEntry.Data.Rows[0]['Name_enUS'] -eq 'Tag Name') 'Tag type name not decoded'
    $tagEntry.Data.Rows[0]['Name_enUS'] = 'Renamed Tag'
    $tagDir = Join-Path $dir 'edited-tags'
    [void](New-Item -Path $tagDir -ItemType Directory)
    $tagEdited = Join-Path $tagDir 'SpellTagTypes.dbc'
    $reader.Write($tagEntry, $tagEdited)
    $tagReopen = $reader.Read($tagEdited)
    Assert ($tagReopen.Data.Rows[0]['Name_enUS'] -eq 'Renamed Tag') 'Tag type name edit failed'
    Write-Host 'PASS: SpellTagTypes editable string and round trip'

    # Verify that a valid stock definition remains active, but a mismatched
    # custom table with the SAME file name is switched to raw mode.
    $defXml = '<Definition><Table Name="KnownSchema" Build="12340"><Field Name="Id" Type="int" IsIndex="true" /><Field Name="Value" Type="uint" /></Table></Definition>'
    $serializer = [Xml.Serialization.XmlSerializer]::new($assembly.GetType('WDBXEditor.Storage.Definition', $true))
    $definition = $serializer.Deserialize([IO.StringReader]::new($defXml))
    $definitionTable = @($definition.Tables)[0]
    $definitionTable.Load()
    $catalog = $databaseType.GetProperty('Definitions', $flags).GetValue($null)
    [void]$catalog.Tables.Add($definitionTable)

    $known = Join-Path $dir 'KnownSchema.dbc'
    $w = New-Writer $known 1 2 8 ([byte[]]@())
    try { $w.Write([int32]1); $w.Write([uint32]300) }
    finally { $w.Dispose() }
    $knownEntry = $reader.Read($known)
    Assert (-not $knownEntry.Header.IsRawLayout) 'Matching retail definition lost'
    Assert ([uint32]$knownEntry.Data.Rows[0]['Value'] -eq 300) 'Matching retail schema failed'

    $mismatchedDir = Join-Path $dir 'mismatch'
    [void](New-Item -Path $mismatchedDir -ItemType Directory)
    $mismatched = Join-Path $mismatchedDir 'KnownSchema.dbc'
    $w = New-Writer $mismatched 1 3 12 ([byte[]]@())
    try { $w.Write([uint32]1); $w.Write([uint32]300); $w.Write([uint32]400) }
    finally { $w.Dispose() }
    $mismatchEntry = $reader.Read($mismatched)
    Assert $mismatchEntry.Header.IsRawLayout 'Mismatched retail definition was incorrectly trusted'
    Assert ([uint32]$mismatchEntry.Data.Rows[0]['Field_002'] -eq 400) 'Mismatched final raw field missing'
    $mismatchedSaved = Join-Path $dir 'mismatch-saved.dbc'
    $reader.Write($mismatchEntry, $mismatchedSaved)
    Assert (SameBytes $mismatched $mismatchedSaved) 'Mismatched schema round trip failed'
    Write-Host 'PASS: stock schema retained and mismatched custom schema rejected'

    # Synthetic MysticEnchant matching 31 on-disk words and real Patch-M column offsets.
    # IMPORTANT: string at offset zero is NONEMPTY (RE_QUALITY_RARE).
    $mysticStrings = [Text.Encoding]::UTF8.GetBytes("RE_QUALITY_RARE`0RE_QUALITY_UNCOMMON`0NONE`0FROST`0")
    $mysticPath = Join-Path $dir 'MysticEnchant.dbc'
    $mw = New-Writer $mysticPath 2 31 124 $mysticStrings
    try {
        for ($r = 0; $r -lt 2; $r++) {
            for ($i = 0; $i -lt 31; $i++) {
                $v = [uint32]0
                switch ($i) {
                    0 { $v = [uint32]($r + 1) }
                    1 { $v = [uint32](95923 + $r) }
                    2 { $v = [BitConverter]::ToUInt32([BitConverter]::GetBytes([single]95), 0) }
                    3 { $v = [uint32]0 }
                    4 { $v = [uint32]16 }
                    15 { $v = [uint32]36 }
                    16 { $v = [uint32]36 }
                    17 { $v = [uint32]36 }
                    18 { $v = [uint32]36 }
                    20 { $v = [uint32]12345 }
                }
                $mw.Write($v)
            }
        }
        $mw.Write($mysticStrings)
    } finally { $mw.Dispose() }
    $m = $reader.Read($mysticPath)
    Assert ([string]::IsNullOrEmpty($reader.ErrorMessage)) 'MysticEnchant emitted raw warning'
    Assert ($m.Data.Rows[0]['QualityToken_1'] -eq 'RE_QUALITY_RARE') 'String offset zero decoded incorrectly'
    Assert ($m.Data.Rows[0]['QualityToken_2'] -eq 'RE_QUALITY_UNCOMMON') 'Quality 2 decoding failed'
    Assert ($m.Data.Rows[0]['SpecializationToken_1'] -eq 'NONE') 'Specialization decoding failed'
    Assert ([float]$m.Data.Rows[0]['Field_002_Float'] -eq 95) 'Float field not decoded'
    $mUnedited = Join-Path $dir 'MysticEnchant-unchanged.dbc'
    $reader.Write($m, $mUnedited)
    Assert (SameBytes $mysticPath $mUnedited) 'Unedited MysticEnchant roundtrip changed bytes'
    $m.Data.Rows[0]['QualityToken_1'] = 'RE_QUALITY_EPIC'
    $m.Data.Rows[0]['SpecializationToken_1'] = 'FROST'
    $m.Data.Rows[0]['Field_002_Float'] = [single]112
    $mDir = Join-Path $dir 'Mystic-Edited'
    [void](New-Item -ItemType Directory -Path $mDir)
    $mPath2 = Join-Path $mDir 'MysticEnchant.dbc'
    $reader.Write($m, $mPath2)
    $m2 = $reader.Read($mPath2)
    Assert ($m2.Data.Rows[0]['QualityToken_1'] -eq 'RE_QUALITY_EPIC') 'Quality token edit lost'
    Assert ($m2.Data.Rows[0]['SpecializationToken_1'] -eq 'FROST') 'Specialization edit lost'
    Assert ([float]$m2.Data.Rows[0]['Field_002_Float'] -eq 112) 'Float edit lost'
    Assert ([uint32]$m2.Data.Rows[0]['Field_020'] -eq 12345) 'Unrelated value changed'
    Assert ($m2.Data.Rows[1]['QualityToken_1'] -eq 'RE_QUALITY_RARE') 'Second row corrupted'
    $mAgain = Join-Path $dir 'mystic-again.dbc'
    $reader.Write($m, $mAgain)
    Assert (SameBytes $mPath2 $mAgain) 'Second save should not change MysticEnchant'
    Write-Host 'PASS: MysticEnchant six editable strings, float, no warning, unchanged/edited round trips'

    $bad = Join-Path $dir 'InvalidLength.dbc'
    [IO.File]::WriteAllBytes($bad, [byte[]]([IO.File]::ReadAllBytes($source) + [byte]255))
    $rejected = $false
    try { [void]$reader.Read($bad) } catch { $rejected = $true }
    Assert $rejected 'Malformed file was not rejected'
    Write-Host 'PASS: invalid file size rejected'
}
finally {
    Remove-Item -LiteralPath $dir -Recurse -Force -ErrorAction SilentlyContinue
}
