using Verse;

namespace VFM_VanillaFireModes.Settings.CustomWeaponProfile;

public class VFM_WeaponProfile : IExposable
{
    public string defName;

    public VFM_FireModeProfile Default;
    public VFM_FireModeProfile Precision;
    public VFM_FireModeProfile Burst;
    public VFM_FireModeProfile Suppression;

    public VFM_WeaponProfile()
    {
        // this.Default = new VFM_FireModeProfile();
        // this.Precision = new VFM_FireModeProfile();
        // this.Burst = new VFM_FireModeProfile();
        // this.Suppression = new VFM_FireModeProfile();
    }

    public VFM_WeaponProfile(string defName,
        VFM_FireModeProfile Default,
        VFM_FireModeProfile Precision,
        VFM_FireModeProfile Burst,
        VFM_FireModeProfile Suppression)
    {
        this.defName = defName;
        this.Default = Default;
        this.Precision = Precision;
        this.Burst = Burst;
        this.Suppression = Suppression;
    }

    public bool defIsValid()
    {
        return !defName.NullOrEmpty() &&
               DefDatabase<ThingDef>.GetNamedSilentFail(defName) != null;
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref defName, nameof(defName));
        Scribe_Deep.Look(ref Default, nameof(Default));
        Scribe_Deep.Look(ref Precision, nameof(Precision));
        Scribe_Deep.Look(ref Burst, nameof(Burst));
        Scribe_Deep.Look(ref Suppression, nameof(Suppression));
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            Default ??= new VFM_FireModeProfile();
            Precision ??= new VFM_FireModeProfile();
            Burst ??= new VFM_FireModeProfile();
            Suppression ??= new VFM_FireModeProfile();
        }
    }
    
}