# AutoSaveAddin

Small AVEVA E3D addin for timed `SaveWork`.

## Install

Download the latest release zip from GitHub Releases.

Copy the matching `AutoSaveAddin.dll` to the addin folder that your E3D environment loads on startup. This is usually a site or user addin location configured by the administrator, not necessarily `Program Files`.

Use:

- `E3D-2.10/AutoSaveAddin.dll` for AVEVA E3D 2.10
- `E3D-3.1/AutoSaveAddin.dll` for AVEVA E3D 3.1

## Build

Build `AutoSaveAddin.sln` for .NET Framework 4.8, x86.

Configurations:

- `Release`: AVEVA E3D 2.10
- `Release3`: AVEVA E3D 3.1

The project uses installed AVEVA DLLs as build references. Default reference roots:

```text
C:\Program Files (x86)\AVEVA\Everything3D2.10\
C:\Program Files (x86)\AVEVA\Everything3D3.1\
```

Optional build copy:

```powershell
MSBuild AutoSaveAddin.sln /p:Configuration=Release /p:CopyToAveva=true
MSBuild AutoSaveAddin.sln /p:Configuration=Release3 /p:CopyToAveva=true
```

## Use

Command key:

```text
AutoSaveFormCmd
```

Settings JSON:

```text
%APPDATA%\AutoSaveAddin\settings.json
```

Features:

- autosave interval;
- optional confirmation;
- strict confirmation mode;
- optional `unclaim all` after save;
- RU/EN UI by system language.
