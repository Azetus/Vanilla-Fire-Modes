using RimWorld;
using Verse;
using VFM_VanillaFireModes.Comps;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Stat
{
    public abstract class VFM_FireMode_StatPart : StatPart
    {
        protected abstract float GetFactor(FireMode mode);
        public override void TransformValue(StatRequest req, ref float val)
        {
            VFM_PawnCompFireMode? comp = TryGetComp(req);
            if (comp == null) return;

            val *= GetFactor(comp.curMode);
        }

        public override string? ExplanationPart(StatRequest req)
        {
            VFM_PawnCompFireMode? comp = TryGetComp(req);
            if (comp == null) return null;

            var factor = GetFactor(comp.curMode);

            return "VFM_StatPart_Label"
                .Translate(
                    Utils.GetFireModeLabelFor(comp.curMode),
                    Utils.ToPercentString(factor)
                );
        }

        private static VFM_PawnCompFireMode? TryGetComp(StatRequest req)
        {

            if (!req.HasThing) return null;
            if (req.Thing is not Pawn pawn) return null;

            return pawn.TryGetComp<VFM_PawnCompFireMode>();
        }

    }
    public class VFM_FireMode_AimingDelayFactorPart : VFM_FireMode_StatPart
    {
        protected override float GetFactor(FireMode mode)
            => FireModeDB.GetWarmup(mode);
    }

    public class VFM_FireMode_RangedCooldownFactorPart : VFM_FireMode_StatPart
    {
        protected override float GetFactor(FireMode mode)
            => FireModeDB.GetCooldown(mode);
    }

    public class VFM_FireMode_ShootingAccuracyPawnPart : VFM_FireMode_StatPart
    {
        protected override float GetFactor(FireMode mode)
            => FireModeDB.GetAccuracy(mode);
    }

}
