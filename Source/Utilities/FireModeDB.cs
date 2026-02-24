using UnityEngine;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes.Utilities
{
    public static class FireModeDB
    {
        public static VanillaFireModesModSetting Settings => VanillaFireModes.settings;

        public static float GetWarmup(VFM_FireMode mode)
        {
            return mode switch
            {
                VFM_FireMode.Precision => Settings.precisionWarmup,
                VFM_FireMode.Burst => Settings.burstWarmup,
                VFM_FireMode.Suppression => Settings.suppressionWarmup,
                _ => 1f
            };
        }

        public static float GetCooldown(VFM_FireMode mode)
        {
            return mode switch
            {
                VFM_FireMode.Precision => Settings.precisionCooldown,
                VFM_FireMode.Burst => Settings.burstCooldown,
                VFM_FireMode.Suppression => Settings.suppressionCooldown,
                _ => 1f
            };
        }

        public static float GetAccuracy(VFM_FireMode mode)
        {
            return mode switch
            {
                VFM_FireMode.Precision => Settings.precisionAccuracy,
                VFM_FireMode.Burst => Settings.burstAccuracy,
                VFM_FireMode.Suppression => Settings.suppressionAccuracy,
                _ => 1f
            };
        }

        public static int GetBurstCount(VFM_FireMode mode, int baseBurstCount)
        {
            return mode switch
            {
                VFM_FireMode.Precision => GetBurstCount_Precision(baseBurstCount, Settings.precisionBurstOption),
                VFM_FireMode.Burst => GetBurstCount_Burst(baseBurstCount, Settings.burstBurstOption),
                VFM_FireMode.Suppression => GetBurstCount_Suppression(baseBurstCount, Settings.suppressionBurstOption),
                _ => baseBurstCount
            };
        }

        private static int GetBurstCount_Precision(int baseBurstCount, BurstShotOption burstShotOption)
        {
            return GetBurstCountByOption(
                baseBurstCount,
                burstShotOption,
                Settings.precisionBurstLinearMultiplier,
                Settings.precisionBurstAdditiveBonus,
                Settings.precisionBurstTentMaxMultiplier,
                Settings.precisionBurstTentSlopeK,
                Settings.precisionBurstTentPeakOffset,
                Settings.precisionBurstAdaptiveBonus,
                Settings.precisionBurstAdaptivePeakOffset
                );
        }

        private static int GetBurstCount_Burst(int baseBurstCount, BurstShotOption burstShotOption)
        {
            return GetBurstCountByOption(
                baseBurstCount,
                burstShotOption,
                Settings.burstBurstLinearMultiplier,
                Settings.burstBurstAdditiveBonus,
                Settings.burstBurstTentMaxMultiplier,
                Settings.burstBurstTentSlopeK,
                Settings.burstBurstTentPeakOffset,
                Settings.burstBurstAdaptiveBonus,
                Settings.burstBurstAdaptivePeakOffset
                );
        }

        private static int GetBurstCount_Suppression(int baseBurstCount, BurstShotOption burstshotOption)
        {
            return GetBurstCountByOption(
                baseBurstCount,
                burstshotOption,
                Settings.suppressionBurstLinearMultiplier,
                Settings.suppressionBurstAdditiveBonus,
                Settings.suppressionBurstTentMaxMultiplier,
                Settings.suppressionBurstTentSlopeK,
                Settings.suppressionBurstTentPeakOffset,
                Settings.suppressionBurstAdaptiveBonus,
                Settings.suppressionBurstAdaptivePeakOffset
                );
        }

        private static int GetBurstCountByOption(
            int baseBurstCount,
            BurstShotOption burstOption,
            float linearMult,
            int addBonus,
            float tentMaxMult,
            float tentSlopeK,
            int tentPeak,
            int adaptBonus,
            int adaptPeak
            )
        {
            return burstOption switch
            {
                BurstShotOption.Linear => Mathf.Max(1, handleLinear(baseBurstCount, linearMult)),
                BurstShotOption.Additive => Mathf.Max(1, handleAdditive(baseBurstCount, addBonus)),
                BurstShotOption.Tent => Mathf.Max(1, handleTentFunc(baseBurstCount, tentMaxMult, tentSlopeK, tentPeak)),
                BurstShotOption.Adaptive => Mathf.Max(1, handleAdaptFunc(baseBurstCount, adaptBonus, adaptPeak)),
                _ => baseBurstCount
            };

        }

        private static int handleLinear(int baseBurstCount, float linearMult)
        {
            return Utils.GetBurstShotCountByMultiplier(baseBurstCount, linearMult);
        }

        private static int handleAdditive(int baseBurstCount, int addBonus)
        {
            return Utils.GetBurstShotCountByBonus(baseBurstCount, addBonus);
        }

        private static int handleTentFunc(int baseBurstCount, float tentMaxMult, float tentSlopeK, int tentPeak)
        {
            return Utils.GetBurstShotCountByTentFunction(baseBurstCount, tentMaxMult, tentSlopeK, tentPeak);
        }

        private static int handleAdaptFunc(int baseBurstCount, int adaptBonus, int adaptPeak)
        {
            return Utils.GetBurstShotCountByMod(baseBurstCount, adaptBonus, adaptPeak);
        }
    }
}
