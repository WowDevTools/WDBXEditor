# WDBX Editor

[![Latest Download](https://img.shields.io/badge/Latest-Download-blue.svg)](https://ci.appveyor.com/api/projects/majorcyto/wdbxeditor/artifacts/WDBXEditor.zip) [![Bountysource](https://www.bountysource.com/badge/tracker?tracker_id=44220492)](https://www.bountysource.com/trackers/44220492-wowdevtools-wdbxeditor?utm_source=44433103&utm_medium=shield&utm_campaign=TRACKER_BADGE)
[![WoWDevDiscord](https://img.shields.io/badge/Discord-WoWDev-blue.svg)](https://discord.gg/EzKJjtv)


### Build Status

CI | Build 
:------------: | :------------: 
AppVeyor | [![Build status](https://ci.appveyor.com/api/projects/status/y4sp6sijsdvu2v80/branch/master?svg=true)](https://ci.appveyor.com/project/majorcyto/wdbxeditor/branch/master) | 


### About
This editor has full support for reading and saving all release versions of DBC, DB2, WDB, ADB and DBCache. This does include support for Legion DB2 and DBCache files and works with all variants (header flags) of these.
Like the other editors I've used a definition based system whereby definitions tell the editor how to interpret each file's columns - this is a lot more reliable than guessing column types but does mean the definitions must be maintained. So far, I've mapped almost all expansions with MoP being ~50% complete and everything else being 99%+ (excluding column names).

You will need [Microsoft .NET Framework 4.6.1](https://www.microsoft.com/en-us/download/details.aspx?id=49982) to run this application

### Features:
* Full support of release versions of DBC, DB2, WDB, ADB and DBCache (WCH3 and WCH4 are not supported as I deem them depreciated)
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
* A simple memory reader to get player's co-ordinates from the client
* A colour picker for LightData and LightIntBand

### Tools:
* Definition editor for maintaining the definitions
* WotLK Item Import to remove the dreaded red question mark from custom items
* Legion Parser which is an attempt to automatically parse the structure of WDB5 and WDB6 files

### Project Goal:
The goal of this project is to create a communal program that is compatible with all file variants, is feature rich and negates the need to use multiple different programs.
This means any and all contribution in the form of commits, change requests, issues etc are more than welcome!

### Credits:
Credits go to Ladislav Zezula for the awesome StormLib and thanks to all those that contribute to the WoWDev wiki.
I've also patched the definitions together for various sources across the internet, there are too many to name, but thanks to all.

### MySQL Setup:
LOAD DATA LOCAL INFILE is not enabled by default.Normally, it should be enabled by placing local-infile=1 in my.cnf. But it does not work for all installations.
Connect to your server using MySQL or any console client using the following command:
```
mysql -u root -p 
```
It should echo the following:
```
Enter password:
```
Now you have to enter your root password.
After you are connected, paste the following command:
```
SHOW GLOBAL VARIABLES LIKE 'local_infile';
```
It should echo the following:
```
+---------------+-------+
| Variable_name | Value |
+---------------+-------+
| local_infile  | OFF   |
+---------------+-------+
1 row in set (0.00 sec)
```
If local_infile = OFF, you need to paste the following command, otherwise if it is ON, you are good to go!
```
SET GLOBAL local_infile = 'ON'
```
It should echo the following:
```
+---------------+-------+
| Variable_name | Value |
+---------------+-------+
| local_infile  | ON    |
+---------------+-------+
1 row in set (0.00 sec)
```
