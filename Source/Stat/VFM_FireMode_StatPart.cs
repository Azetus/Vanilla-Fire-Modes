using RimWorld;
using Verse;
using VFM_VanillaFireModes.Comps;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Stat
{
    public class VFM_FireMode_ShootingAccuracyPawnPart : VFM_FireMode_StatPart { }
    public class VFM_FireMode_RangedCooldownFactorPart : VFM_FireMode_StatPart { }
    public class VFM_FireMode_AimingDelayFactorPart : VFM_FireMode_StatPart { }
    public abstract class VFM_FireMode_StatPart : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing) return;

            if (req.Thing is not Pawn pawn)
            {
                return;
            }
            if (pawn == null) return;

            var comp = pawn.TryGetComp<VFM_PawnCompFireMode>();
            if (comp == null) return;


            if (this is VFM_FireMode_AimingDelayFactorPart)
            {
                val *= FireModeDB.GetWarmup(comp.curMode);
            }

            else if (this is VFM_FireMode_RangedCooldownFactorPart)
            {
                val *= FireModeDB.GetCooldown(comp.curMode);
            }

            else if (this is VFM_FireMode_ShootingAccuracyPawnPart)
            {
                val *= FireModeDB.GetAccuracy(comp.curMode);
            }


        }

        public override string? ExplanationPart(StatRequest req)
        {
            if (!req.HasThing) return null;

            if (req.Thing is not Pawn pawn)
            {
                return null;
            }
            if (pawn == null) return null;

            var comp = pawn.TryGetComp<VFM_PawnCompFireMode>();
            if (comp == null) return null;

            if (this is VFM_FireMode_AimingDelayFactorPart)
                return "VFM_StatPart_Label".Translate(Utils.GetFireModeLabelFor(comp.curMode), Utils.ToPercentString(FireModeDB.GetWarmup(comp.curMode)));

            else if (this is VFM_FireMode_RangedCooldownFactorPart)
                return "VFM_StatPart_Label".Translate(Utils.GetFireModeLabelFor(comp.curMode), Utils.ToPercentString(FireModeDB.GetCooldown(comp.curMode)));

            else if (this is VFM_FireMode_ShootingAccuracyPawnPart)
                return "VFM_StatPart_Label".Translate(Utils.GetFireModeLabelFor(comp.curMode), Utils.ToPercentString(FireModeDB.GetAccuracy(comp.curMode)));

            return null;
        }

    }

}
