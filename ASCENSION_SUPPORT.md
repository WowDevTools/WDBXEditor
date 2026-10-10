# Ascension custom DBC support (WotLK 3.3.5a)

This branch adds an **opt-in-by-missing-definition raw WDBC fallback** to WDBXEditor without changing the existing retail DBC/DB2 definitions or their normal read/write paths.

## Open an Ascension DBC

1. Open a **copy** of an extracted `.dbc` file (not an MPQ or listfile).
2. Select **WotLK 3.3.5 (12340)**.
3. If there is no existing named definition, compatible `WDBC` files open using `_Row`, `Field_000`, `Field_001` etc., and up to three `TailByte_*` columns.

The virtual `_Row` ID is not stored on disk; repeated first-field values are allowed. The raw columns are **opaque integers/bytes** and do not imply signed IDs, floats, string offsets, arrays, or known field semantics. Verified string columns in `ManastormMessages` (`IconToken`, `Title_enUS`, `Text_enUS`) and `SpellTagTypes` (`Name_enUS`) are now **editable**. The editor appends changed UTF-8 strings to the original string block and updates only their known offset fields, keeping all other offsets valid. Unchanged files save byte-identically and saving twice does not duplicate new strings. The original string block is kept byte-for-byte when saving, rather than being reconstructed from guessed offsets.

### Safety constraints

Raw mode requires a standard `WDBC` signature, a nonzero record size, a file size matching `20 + recordCount*recordSize + stringBlockSize`, and a record width <= 65,536 bytes. Invalid files are rejected. Header `FieldCount` is preserved, but **not assumed** to equal `RecordSize / 4`, because custom tables may violate that assumption. An existing XML definition takes precedence only when its expanded field width agrees with the actual record size; otherwise raw fallback prevents mismatched-schema corruption. Empty WDBC tables use raw fallback so they can be viewed and saved without fabricating records.

**This is not a verified schema pack**. Listfiles enumerate filenames but do not supply layouts. For the full Ascension-specific editor, original extracted DBC samples are now available for Patch-M (288 tables) and Patch-S (48 tables). The first verified field labels are provided for the Manastorm and SpellTags family; the remainder are intentionally untyped unless independently verified. Do not edit live game assets without backups.

### Build

The project targets **.NET Framework 4.6.1** and builds as a Windows desktop application. A dedicated workflow is in `.github/workflows/ascension-windows.yml` (branch push or manual dispatch); it produces a downloadable Windows artifact. On Windows with Visual Studio/MSBuild, build `WDBXEditor.sln` in Release after NuGet restore.

### Uploaded sample corpus (2026-10-09)

- Patch-M contains **288** unique extracted WDBC tables; Patch-S contains **48**, with **no overlapping filenames**.
- All **336** files have a valid WDBC signature and internally consistent header/record/string-block lengths.
- **10** tables report a header FieldCount different from their actual record bytes / 4; **3** have non-4-byte-aligned records (CharBaseInfo, PowerDisplay, SpellChainEffects).
- **8** files are valid zero-row tables and must be safely opened/saved without inventing data.
- Sample data is **not** checked into Git: it came from user-supplied archives and should remain local. Only derived structural facts and verified field labels are committed.

### CI round-trip tests

`tests/Test-RawWdbcRoundTrip.ps1` runs in Windows GitHub Actions after compilation. It tests a synthetic WDBC with duplicate first-column values and an intentionally mismatched field count; unchanged save byte equality; numeric edit/reopen; non-four-byte record sizes; and malformed-length rejection. It also checks that a compatible existing retail schema remains active while a custom file reusing that filename with a different record size falls back to raw mode. This is **not** a substitute for real Ascension DBC sample verification.

### Manual round-trip verification

- Open a copied unknown WDBC and confirm `_Row` is virtual and the raw values appear.
- Save without edits, then use `fc /b original.dbc saved.dbc` to check byte equality.
- Edit one known-safe integer in another copy, save/reopen, and compare all bytes; only the intended 4 bytes should differ.
- Verify a known WotLK DBC still opens/saves using its original named definition.
- Reject unsupported files rather than trying to interpret them as raw WDBC.

The Windows build and synthetic read/write/edit/reopen tests are verified in CI. The original user-supplied Ascension DBC files are **not** redistributed through CI or included in the public repository.

### Important: preserve the filename

WDBXEditor identifies DBC definitions by **basename**. When saving or testing a known Ascension table, retain `ManastormMessages.dbc` or `SpellTagTypes.dbc` as the filename. Save in a different **directory** if you need a copy. Renaming it to an unrelated filename intentionally reverts to generic raw fallback.

### MysticEnchant.dbc (Patch-M) and raw-mode notification

A valid raw-mode load is normal, **not a warning**. The fallback no longer reports it as a warning; invalid files still throw real errors.

Patch-M's MysticEnchant has 7,841 records of 31 four-byte words each, with a 359-byte string block. All six string columns (3, 4, 15, 16, 17, 18) were checked against the actual file and are now editable as quality/specialization tokens. Field 2 is IEEE-754 float data, editable through `Field_002_Float` while the original bit pattern remains visible in `Field_002`. Unknown fields remain untouched raw numbers. String offset zero can contain nonempty text, as in this table. The original binary is **not** redistributed via GitHub; synthetic records are tested on Windows CI.
