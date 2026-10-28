using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;

namespace CtrlC;

[HarmonyPatch(typeof(NGame), nameof(NGame._Input))]
internal static class CopySeedPatch
{
    private static readonly AccessTools.FieldRef<NRun, RunState> State =
        AccessTools.FieldRefAccess<NRun, RunState>("_state");

    internal static void Prefix(NGame __instance, InputEvent inputEvent)
    {
        if (inputEvent is not InputEventKey
            {
                Pressed: true,
                Echo: false,
                CtrlPressed: true,
                AltPressed: false,
                ShiftPressed: false,
                MetaPressed: false,
                Keycode: Key.C
            }
            || NRun.Instance is not { } run)
        {
            return;
        }

        DisplayServer.ClipboardSet(State(run).Rng.StringSeed);
        // Prevent a focused text field from overwriting the seed with its selection.
        __instance.GetViewport().SetInputAsHandled();
    }
}
