using Verse;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Settings.CustomWeaponProfile;

public class VFM_FireModeProfile : IExposable
{
    public float accuracyMultiplier = 1.0f;
    public float warmupMultiplier = 1.0f;
    public float cooldownMultiplier = 1.0f;
    /**
     * BurstShotCount 定义在 Verb 上，但是 Verb 并没有全局唯一的标签来用于区分。
     * 自定义武器的各个 FireModeProfile 的 burstShotCount 直接和武器本身绑定。
     */
    public int burstShotCount = 1;

    public VFM_FireModeProfile()
    {
    }

    public VFM_FireModeProfile(
        float accuracyMultiplier,
        float warmupMultiplier,
        float cooldownMultiplier,
        int burstShotCount)
    {
        this.accuracyMultiplier = accuracyMultiplier;
        this.warmupMultiplier = warmupMultiplier;
        this.cooldownMultiplier = cooldownMultiplier;
        this.burstShotCount = burstShotCount;
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref accuracyMultiplier, nameof(accuracyMultiplier), 1.0f);
        Scribe_Values.Look(ref warmupMultiplier, nameof(warmupMultiplier), 1.0f);
        Scribe_Values.Look(ref cooldownMultiplier, nameof(cooldownMultiplier), 1.0f);
        Scribe_Values.Look(ref burstShotCount, nameof(burstShotCount), 1);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (accuracyMultiplier <= 0) accuracyMultiplier = 1.0f;
            if (warmupMultiplier <= 0) warmupMultiplier = 1.0f;
            if (cooldownMultiplier <= 0) cooldownMultiplier = 1.0f;
            if (burstShotCount < 1) burstShotCount = 1;
        }
    }

    // static methods
    public static VFM_FireModeProfile CreateDefault(int baseBurstShotCount)
    {
        return new VFM_FireModeProfile(
            VanillaFireModes.settings.defaultAccuracy,
            VanillaFireModes.settings.defaultWarmup,
            VanillaFireModes.settings.defaultCooldown,
            FireModeDB.GetBurstCount_Default(baseBurstShotCount)
        );
    }

    public static VFM_FireModeProfile CreatePrecision(int baseBurstShotCount)
    {
        return new VFM_FireModeProfile(
            VanillaFireModes.settings.precisionAccuracy,
            VanillaFireModes.settings.precisionWarmup,
            VanillaFireModes.settings.precisionCooldown,
            FireModeDB.GetBurstCount_Precision(baseBurstShotCount)
        );
    }

    public static VFM_FireModeProfile CreateBurst(int baseBurstShotCount)
    {
        return new VFM_FireModeProfile(
            VanillaFireModes.settings.burstAccuracy,
            VanillaFireModes.settings.burstWarmup,
            VanillaFireModes.settings.burstCooldown,
            FireModeDB.GetBurstCount_Burst(baseBurstShotCount)
        );
    }

    public static VFM_FireModeProfile CreateSuppression(int baseBurstShotCount)
    {
        return new VFM_FireModeProfile(
            VanillaFireModes.settings.suppressionAccuracy,
            VanillaFireModes.settings.suppressionWarmup,
            VanillaFireModes.settings.suppressionCooldown,
            FireModeDB.GetBurstCount_Suppression(baseBurstShotCount)
        );
    }
}