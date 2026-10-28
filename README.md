# Ctrl+C

A Slay the Spire 2 mod that copies the current run's seed to the system clipboard when you press **Ctrl+C**.

Copies only the seed, with no message or popup. Works while a run is open, including its game-over screen. Outside a run, the shortcut is unchanged. Key repeats and additional modifiers are ignored.

## Build and install

Requires a .NET SDK supporting `net9.0` and a local game installation.

```sh
dotnet build -c Release
./install.sh
```

Default Steam paths are supported on Windows, macOS, and WSL. For another installation, pass `-p:GameDir="/path/to/Slay the Spire 2"` or `-p:GameDataDir="/path/to/data_sts2_platform"`.

Alternatively, copy `CtrlC.dll` and `CtrlC.json` from `dist/CtrlC/` into the game's `mods/CtrlC/` directory. On macOS, `mods/` is inside `SlayTheSpire2.app/Contents/MacOS/`.

Restart the game after installing. To uninstall, remove `mods/CtrlC/`.

## Development checks

The checks require the .NET 9 runtime, matching the game's Harmony library.

```sh
dotnet run --project tests/Behavior
dotnet run --project tests/Compatibility -- \
  "/path/to/data_sts2_platform" \
  "dist/CtrlC/CtrlC.dll"
```

These check input behavior with stubs and patch compatibility against the installed game. The system clipboard still needs an in-game check.
