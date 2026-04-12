using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    /// <summary>
    /// 仅对未重写 WarmupComplete 的 Verb 子类（如 Verb_Shoot）生效。
    /// Verb_Spray 及其子类 Verb_ArcSpray 依赖 verbProps.sprayNumExtraCells（BurstShotCount 必须小于此值）创建子弹路径列表 path。
    /// 子弹路径列表 List[IntVec3] path在 WarmupComplete 中初始化。
    /// 任何重写了 WarmupComplete 方法的 Verb 都不会计入缓存，而被自动忽略，以防止索引越界。
    /// </summary>
    [HarmonyPatch]
    public static class Patch_BurstShotCount
    {
        // 用来存储当前正在执行射击的 Verb 及其被锁定的连发数
        private static readonly Dictionary<string, int> _burstCache = new Dictionary<string, int>();

        // 对于重写了 WarmupComplete 的 Verb 子类不会执行
        [HarmonyPatch(typeof(Verb), nameof(Verb.WarmupComplete))]
        [HarmonyPrefix]
        public static void LockCount(Verb __instance)
        {
            if (__instance.loadID == null) return;
            if (ShouldModify(__instance, out Pawn pawn, out ThingWithComps weapon))
            {
                var mode = pawn.VFM_GetFireMode();
                var weaponDefName = weapon.def.defName;
                var m = FireModeDB.GetBurstCount(mode, __instance.BurstShotCount, weaponDefName);
                // 锁定数值
                _burstCache[__instance.loadID] = Mathf.Max(1, m);
            }
        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.BurstShotCount), MethodType.Getter)]
        [HarmonyPostfix]
        public static void BurstShotCountPostFix(Verb __instance, ref int __result)
        {
            if (__instance.loadID == null) return;
            if(ShouldModify(__instance, out Pawn pawn, out ThingWithComps weapon))
            {
                if (_burstCache.TryGetValue(__instance.loadID, out var cached))
                {
                    __result =  Mathf.Max(1, cached);
                }
            }
        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.VerbTick))]
        [HarmonyPostfix]
        public static void Postfix_Cleanup(Verb __instance)
        {
            if (__instance.loadID == null) return;
            if (_burstCache.ContainsKey(__instance.loadID) && __instance.state != VerbState.Bursting)
            {
                _burstCache.Remove(__instance.loadID);
            }
        }

        private static bool ShouldModify(Verb verb, out Pawn pawn, out ThingWithComps weapon)
        {
            pawn = null;
            weapon = null;
            if (verb.CasterPawn is not Pawn p) return false;
            if (verb.verbProps == null) return false;
            if (verb is Verb_ShootOneUse) return false;
            if (verb.verbProps.IsMeleeAttack) return false;
            
            var eq = verb.EquipmentSource;
            if (eq == null || eq.def == null || !eq.def.IsRangedWeapon || eq.def.defName.NullOrEmpty()) return false;

            pawn = p;
            weapon = eq;
            return true;
        }
    }
}