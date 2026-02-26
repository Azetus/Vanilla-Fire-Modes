using Verse;

namespace VFM_VanillaFireModes.Settings
{
    public class VanillaFireModesModSetting : ModSettings
    {
        // ------ 默认 (Default) ------
        public float defaultAccuracy = 1f;
        public float defaultWarmup = 1f;
        public float defaultCooldown = 1f;

        public BurstShotOption defaultBurstOption = BurstShotOption.Linear;

        public float defaultBurstLinearMultiplier = 1f;

        public int defaultBurstAdditiveBonus = 1;

        public float defaultBurstTentMaxMultiplier = 1f;
        public float defaultBurstTentSlopeK = 0.10f;
        public int defaultBurstTentPeakOffset = 3;

        public int defaultBurstAdaptiveBonus = 1;
        public int defaultBurstAdaptivePeakOffset = 2;
        
        
        // ------ 精确射击 (Precision) ------
        public float precisionAccuracy = 1.5f;
        public float precisionWarmup = 1.2f;
        public float precisionCooldown = 0.5f;

        public BurstShotOption precisionBurstOption = BurstShotOption.Linear;

        public float precisionBurstLinearMultiplier = 0.8f;

        public int precisionBurstAdditiveBonus = 1;

        public float precisionBurstTentMaxMultiplier = 1f;
        public float precisionBurstTentSlopeK = 0.05f;
        public int precisionBurstTentPeakOffset = 3;

        public int precisionBurstAdaptiveBonus = 1;
        public int precisionBurstAdaptivePeakOffset = 2;


        // ------ 短点射 (Burst) ------
        public float burstAccuracy = 0.8f;
        public float burstWarmup = 0.8f;
        public float burstCooldown = 0.8f;

        public BurstShotOption burstBurstOption = BurstShotOption.Tent;

        public float burstBurstLinearMultiplier = 1f;

        public int burstBurstAdditiveBonus = 3;

        public float burstBurstTentMaxMultiplier = 1.75f;
        public float burstBurstTentSlopeK = 0.10f;
        public int burstBurstTentPeakOffset = 4;

        public int burstBurstAdaptiveBonus = 5;
        public int burstBurstAdaptivePeakOffset = 4;


        // ------ 压制射击 (Suppression) ------
        public float suppressionAccuracy = 0.5f;
        public float suppressionWarmup = 0.5f;
        public float suppressionCooldown = 1.2f;

        public BurstShotOption suppressionBurstOption = BurstShotOption.Adaptive;

        public float suppressionBurstLinearMultiplier = 1f;

        public int suppressionBurstAdditiveBonus = 10;


        public float suppressionBurstTentMaxMultiplier = 2f;
        public float suppressionBurstTentSlopeK = 0.05f;
        public int suppressionBurstTentPeakOffset = 5;

        public int suppressionBurstAdaptiveBonus = 10;
        public int suppressionBurstAdaptivePeakOffset = 5;

        // ------ 自动选择模式 (Auto Selection) ------
        public bool enableAutoSelectionForPlayer = true;
        public float burstMinDistance = 12f;
        public float precisionMinDistance = 25f;


        // ------ NPC单位是否启用开火模式 ------
        public bool enableFireModeForNPC = true;

