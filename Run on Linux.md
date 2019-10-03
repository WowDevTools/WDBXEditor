## Run WDBX on Ubuntu with wine

Install last version of wine from this guide: https://tecadmin.net/install-wine-on-ubuntu/

Install also winetricks using:
```
$ sudo apt install winetricks
```

Delete all previous dotnet versions that you have installed and run:

```
$ winetricks
```

Running winetricks, Wine Mono Installer will appear, just wait and install.
This will solve the following error:
```
0009:err:mscoree:CLRRuntimeInfo_GetRuntimeHost Wine Mono is not installed
```


Run:
```
winetricks dotnet461
```

Here you go, now you can run WDBX.exe


## Troubleshooting:

If **Wine Mono Installer** does not appear or you get some errors, try to clean everything about wine and winetricks using:
```
$ winetricks
```
- `Select the default wineprefix`
- `Install a Windows DLL or component`
- last option `Delete ALL DATA AND APPLICATIONS INSIDE THIS WINEPREFIX` <-- be careful, this will delete everything inside your ~/.wine directory.
- follow again the steps


### Credits

- [Helias](https://github.com/Helias)

Tested with wine 4.0.2, Ubuntu 18.04.3 LTS and with [WDBX 1.1.9a](https://github.com/WowDevTools/WDBXEditor/releases/tag/1.1.9.a)
