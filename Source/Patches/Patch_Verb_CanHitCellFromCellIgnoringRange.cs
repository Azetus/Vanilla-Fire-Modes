using HarmonyLib;
using System.Reflection;
using Verse;
using static VFM_VanillaFireModes.Utilities.Utils;

namespace VFM_VanillaFireModes.Patches
{
    // 丢失视野不中断连射
    [HarmonyPatch]
    public static class Patch_Verb_CanHitCellFromCellIgnoringRange
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Verb), "CanHitCellFromCellIgnoringRange");
        }

        [HarmonyPrefix]
        static bool Prefix(Verb __instance, IntVec3 sourceSq, IntVec3 targetLoc, ref bool __result, bool includeCorners = false)
        {
            if (__instance.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(__instance.caster.Map) || __instance.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
            {
                // __result = false;
                // return false;
                return true;
            }
            if (__instance.verbProps.requireLineOfSight)
            {
                if (!__instance.Bursting)
                    return true;

                if (!CanKeepBurstingWhileLoS(__instance))
                    return true;

                __result = true;
                return false; // 阻止原方法执行
            }
            return true; // 原方法继续执行
        }
    }
}