        public override void ExposeData()
        {
            // ------ 默认 (Default) ------
            Scribe_Values.Look(ref defaultAccuracy, "defaultAccuracy", 1f);
            Scribe_Values.Look(ref defaultWarmup, "defaultWarmup", 1f);
            Scribe_Values.Look(ref defaultCooldown, "defaultCooldown", 1f);
            Scribe_Values.Look(ref defaultBurstOption, "defaultBurstOption", BurstShotOption.Linear);
            Scribe_Values.Look(ref defaultBurstLinearMultiplier, "defaultBurstLinearMultiplier", 1f);
            Scribe_Values.Look(ref defaultBurstAdditiveBonus, "defaultBurstAdditiveBonus", 1);
            Scribe_Values.Look(ref defaultBurstTentMaxMultiplier, "defaultBurstTentMaxMultiplier", 1f);
            Scribe_Values.Look(ref defaultBurstTentSlopeK, "defaultBurstTentSlopeK", 0.10f);
            Scribe_Values.Look(ref defaultBurstTentPeakOffset, "defaultBurstTentPeakOffset", 3);
            Scribe_Values.Look(ref defaultBurstAdaptiveBonus, "defaultBurstAdaptiveBonus", 1);
            Scribe_Values.Look(ref defaultBurstAdaptivePeakOffset, "defaultBurstAdaptivePeakOffset", 2);
            
            // --- 精确射击 (Precision) ---
            Scribe_Values.Look(ref precisionAccuracy, "precisionAccuracy", 1.5f);
            Scribe_Values.Look(ref precisionWarmup, "precisionWarmup", 1.2f);
            Scribe_Values.Look(ref precisionCooldown, "precisionCooldown", 0.5f);
            Scribe_Values.Look(ref precisionBurstOption, "precisionBurstOption", BurstShotOption.Linear);
            Scribe_Values.Look(ref precisionBurstLinearMultiplier, "precisionBurstLinearMultiplier", 0.8f);
            Scribe_Values.Look(ref precisionBurstAdditiveBonus, "precisionBurstAdditiveBonus", 1);
            Scribe_Values.Look(ref precisionBurstTentMaxMultiplier, "precisionBurstTentMaxMultiplier", 1f);
            Scribe_Values.Look(ref precisionBurstTentSlopeK, "precisionBurstTentSlopeK", 0.05f);
            Scribe_Values.Look(ref precisionBurstTentPeakOffset, "precisionBurstTentPeakOffset", 3);
            Scribe_Values.Look(ref precisionBurstAdaptiveBonus, "precisionBurstAdaptiveBonus", 1);
            Scribe_Values.Look(ref precisionBurstAdaptivePeakOffset, "precisionBurstAdaptivePeakOffset", 2);

            // --- 短点射 (Burst) ---
            Scribe_Values.Look(ref burstAccuracy, "burstAccuracy", 0.8f);
            Scribe_Values.Look(ref burstWarmup, "burstWarmup", 0.8f);
            Scribe_Values.Look(ref burstCooldown, "burstCooldown", 0.8f);
            Scribe_Values.Look(ref burstBurstOption, "burstBurstOption", BurstShotOption.Tent);
            Scribe_Values.Look(ref burstBurstLinearMultiplier, "burstBurstLinearMultiplier", 1f);
            Scribe_Values.Look(ref burstBurstAdditiveBonus, "burstBurstAdditiveBonus", 3);
            Scribe_Values.Look(ref burstBurstTentMaxMultiplier, "burstBurstTentMaxMultiplier", 1.75f);
            Scribe_Values.Look(ref burstBurstTentSlopeK, "burstBurstTentSlopeK", 0.10f);
            Scribe_Values.Look(ref burstBurstTentPeakOffset, "burstBurstTentPeakOffset", 4);
            Scribe_Values.Look(ref burstBurstAdaptiveBonus, "burstBurstAdaptiveBonus", 5);
            Scribe_Values.Look(ref burstBurstAdaptivePeakOffset, "burstBurstAdaptivePeakOffset", 4);

            // --- 压制射击 (Suppression) ---
            Scribe_Values.Look(ref suppressionAccuracy, "suppressionAccuracy", 0.5f);
            Scribe_Values.Look(ref suppressionWarmup, "suppressionWarmup", 0.5f);
            Scribe_Values.Look(ref suppressionCooldown, "suppressionCooldown", 1.2f);
            Scribe_Values.Look(ref suppressionBurstOption, "suppressionBurstOption", BurstShotOption.Adaptive);
            Scribe_Values.Look(ref suppressionBurstLinearMultiplier, "suppressionBurstLinearMultiplier", 1f);
            Scribe_Values.Look(ref suppressionBurstAdditiveBonus, "suppressionBurstAdditiveBonus", 10);
            Scribe_Values.Look(ref suppressionBurstTentMaxMultiplier, "suppressionBurstTentMaxMultiplier", 2f);
            Scribe_Values.Look(ref suppressionBurstTentSlopeK, "suppressionBurstTentSlopeK", 0.05f);
            Scribe_Values.Look(ref suppressionBurstTentPeakOffset, "suppressionBurstTentPeakOffset", 5);
            Scribe_Values.Look(ref suppressionBurstAdaptiveBonus, "suppressionBurstAdaptiveBonus", 10);
            Scribe_Values.Look(ref suppressionBurstAdaptivePeakOffset, "suppressionBurstAdaptivePeakOffset", 5);

            // ------ 自动选择模式 (Auto Selection) ------
            Scribe_Values.Look(ref enableAutoSelectionForPlayer, "enableAutoSelectionForPlayer", true);
            Scribe_Values.Look(ref burstMinDistance, "burstMinDistance", 12f);
            Scribe_Values.Look(ref precisionMinDistance, "precisionMinDistance", 25f);

            // ------ NPC单位是否启用开火模式 ------
            Scribe_Values.Look(ref enableFireModeForNPC, "enableFireModeForNPC", true);
            base.ExposeData();
        }

