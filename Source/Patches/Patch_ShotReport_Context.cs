using HarmonyLib;
using RimWorld;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch]
    public static class Patch_ShotReport_Context
    {
        [ThreadStatic]
        private static Verb? _activeVerb;

        [HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitReportFor))]
        [HarmonyPrefix]
        static void SaveVerb(Verb verb) => _activeVerb = verb;

        [HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitReportFor))]
        [HarmonyPostfix]
        static void CleanVerb() => _activeVerb = null;

        [HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitFactorFromShooter))]
        [HarmonyPostfix]
        static void Postfix(Thing caster, ref float __result)
        {
            if (caster is not Pawn pawn) return;

            if (pawn.CurJobDef == JobDefOf.Hunt) return;

            if(_activeVerb == null) return;
            if (_activeVerb.verbProps == null) return;

            if (!_activeVerb.verbProps.IsMeleeAttack && _activeVerb.EquipmentSource != null)
            {
                if (_activeVerb.EquipmentSource.def.IsRangedWeapon)
                {
                    var mode = pawn.VFM_GetFireMode();
                    var m = FireModeDB.GetAccuracy(mode);

                    __result *= m;
                }
            }

        }
    }
}
