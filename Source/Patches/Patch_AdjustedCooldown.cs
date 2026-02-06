using HarmonyLib;
using RimWorld;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch(typeof(VerbProperties), nameof(VerbProperties.AdjustedCooldown))]
    [HarmonyPatch(new Type[] { typeof(Verb), typeof(Pawn) })]
    public static class Patch_AdjustedCooldown
    {
        static void Postfix(Verb ownerVerb, Pawn attacker, ref float __result)
        {
            if (ownerVerb == null || attacker == null) return;
            // if (!attacker.Drafted) return;
            if (ownerVerb.CasterPawn is not Pawn pawn) return;
            if (pawn.CurJobDef == JobDefOf.Hunt) return;
            // if (attacker.CurJobDef == JobDefOf.Hunt) return;
            if (ownerVerb.verbProps == null) return;

            if (!ownerVerb.verbProps.IsMeleeAttack && ownerVerb.EquipmentSource != null)
            {
                if (ownerVerb.EquipmentSource.def.IsRangedWeapon)
                {
                    var mode = attacker.GetFireMode();
                    var m = FireModeDB.GetCooldown(mode);

                    __result *= m;
                }
            }
        }
    }
}