        public void ResetSetting()
        {
            // ------ 默认 (Default) ------
            defaultAccuracy = 1f;
            defaultWarmup = 1f;
            defaultCooldown = 1f;

            defaultBurstOption = BurstShotOption.Linear;

            defaultBurstLinearMultiplier = 1f;
            defaultBurstAdditiveBonus = 1;

            defaultBurstTentMaxMultiplier = 1f;
            defaultBurstTentSlopeK = 0.10f;
            defaultBurstTentPeakOffset = 3;

            defaultBurstAdaptiveBonus = 1;
            defaultBurstAdaptivePeakOffset = 2;
            
            // ------ 精确射击 (Precision) ------
            precisionAccuracy = 1.5f;
            precisionWarmup = 1.2f;
            precisionCooldown = 0.5f;

            precisionBurstOption = BurstShotOption.Linear;

            precisionBurstLinearMultiplier = 0.8f;

            precisionBurstAdditiveBonus = 1;

            precisionBurstTentMaxMultiplier = 1f;
            precisionBurstTentSlopeK = 0.05f;
            precisionBurstTentPeakOffset = 3;

            precisionBurstAdaptiveBonus = 1;
            precisionBurstAdaptivePeakOffset = 2;


            // ------ 短点射 (Burst) ------
            burstAccuracy = 0.8f;
            burstWarmup = 0.8f;
            burstCooldown = 0.8f;

            burstBurstOption = BurstShotOption.Tent;

            burstBurstLinearMultiplier = 1f;

            burstBurstAdditiveBonus = 3;

            burstBurstTentMaxMultiplier = 1.75f;
            burstBurstTentSlopeK = 0.10f;
            burstBurstTentPeakOffset = 4;

            burstBurstAdaptiveBonus = 5;
            burstBurstAdaptivePeakOffset = 4;


            // ------ 压制射击 (Suppression) ------
            suppressionAccuracy = 0.5f;
            suppressionWarmup = 0.5f;
            suppressionCooldown = 1.2f;

            suppressionBurstOption = BurstShotOption.Adaptive;

            suppressionBurstLinearMultiplier = 1f;

            suppressionBurstAdditiveBonus = 10;


            suppressionBurstTentMaxMultiplier = 2f;
            suppressionBurstTentSlopeK = 0.05f;
            suppressionBurstTentPeakOffset = 5;

            suppressionBurstAdaptiveBonus = 10;
            suppressionBurstAdaptivePeakOffset = 5;


            // ------ 自动选择模式 (Auto Selection) ------
            enableAutoSelectionForPlayer = true;
            burstMinDistance = 12f;
            precisionMinDistance = 25f;


            // ------ NPC单位是否启用开火模式 ------
            enableFireModeForNPC = true;
        }
    }
}
