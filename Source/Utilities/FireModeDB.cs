using UnityEngine;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.Utilities
{
    public static class FireModeDB
    {
        public static VanillaFireModesModSetting Settings => VanillaFireModes.settings;

        public static float GetWarmup(VFM_FireMode mode, string? weaponDefName)
        {
            if (weaponDefName != null && Settings.CustomWeaponProfiles.TryGetValue(weaponDefName, out VFM_WeaponProfile weaponProfile))
            {
                return mode switch
                {
                    VFM_FireMode.Precision => weaponProfile.Precision.warmupMultiplier,
                    VFM_FireMode.Burst => weaponProfile.Burst.warmupMultiplier,
                    VFM_FireMode.Suppression => weaponProfile.Suppression.warmupMultiplier,
                    VFM_FireMode.Default => weaponProfile.Default.warmupMultiplier,
                    _ => 1f
                };
            }

            return mode switch
            {
                VFM_FireMode.Precision => Settings.precisionWarmup,
                VFM_FireMode.Burst => Settings.burstWarmup,
                VFM_FireMode.Suppression => Settings.suppressionWarmup,
                VFM_FireMode.Default => Settings.defaultWarmup,
                _ => 1f
            };
        }

        public static float GetCooldown(VFM_FireMode mode, string? weaponDefName)
        {
            if (weaponDefName != null && Settings.CustomWeaponProfiles.TryGetValue(weaponDefName, out VFM_WeaponProfile weaponProfile))
            {
                return mode switch
                {
                    VFM_FireMode.Precision => weaponProfile.Precision.cooldownMultiplier,
                    VFM_FireMode.Burst => weaponProfile.Burst.cooldownMultiplier,
                    VFM_FireMode.Suppression => weaponProfile.Suppression.cooldownMultiplier,
                    VFM_FireMode.Default => weaponProfile.Default.cooldownMultiplier,
                    _ => 1f
                };
            }

            return mode switch
            {
                VFM_FireMode.Precision => Settings.precisionCooldown,
                VFM_FireMode.Burst => Settings.burstCooldown,
                VFM_FireMode.Suppression => Settings.suppressionCooldown,
                VFM_FireMode.Default => Settings.defaultCooldown,
                _ => 1f
            };
        }

        public static float GetAccuracy(VFM_FireMode mode, string? weaponDefName)
        {
            if (weaponDefName != null && Settings.CustomWeaponProfiles.TryGetValue(weaponDefName, out VFM_WeaponProfile weaponProfile))
            {
                return mode switch
                {
                    VFM_FireMode.Precision => weaponProfile.Precision.accuracyMultiplier,
                    VFM_FireMode.Burst => weaponProfile.Burst.accuracyMultiplier,
                    VFM_FireMode.Suppression => weaponProfile.Suppression.accuracyMultiplier,
                    VFM_FireMode.Default => weaponProfile.Default.accuracyMultiplier,
                    _ => 1f
                };
            }

            return mode switch
            {
                VFM_FireMode.Precision => Settings.precisionAccuracy,
                VFM_FireMode.Burst => Settings.burstAccuracy,
                VFM_FireMode.Suppression => Settings.suppressionAccuracy,
                VFM_FireMode.Default => Settings.defaultAccuracy,
                _ => 1f
            };
        }

        public static int GetBurstCount(VFM_FireMode mode, int baseBurstCount, string? weaponDefName)
        {
            if (weaponDefName != null && Settings.CustomWeaponProfiles.TryGetValue(weaponDefName, out VFM_WeaponProfile weaponProfile))
            {
                return mode switch
                {
                    VFM_FireMode.Precision => weaponProfile.Precision.burstShotCount,
                    VFM_FireMode.Burst => weaponProfile.Burst.burstShotCount,
                    VFM_FireMode.Suppression => weaponProfile.Suppression.burstShotCount,
                    VFM_FireMode.Default => weaponProfile.Default.burstShotCount,
                    _ => baseBurstCount
                };
            }

            return mode switch
            {
                VFM_FireMode.Precision => GetBurstCount_Precision(baseBurstCount, Settings.precisionBurstOption),
                VFM_FireMode.Burst => GetBurstCount_Burst(baseBurstCount, Settings.burstBurstOption),
                VFM_FireMode.Suppression => GetBurstCount_Suppression(baseBurstCount, Settings.suppressionBurstOption),
                VFM_FireMode.Default => GetBurstCount_Default(baseBurstCount, Settings.defaultBurstOption),
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

        private static int GetBurstCount_Default(int baseBurstCount, BurstShotOption burstShotOption)
        {
            return GetBurstCountByOption(
                baseBurstCount,
                burstShotOption,
                Settings.defaultBurstLinearMultiplier,
                Settings.defaultBurstAdditiveBonus,
                Settings.defaultBurstTentMaxMultiplier,
                Settings.defaultBurstTentSlopeK,
                Settings.defaultBurstTentPeakOffset,
                Settings.defaultBurstAdaptiveBonus,
                Settings.defaultBurstAdaptivePeakOffset
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