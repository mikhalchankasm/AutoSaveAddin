# AutoSaveAddin

Small AVEVA E3D addin for timed `SaveWork`.

## Install

Build `AutoSaveAddin.sln` for .NET Framework 4.8, x86.

Default AVEVA references point to:

```text
C:\Program Files (x86)\AVEVA\Everything3D2.10\
```

Copy the built DLL into the E3D addin load location used by your site.

Optional build copy:

```powershell
MSBuild AutoSaveAddin.sln /p:Configuration=Release /p:CopyToAveva=true
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
