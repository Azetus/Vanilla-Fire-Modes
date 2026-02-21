using RimWorld;
using Verse;
using VFM_VanillaFireModes.Stat;

[StaticConstructorOnStartup]
public static class VFM_StatPatcher
{
    static VFM_StatPatcher()
    {
        InjectPart(StatDefOf.ShootingAccuracyPawn, new VFM_FireMode_ShootingAccuracyPawnPart());
        InjectPart(StatDefOf.RangedCooldownFactor, new VFM_FireMode_RangedCooldownFactorPart());
        InjectPart(StatDefOf.AimingDelayFactor, new VFM_FireMode_AimingDelayFactorPart());
    }

    private static void InjectPart<T>(StatDef stat, T part) where T : VFM_FireMode_StatPart, new()
    {
        if (stat.parts == null)
            stat.parts = new List<StatPart>();

        if (!stat.parts.Any(p => p is T))
            stat.parts.Add(part);
    }
}