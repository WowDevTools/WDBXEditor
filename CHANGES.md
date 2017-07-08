## 1.0.6

### Changes
* WDB6 updated to parse 7.2.5+ versions
* Definition files have been updated for 7.2.0
* Additional fixes to the software
* Added column names to 7.2.0 defintion
* Definition files have been updated for 7.3.0

## 1.0.5

### Changes
* WDB6 (the new Legion DB2 format) is now supported for both reading and writing
* Definition files have been updated for 7.2.0
* The software now caches loaded but not being edited files on the file system for better memory useage
* WD5 Parser is now Legion Parser and parses WDB5 and WDB6
* A new attribute has been added to the definition editor called "DefaultValue" for the new Common Data Column in WDB6
* Fixed various issues posted on Git and Model-Changing.net

## 1.0.4

### Changes
* Both Mac and Windows shortcuts for redo are now supported (Control + Shift + Z and Control + Y)
* Many fixes and tweaks
* Column size mode dropdown has been added
* WDB5 has be reworked due to new understanding of how it works
* Definition files have been updated for TBC, WotLK and WoD (Thanks to Kallar)
* Definitions have been added for 7.0.1
* A colour picker tool has been added for editing LightData and LightIntBand
* A basic Text Editor has been added to the right click menu to help with large strings such as in SceneScript.db2
* Added a Player Location tool that reads the player's current X, Y and Z co-ordinates from the WoW client's memory. This is an attempt to help with editing files such as TaxiPathNode that require a lot of co-ordinates to be entered.
   * Offsets for 1.12.1, 2.4.3 and 3.3.5a are included and tested
   * Includes the ability to add, edit and remove offsets


## 1.0.3

### Changes
* WCH8 (the new Legion ADB format) is now supported for both reading and writing
* Save As (F12) has been added meaning Save doesn't prompt for file location
* SQL export now works with all "sql_mode"s
* SQL import now automatically fixes NULL values
* Datagrid context menu has been added to all cells as well row headers
* Supports opening multiple selected files at once
* Supports drag and drop files to open (based on Windows permissions)
* Added JSON as an export type
* Added version check based on Github releases

The application now runs as a single instance when launched in GUI mode. This means any files opened thereafter are sent to the running instance instead of opening the application multiple times. In certain situations this is not ideal, so a button has been added to the Load Definition screen which will open a new instance of the application for all files waiting to be loaded.

A range of command line arguments have been added (this is still early stages). This is the beginning of providing an interface so tasks can be automated/invoked via batch scripts and external programs.
Commands currently include (see the help file for more information):
* Console Mode which opens the software in a console session
* Extract which will extract all DBC/DB2 files from a MPQ archive or CASC directory based on a filter string
* Export which exports a specific file to CSV, JSON or SQL
* SQL dump which dumps a file's data directly into a MySQL database table

## 1.0.2

### Changes
* WCH7 is now supported for both reading and writing
* Fixed more bugs
* Have added some options to the CSV import to fix duplicate Ids in the source data
   * Option 1 (default): Increment Ids so none are duplicated
   * Option 2: Take the newest data of the duplicated Ids
* DataGridView has been overhauled meaning it is slightly faster, more efficient and now has cache and state systems
* Undo and Redo has been rebuilt and now triggers on almost all events
   * Undo/Redo history is still lost on changing file
   * Deleting more than 25 rows is a permanent action (cannot be undone) due to BindingSource + DataGridView performance limitations
* New rows now have default values for their value type and value types are now enforced
* Added a new option to the column filter to hide all empty columns (the eye button)
* Added a clear row right click option which sets the row to default values
* Added the ability to insert a row with a specific id [Ctrl + I]
* Added a new line shortcut to avoid having to scroll to the bottom of the grid [Ctrl + N]

Big thanks to UncleChristiof and Skarn for ideas, bugs and useability issues

## 1.0.1

### Changes
* Added a FileSystemWatcher so that definitions are reloaded as soon as any definition file is saved meaning the application no longer needs to be restarted
* The program now functions correctly if set as the default program for files (thanks to Skarn for this suggestion)
* Find and Replace now uses a lookup table so is much faster
* Specific errors have been refined and made more human
* The program now no longer stops importing if a string is not found in the string table and instead writes "String not found" in the cell's value. This is an attempt to combat some incorrectly modified DB2 files I've come across
* Definitions have been updated for WoD 6.2.4 which is at ~98% now
* A fair amount of optimisation and tinkering
