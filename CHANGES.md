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