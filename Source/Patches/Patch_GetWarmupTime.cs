using HarmonyLib;
using RimWorld;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch(typeof(Verb), nameof(Verb.WarmupTime), MethodType.Getter)]
    public static class Patch_GetWarmupTime
    {
        static void Postfix(Verb __instance, ref float __result)
        {
            if (__instance.CasterPawn is not Pawn pawn) return;
            if (pawn.CurJobDef == JobDefOf.Hunt) return;
            if (__instance.verbProps == null) return;

            if (!__instance.verbProps.IsMeleeAttack && __instance.EquipmentSource != null)
            {
                if (__instance.EquipmentSource.def.IsRangedWeapon)
                {
                    var mode = pawn.GetFireMode();
                    var m = FireModeDB.GetWarmup(mode);

                    __result *= m;
                }
            }

        }
    }
}
