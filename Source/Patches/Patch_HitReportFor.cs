using HarmonyLib;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitFactorFromShooter))]
    public static class Patch_HitReportFor
    {
        static void Postfix(Thing caster, ref float __result)
        {
            if (caster is not Pawn pawn) return;

            var mode = pawn.GetFireMode();
            var m = FireModeDB.GetAccuracy(mode);

            __result *= m;
        }
    }
}
