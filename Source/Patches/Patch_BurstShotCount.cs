using HarmonyLib;
using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch(typeof(Verb), nameof(Verb.BurstShotCount), MethodType.Getter)]

    public static class Patch_BurstShotCount
    {
        static void Postfix(Verb __instance, ref int __result)
        {
            Pawn pawn = __instance.CasterPawn;
            if (pawn == null) return;

            var mode = pawn.GetFireMode();
            var m = FireModeDB.GetBurstCount(mode);

            __result = Mathf.Max(1,
                Mathf.RoundToInt(__result * m));
        }
    }
}
