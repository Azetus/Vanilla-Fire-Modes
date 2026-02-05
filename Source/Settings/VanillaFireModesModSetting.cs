using Verse;

namespace VFM_VanillaFireModes.Settings
{
    public class VanillaFireModesModSetting : ModSettings
    {
        // 每个 fire mode 的四个属性
        public float precisionWarmup = 1f;
        public float precisionCooldown = 1f;
        public float precisionBurst = 1f;
        public float precisionAccuracy = 1f;

        public float burstWarmup = 1f;
        public float burstCooldown = 1f;
        public float burstBurst = 1f;
        public float burstAccuracy = 1f;

        public float suppressionWarmup = 1f;
        public float suppressionCooldown = 1f;
        public float suppressionBurst = 1f;
        public float suppressionAccuracy = 1f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref precisionWarmup, "precisionWarmup", 1f);
            Scribe_Values.Look(ref precisionCooldown, "precisionCooldown", 1f);
            Scribe_Values.Look(ref precisionBurst, "precisionBurst", 1f);
            Scribe_Values.Look(ref precisionAccuracy, "precisionAccuracy", 1f);

            Scribe_Values.Look(ref burstWarmup, "burstWarmup", 1f);
            Scribe_Values.Look(ref burstCooldown, "burstCooldown", 1f);
            Scribe_Values.Look(ref burstBurst, "burstBurst", 1f);
            Scribe_Values.Look(ref burstAccuracy, "burstAccuracy", 1f);

            Scribe_Values.Look(ref suppressionWarmup, "suppressionWarmup", 1f);
            Scribe_Values.Look(ref suppressionCooldown, "suppressionCooldown", 1f);
            Scribe_Values.Look(ref suppressionBurst, "suppressionBurst", 1f);
            Scribe_Values.Look(ref suppressionAccuracy, "suppressionAccuracy", 1f);

            base.ExposeData();
        }
    }
}
