using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

if (args.Length != 2)
{
    throw new ArgumentException("Usage: Compatibility <game-data-directory> <CtrlC.dll>");
}

var gameData = Path.GetFullPath(args[0]);
AssemblyLoadContext.Default.Resolving += (_, name) =>
{
    var path = Path.Combine(gameData, name.Name + ".dll");
    return File.Exists(path) ? AssemblyLoadContext.Default.LoadFromAssemblyPath(path) : null;
};

var mod = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(args[1]));
RuntimeHelpers.RunClassConstructor(mod.GetType("CtrlC.CopySeedPatch", throwOnError: true)!.TypeHandle);
var harmonyAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(gameData, "0Harmony.dll"));
var harmonyType = harmonyAssembly.GetType("HarmonyLib.Harmony", throwOnError: true)!;
var harmony = Activator.CreateInstance(harmonyType, "ctrl-c.compatibility")!;
harmonyType.GetMethod("PatchAll", [typeof(Assembly)])!.Invoke(harmony, [mod]);

var game = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(gameData, "sts2.dll"));
var target = game.GetType("MegaCrit.Sts2.Core.Nodes.NGame", throwOnError: true)!
    .GetMethod("_Input", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)!;
if (harmonyType.GetMethod("GetPatchInfo")!.Invoke(null, [target]) == null)
{
    throw new InvalidOperationException("Harmony did not patch NGame._Input.");
}
Console.WriteLine("PASS: Run-state binding and input patch work against the installed game.");
