using CtrlC;
using Godot;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;

NRun.Instance = new NRun(new RunState("4LJH6P4QJACB"));
Check(Shortcut(), "4LJH6P4QJACB", true);
NRun.Instance = new NRun(new RunState("NEXT-SEED"));
Check(Shortcut(), "NEXT-SEED", true);

foreach (var change in new Action<InputEventKey>[]
{
    key => key.Pressed = false,
    key => key.Echo = true,
    key => key.CtrlPressed = false,
    key => key.AltPressed = true,
    key => key.ShiftPressed = true,
    key => key.MetaPressed = true,
    key => key.Keycode = Key.V
})
{
    var key = Shortcut();
    change(key);
    Check(key, "previous clipboard", false);
}
Check(new InputEvent(), "previous clipboard", false);
NRun.Instance = null;
Check(Shortcut(), "previous clipboard", false);
Console.WriteLine("PASS: Ctrl+C, changing runs, no run, unrelated input, modifiers, releases, and repeats.");

static InputEventKey Shortcut() => new() { Pressed = true, CtrlPressed = true, Keycode = Key.C };

static void Check(InputEvent input, string expected, bool handled)
{
    DisplayServer.Clipboard = "previous clipboard";
    var game = new NGame();
    CopySeedPatch.Prefix(game, input);
    if (DisplayServer.Clipboard != expected || game.GetViewport().Handled != handled)
    {
        throw new Exception($"Unexpected clipboard or input handling: {DisplayServer.Clipboard}");
    }
}
