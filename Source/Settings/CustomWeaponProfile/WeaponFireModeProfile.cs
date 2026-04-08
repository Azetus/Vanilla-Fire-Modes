using Verse;

namespace VFM_VanillaFireModes.Settings.CustomWeaponProfile;

public class WeaponFireModeProfile : IExposable
{
    public string defName;

    public FireModeProfile Default;
    public FireModeProfile Precision;
    public FireModeProfile Burst;
    public FireModeProfile Suppression;

    public WeaponFireModeProfile(string defName)
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
        Scribe_Deep.Look(ref Default, "Default", new FireModeProfile());
        Scribe_Deep.Look(ref Precision, "Precision", new FireModeProfile());
        Scribe_Deep.Look(ref Burst, "Burst", new FireModeProfile());
        Scribe_Deep.Look(ref Suppression, "Suppression", new FireModeProfile());
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            Default ??= new FireModeProfile();
            Precision ??= new FireModeProfile();
            Burst ??= new FireModeProfile();
            Suppression ??= new FireModeProfile();
        }
    }
}