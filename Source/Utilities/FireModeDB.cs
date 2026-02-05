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

        public static float GetBurstCount(FireMode mode)
        {
            return mode switch
            {
                FireMode.Precision => Settings.precisionBurst,
                FireMode.Burst => Settings.burstBurst,
                FireMode.Suppression => Settings.suppressionBurst,
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
    }
}
