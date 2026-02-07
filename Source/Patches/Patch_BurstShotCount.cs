using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    [HarmonyPatch]

    public static class Patch_BurstShotCount
    {
        // 用来存储当前正在执行射击的 Verb 及其被锁定的连发数
        [ThreadStatic]
        private static int? lockedBurstCount;

        [HarmonyPatch(typeof(Verb), nameof(Verb.WarmupComplete))]
        [HarmonyPrefix]
        public static void LockCount(Verb __instance)
        {
            if (__instance.CasterPawn is not Pawn pawn) return;
            if (pawn.CurJobDef == JobDefOf.Hunt) return;
            if (__instance.verbProps == null) return;
            if (__instance is Verb_ShootOneUse) return;

            if (!__instance.verbProps.IsMeleeAttack && __instance.EquipmentSource != null)
            {
                if (__instance.EquipmentSource.def.IsRangedWeapon)
                {
                    var mode = pawn.GetFireMode();
                    // var m = FireModeDB.GetBurstCount(mode, __instance.verbProps.burstShotCount);
                    var m = FireModeDB.GetBurstCount(mode, __instance.BurstShotCount);

                    // 锁定数值
                    lockedBurstCount = Mathf.Max(1, m);
                }
            }
        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.BurstShotCount), MethodType.Getter)]
        [HarmonyPostfix]
        public static void BurstShotCountPostFix(Verb __instance, ref int __result)
        {
            if (__instance.CasterPawn is not Pawn pawn) return;
            if (pawn.CurJobDef == JobDefOf.Hunt) return;
            if (__instance.verbProps == null) return;
            if (__instance is Verb_ShootOneUse) return;

            if (!__instance.verbProps.IsMeleeAttack && __instance.EquipmentSource != null)
            {
                if (__instance.EquipmentSource.def.IsRangedWeapon)
                {
                    if (lockedBurstCount.HasValue)
                    {
                        __result = lockedBurstCount.Value;
                    }

                }
            }


        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.VerbTick))]
        [HarmonyPostfix]
        public static void Postfix_Cleanup(Verb __instance)
        {
            if (lockedBurstCount.HasValue && __instance.state != VerbState.Bursting)
            {
                lockedBurstCount = null;
            }
        }
    }
}
