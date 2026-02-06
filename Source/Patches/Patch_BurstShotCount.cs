using HarmonyLib;
using RimWorld;
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
            // if (!pawn.Drafted) return;
            if (pawn.CurJobDef == JobDefOf.Hunt) return;

            if (__instance is Verb_ShootOneUse) return;
            if (__instance is Verb_ShootBeam) return;

            if (__instance.verbProps == null) return;
            if (!__instance.verbProps.IsMeleeAttack && __instance.EquipmentSource != null)
            {
                if (__instance.EquipmentSource.def.IsRangedWeapon)
                {
                    var mode = pawn.GetFireMode();
                    var m = FireModeDB.GetBurstCount(mode);

                    __result = Mathf.Max(1, Mathf.RoundToInt(__result * m));
                }
            }


        }
    }
}
