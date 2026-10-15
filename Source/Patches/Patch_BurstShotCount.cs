using HarmonyLib;
using RimWorld;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    /// <summary>
    /// 仅对调用链经过 Verb.WarmupComplete 的 Verb 生效。
    /// Verb_Spray、Verb_ShootBeam 自行初始化射击，不经过该入口，因此不建立缓存，避免干扰其路径生成和索引。
    /// 每轮锁定值按 Verb 实例关联，不写入存档；失效的状态对象可供下一轮复用。
    /// </summary>
    [HarmonyPatch]
    public static class Patch_BurstShotCount
    {
        private sealed class BurstCacheState
        {
            public int Count;
            public bool IsValid;
        }

        // 弱键不会阻止废弃 Verb 被回收，也不会让读档后的同 loadID 新实例命中旧状态。
        private static readonly ConditionalWeakTable<Verb, BurstCacheState> _burstCache = new ConditionalWeakTable<Verb, BurstCacheState>();

        // 在原版初始化本轮剩余发数前锁定数量。
        [HarmonyPatch(typeof(Verb), nameof(Verb.WarmupComplete))]
        [HarmonyPrefix]
        public static void LockCount(Verb __instance)
        {
            if (_burstCache.TryGetValue(__instance, out var cached))
                cached.IsValid = false;

            if (ShouldModify(__instance, out var pawn, out var weapon))
            {
                var mode = pawn.VFM_GetFireMode();
                var weaponDefName = weapon.def.defName;
                // 先使旧值失效，再通过公共 getter 保留原版武器词条及其他 Mod 的修正。
                var m = FireModeDB.GetBurstCount(mode, __instance.BurstShotCount, weaponDefName);
                cached ??= _burstCache.GetValue(__instance, static _ => new BurstCacheState());
                cached.Count = Mathf.Max(1, m);
                cached.IsValid = true;
            }
        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.BurstShotCount), MethodType.Getter)]
        [HarmonyPostfix]
        public static void BurstShotCountPostFix(Verb __instance, ref int __result)
        {
            if (_burstCache.TryGetValue(__instance, out var cached) && cached.IsValid &&
                ShouldModify(__instance, out _, out _))
            {
                __result = cached.Count;
            }
        }

        [HarmonyPatch(typeof(Verb), nameof(Verb.VerbTick))]
        [HarmonyPostfix]
        public static void Postfix_Cleanup(Verb __instance)
        {
            if (__instance.state != VerbState.Bursting &&
                _burstCache.TryGetValue(__instance, out var cached) && cached.IsValid)
            {
                cached.IsValid = false;
            }
        }

        private static bool ShouldModify(Verb verb, out Pawn pawn, out ThingWithComps weapon)
        {
            pawn = null!;
            weapon = null!;
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
