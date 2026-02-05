using HarmonyLib;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch(typeof(Verb), nameof(Verb.WarmupTime), MethodType.Getter)]
    public static class Patch_GetWarmupTime
    {
        static void Postfix(Verb __instance, ref float __result)
        {
            Pawn pawn = __instance.CasterPawn;
            if (pawn == null) return;

            var mode = pawn.GetFireMode();
            var m = FireModeDB.GetWarmup(mode);

            __result *= m;
        }
    }
}
