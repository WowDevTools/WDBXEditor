# WDBX Editor

[![Latest Download](https://img.shields.io/badge/Latest-Download-blue.svg)](https://ci.appveyor.com/api/projects/majorcyto/wdbxeditor/artifacts/WDBXEditor.zip) [![DoxygenDoc](https://img.shields.io/badge/Docs-Doxygen-red.svg)](http://wdbxdocs.wowdev.info/) 
[![Bountysource](https://www.bountysource.com/badge/tracker?tracker_id=44220492)](https://www.bountysource.com/trackers/44220492-wowdevtools-wdbxeditor?utm_source=44433103&utm_medium=shield&utm_campaign=TRACKER_BADGE) [![WoWDev Tools on discord](https://img.shields.io/badge/Discord-WoWDev Tools-738bd7.svg)](https://discord.gg/9aVwPpb)


## Build Status

CI | Build | Defects
:------------: | :------------: | :------------:
AppVeyor | [![Build status](https://ci.appveyor.com/api/projects/status/y4sp6sijsdvu2v80/branch/master?svg=true)](https://ci.appveyor.com/project/majorcyto/wdbxeditor/branch/master) | CodeCov Badge Soon | 


## About
This editor has full support for reading and saving all release versions of DBC, DB2, WDB and ADB. This does include support for Legion DB2 and ADB files and works with all variants (header flags) of these.
Like the other editors I've used a definition based system whereby definitions tell the editor how to interpret each file's columns - this is a lot more reliable than guessing column types but does mean the definitions must be maintained. So far, I've mapped almost all expansions with MoP being ~50% complete and everything else being 99%+ (excluding column names).

You will need [Microsoft .NET Framework 4.6.1](https://www.microsoft.com/en-us/download/details.aspx?id=49982) to run this application

##Features:
* Full support of release versions of DBC, DB2, WDB and ADB (WCH3 and WCH4 are not supported as I deem them depreciated)
* Supports being the default file assocation
* Opening and having open multiple files regardless of type and build
* Open DBC/DB2 files from both MPQ archives and CASC directories
* Save single (to file) and save all (to folder)
* Standard CRUD operations as well as go to, copy row, paste row, undo and redo
* Hide, show, hide empty and sort columns
* A relatively powerful column filter system (similar to boolean search)
* Displaying and editing columns in hex (numeric columns only)
* Exporting to a SQL database, SQL file, CSV file and MPQ archives
* Importing from a SQL database and a CSV file
* An Excel style Find and Replace
* Shortcuts for common tasks using common shortcut key combinations
* A help file to try and cover off some of the pitfalls and caveats of the program (needs some work)

##Tools:
* Definition editor for maintaining the definitions
* WotLK Item Import to remove the dreaded red question mark from custom items
* WDB5 Parser which is an attempt to automatically parse the structure of WDB5 files

##Project Goal:
The goal of this project is to create a communal program that is compatible with all file variants, is feature rich and negates the need to use multiple different programs.
This means any and all contribution in the form of commits, change requests, issues etc are more than welcome!

##Credits:
Credits go to Ladislav Zezula for the awesome StormLib and thanks to all those that contribute to the WoWDev wiki.
I've also patched the definitions together for various sources across the internet, there are too many to name, but thanks to all.
