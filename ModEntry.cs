using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace CtrlC;

[ModInitializer(nameof(Initialize))]
public static class ModEntry
{
    public static void Initialize()
    {
        RuntimeHelpers.RunClassConstructor(typeof(CopySeedPatch).TypeHandle);
        new Harmony("james.sts2.ctrl-c").PatchAll(typeof(ModEntry).Assembly);
    }
}
