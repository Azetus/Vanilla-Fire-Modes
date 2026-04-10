using Verse;

namespace VFM_VanillaFireModes.Settings.CustomWeaponProfile;

public class VFM_WeaponProfile : IExposable
{
    public string defName;

    public VFM_FireModeProfile Default;
    public VFM_FireModeProfile Precision;
    public VFM_FireModeProfile Burst;
    public VFM_FireModeProfile Suppression;

    public VFM_WeaponProfile(string defName)
    {
        this.defName = defName;
    }

    public bool defIsValid()
    {
        return !defName.NullOrEmpty() &&
               DefDatabase<ThingDef>.GetNamedSilentFail(defName) != null;
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref defName, "defName");
        Scribe_Deep.Look(ref Default, "Default", new VFM_FireModeProfile());
        Scribe_Deep.Look(ref Precision, "Precision", new VFM_FireModeProfile());
        Scribe_Deep.Look(ref Burst, "Burst", new VFM_FireModeProfile());
        Scribe_Deep.Look(ref Suppression, "Suppression", new VFM_FireModeProfile());
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            Default ??= new VFM_FireModeProfile();
            Precision ??= new VFM_FireModeProfile();
            Burst ??= new VFM_FireModeProfile();
            Suppression ??= new VFM_FireModeProfile();
        }
    }
}