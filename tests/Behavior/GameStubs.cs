namespace Godot
{
    public class InputEvent;
    public enum Key { C, V }
    public sealed class InputEventKey : InputEvent
    {
        public bool Pressed { get; set; }
        public bool Echo { get; set; }
        public bool CtrlPressed { get; set; }
        public bool AltPressed { get; set; }
        public bool ShiftPressed { get; set; }
        public bool MetaPressed { get; set; }
        public Key Keycode { get; set; }
    }

    public static class DisplayServer
    {
        public static string Clipboard { get; set; } = "";
        public static void ClipboardSet(string text) => Clipboard = text;
    }

    public sealed class Viewport
    {
        public bool Handled { get; private set; }
        public void SetInputAsHandled() => Handled = true;
    }
}

namespace MegaCrit.Sts2.Core.Runs
{
    public sealed class RunState(string seed)
    {
        public RunRngSet Rng { get; } = new(seed);
    }

    public sealed class RunRngSet(string seed)
    {
        public string StringSeed { get; } = seed;
    }
}

namespace MegaCrit.Sts2.Core.Nodes
{
    public sealed class NRun(Runs.RunState state)
    {
        private readonly Runs.RunState _state = state;
        public Runs.RunState State => _state;
        public static NRun? Instance { get; set; }
    }

    public sealed class NGame
    {
        private readonly Godot.Viewport _viewport = new();
        public Godot.Viewport GetViewport() => _viewport;
        public void _Input(Godot.InputEvent inputEvent) { }
    }
}
