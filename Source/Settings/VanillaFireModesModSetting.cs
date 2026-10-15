using Verse;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.Settings
{
    public class VanillaFireModesModSetting : ModSettings
    {
        # region FireModes

        // ------ 默认 (Default) ------
        public float defaultAccuracy = 1f;
        public float defaultWarmup = 1f;
        public float defaultCooldown = 1f;

        public BurstShotOption defaultBurstOption = BurstShotOption.Linear;

        public float defaultBurstLinearMultiplier = 1f;

        public int defaultBurstAdditiveBonus = 1;

        public float defaultBurstTentMaxMultiplier = 1f;
        public float defaultBurstTentSlopeK = 0f;
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

        #endregion

        // ------ 非征召状态显示按钮 ------
        public bool alwaysDisplayGizmo = false;


        // ------ 自动选择模式 (Auto Selection) ------
        public bool enableAutoSelectionForPlayer = true;
        public float burstMinDistance = 12f;
        public float precisionMinDistance = 25f;
        public bool autoModeDefaultOn = false;

        // ------ NPC单位是否启用开火模式 ------
        public bool enableFireModeForNPC = true;

        // ------ CustomWeaponProfile ------
        public Dictionary<string, VFM_WeaponProfile> CustomWeaponProfiles = new Dictionary<string, VFM_WeaponProfile>();

        public override void ExposeData()
        {
            base.ExposeData();

            # region FireModes

            // ------ 默认 (Default) ------
            Scribe_Values.Look(ref defaultAccuracy, nameof(defaultAccuracy), 1f);
            Scribe_Values.Look(ref defaultWarmup, nameof(defaultWarmup), 1f);
            Scribe_Values.Look(ref defaultCooldown, nameof(defaultCooldown), 1f);
            Scribe_Values.Look(ref defaultBurstOption, nameof(defaultBurstOption), BurstShotOption.Linear);
            Scribe_Values.Look(ref defaultBurstLinearMultiplier, nameof(defaultBurstLinearMultiplier), 1f);
            Scribe_Values.Look(ref defaultBurstAdditiveBonus, nameof(defaultBurstAdditiveBonus), 1);
            Scribe_Values.Look(ref defaultBurstTentMaxMultiplier, nameof(defaultBurstTentMaxMultiplier), 1f);
            Scribe_Values.Look(ref defaultBurstTentSlopeK, nameof(defaultBurstTentSlopeK), 0f);
            Scribe_Values.Look(ref defaultBurstTentPeakOffset, nameof(defaultBurstTentPeakOffset), 3);
            Scribe_Values.Look(ref defaultBurstAdaptiveBonus, nameof(defaultBurstAdaptiveBonus), 1);
            Scribe_Values.Look(ref defaultBurstAdaptivePeakOffset, nameof(defaultBurstAdaptivePeakOffset), 2);

            // --- 精确射击 (Precision) ---
            Scribe_Values.Look(ref precisionAccuracy, nameof(precisionAccuracy), 1.5f);
            Scribe_Values.Look(ref precisionWarmup, nameof(precisionWarmup), 1.2f);
            Scribe_Values.Look(ref precisionCooldown, nameof(precisionCooldown), 0.5f);
            Scribe_Values.Look(ref precisionBurstOption, nameof(precisionBurstOption), BurstShotOption.Linear);
            Scribe_Values.Look(ref precisionBurstLinearMultiplier, nameof(precisionBurstLinearMultiplier), 0.8f);
            Scribe_Values.Look(ref precisionBurstAdditiveBonus, nameof(precisionBurstAdditiveBonus), 1);
            Scribe_Values.Look(ref precisionBurstTentMaxMultiplier, nameof(precisionBurstTentMaxMultiplier), 1f);
            Scribe_Values.Look(ref precisionBurstTentSlopeK, nameof(precisionBurstTentSlopeK), 0.05f);
            Scribe_Values.Look(ref precisionBurstTentPeakOffset, nameof(precisionBurstTentPeakOffset), 3);
            Scribe_Values.Look(ref precisionBurstAdaptiveBonus, nameof(precisionBurstAdaptiveBonus), 1);
            Scribe_Values.Look(ref precisionBurstAdaptivePeakOffset, nameof(precisionBurstAdaptivePeakOffset), 2);

            // --- 短点射 (Burst) ---
            Scribe_Values.Look(ref burstAccuracy, nameof(burstAccuracy), 0.8f);
            Scribe_Values.Look(ref burstWarmup, nameof(burstWarmup), 0.8f);
            Scribe_Values.Look(ref burstCooldown, nameof(burstCooldown), 0.8f);
            Scribe_Values.Look(ref burstBurstOption, nameof(burstBurstOption), BurstShotOption.Tent);
            Scribe_Values.Look(ref burstBurstLinearMultiplier, nameof(burstBurstLinearMultiplier), 1f);
            Scribe_Values.Look(ref burstBurstAdditiveBonus, nameof(burstBurstAdditiveBonus), 3);
            Scribe_Values.Look(ref burstBurstTentMaxMultiplier, nameof(burstBurstTentMaxMultiplier), 1.75f);
            Scribe_Values.Look(ref burstBurstTentSlopeK, nameof(burstBurstTentSlopeK), 0.10f);
            Scribe_Values.Look(ref burstBurstTentPeakOffset, nameof(burstBurstTentPeakOffset), 4);
            Scribe_Values.Look(ref burstBurstAdaptiveBonus, nameof(burstBurstAdaptiveBonus), 5);
            Scribe_Values.Look(ref burstBurstAdaptivePeakOffset, nameof(burstBurstAdaptivePeakOffset), 4);

            // --- 压制射击 (Suppression) ---
            Scribe_Values.Look(ref suppressionAccuracy, nameof(suppressionAccuracy), 0.5f);
            Scribe_Values.Look(ref suppressionWarmup, nameof(suppressionWarmup), 0.5f);
            Scribe_Values.Look(ref suppressionCooldown, nameof(suppressionCooldown), 1.2f);
            Scribe_Values.Look(ref suppressionBurstOption, nameof(suppressionBurstOption), BurstShotOption.Adaptive);
            Scribe_Values.Look(ref suppressionBurstLinearMultiplier, nameof(suppressionBurstLinearMultiplier), 1f);
            Scribe_Values.Look(ref suppressionBurstAdditiveBonus, nameof(suppressionBurstAdditiveBonus), 10);
            Scribe_Values.Look(ref suppressionBurstTentMaxMultiplier, nameof(suppressionBurstTentMaxMultiplier), 2f);
            Scribe_Values.Look(ref suppressionBurstTentSlopeK, nameof(suppressionBurstTentSlopeK), 0.05f);
            Scribe_Values.Look(ref suppressionBurstTentPeakOffset, nameof(suppressionBurstTentPeakOffset), 5);
            Scribe_Values.Look(ref suppressionBurstAdaptiveBonus, nameof(suppressionBurstAdaptiveBonus), 10);
            Scribe_Values.Look(ref suppressionBurstAdaptivePeakOffset, nameof(suppressionBurstAdaptivePeakOffset), 5);

            #endregion

            // ------ 非征召状态显示按钮 ------
            Scribe_Values.Look(ref alwaysDisplayGizmo, nameof(alwaysDisplayGizmo), false);

            // ------ 自动选择模式 (Auto Selection) ------
            Scribe_Values.Look(ref enableAutoSelectionForPlayer, nameof(enableAutoSelectionForPlayer), true);
            Scribe_Values.Look(ref burstMinDistance, nameof(burstMinDistance), 12f);
            Scribe_Values.Look(ref precisionMinDistance, nameof(precisionMinDistance), 25f);
            Scribe_Values.Look(ref autoModeDefaultOn, nameof(autoModeDefaultOn), false);
            
            // ------ NPC单位是否启用开火模式 ------
            Scribe_Values.Look(ref enableFireModeForNPC, nameof(enableFireModeForNPC), true);

            // ------ CustomWeaponProfile ------
            Scribe_Collections.Look(ref CustomWeaponProfiles, nameof(CustomWeaponProfiles), LookMode.Value);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (CustomWeaponProfiles == null) CustomWeaponProfiles = new Dictionary<string, VFM_WeaponProfile>();
            }
        }

        public void ResetSetting()
        {
            #region FireModes

            // ------ 默认 (Default) ------
            defaultAccuracy = 1f;
            defaultWarmup = 1f;
            defaultCooldown = 1f;

            defaultBurstOption = BurstShotOption.Linear;

            defaultBurstLinearMultiplier = 1f;
            defaultBurstAdditiveBonus = 1;

            defaultBurstTentMaxMultiplier = 1f;
            defaultBurstTentSlopeK = 0f;
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

            #endregion

            // ------ 非征召状态显示按钮 ------
            alwaysDisplayGizmo = false;

            // ------ 自动选择模式 (Auto Selection) ------
            enableAutoSelectionForPlayer = true;
            burstMinDistance = 12f;
            precisionMinDistance = 25f;
            autoModeDefaultOn = false;

            // ------ NPC单位是否启用开火模式 ------
            enableFireModeForNPC = true;
        }

        public void ResetWeaponProfileSetting()
        {
            if (CustomWeaponProfiles != null) 
                CustomWeaponProfiles.Clear();
            if (CustomWeaponProfiles == null) 
                CustomWeaponProfiles = new Dictionary<string, VFM_WeaponProfile>();
        }
    }
}