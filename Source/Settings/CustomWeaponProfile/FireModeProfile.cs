using Verse;

namespace VFM_VanillaFireModes.Settings.CustomWeaponProfile;

public class FireModeProfile : IExposable
{
    public float accuracyMultiplier = 1.0f;
    public float warmupMultiplier = 1.0f;
    public float cooldownMultiplier = 1.0f;
    public int burstShotCount = 1;

    public FireModeProfile(
        float accuracyMultiplier = 1.0f,
        float warmupMultiplier = 1.0f,
        float cooldownMultiplier = 1.0f,
        int burstShotCount = 1)
    {
        this.accuracyMultiplier = accuracyMultiplier;
        this.warmupMultiplier = warmupMultiplier;
        this.cooldownMultiplier = cooldownMultiplier;
        this.burstShotCount = burstShotCount;
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref accuracyMultiplier, "accuracyMultiplier", 1.0f);
        Scribe_Values.Look(ref warmupMultiplier, "warmupMultiplier", 1.0f);
        Scribe_Values.Look(ref cooldownMultiplier, "cooldownMultiplier", 1.0f);
        Scribe_Values.Look(ref burstShotCount, "burstShotCount", 1);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (accuracyMultiplier <= 0) accuracyMultiplier = 1.0f;
            if (warmupMultiplier <= 0) warmupMultiplier = 1.0f;
            if (cooldownMultiplier <= 0) cooldownMultiplier = 1.0f;
            if (burstShotCount < 1) burstShotCount = 1;
        }
    }
}