using UnityEngine;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes.Utilities
{
    public static class FireModeDB
    {
        public static VanillaFireModesModSetting Settings => VanillaFireModes.settings;

        public static float GetWarmup(FireMode mode)
        {
            return mode switch
            {
                FireMode.Precision => Settings.precisionWarmup,
                FireMode.Burst => Settings.burstWarmup,
                FireMode.Suppression => Settings.suppressionWarmup,
                _ => 1f
            };
        }

        public static float GetCooldown(FireMode mode)
        {
            return mode switch
            {
                FireMode.Precision => Settings.precisionCooldown,
                FireMode.Burst => Settings.burstCooldown,
                FireMode.Suppression => Settings.suppressionCooldown,
                _ => 1f
            };
        }

        public static float GetAccuracy(FireMode mode)
        {
            return mode switch
            {
                FireMode.Precision => Settings.precisionAccuracy,
                FireMode.Burst => Settings.burstAccuracy,
                FireMode.Suppression => Settings.suppressionAccuracy,
                _ => 1f
            };
        }

        public static int GetBurstCount(FireMode mode, int baseBurstCount)
        {
            return mode switch
            {
                FireMode.Precision => GetBurstCount_Precision(baseBurstCount, Settings.precisionBurstOption),
                FireMode.Burst => GetBurstCount_Burst(baseBurstCount, Settings.burstBurstOption),
                FireMode.Suppression => GetBurstCount_Suppression(baseBurstCount, Settings.suppressionBurstOption),
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
                BurstShotOption.Linear => Mathf.Max(1, Mathf.RoundToInt(baseBurstCount * linearMult)),
                BurstShotOption.Additive => Mathf.Max(1, baseBurstCount + addBonus),
                BurstShotOption.Tent => Mathf.Max(1, handleTentFunc(baseBurstCount, tentMaxMult, tentSlopeK, tentPeak)),
                BurstShotOption.Adaptive => Mathf.Max(1, handleAdaptFunc(baseBurstCount, adaptBonus, adaptPeak)),
                _ => baseBurstCount
            };

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
