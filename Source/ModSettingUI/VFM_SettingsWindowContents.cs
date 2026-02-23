using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VFM_VanillaFireModes.Settings;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_SettingGroup;

namespace VFM_VanillaFireModes.ModSettingUI
{
    internal static class VFM_SettingsWindowContents
    {
        private enum TacticTab { GeneralTab, PrecisionTab, BurstTab, SuppressionTab }
        private static TacticTab currentTab = TacticTab.GeneralTab;

        private static Vector2 scrollPos;
        private static float lastCalculatedHeight = 1000f;
        public static void SettingsWindowContents(Rect inRect, ref VanillaFireModesModSetting settings)
        {

            float tabHeight = 30f;
            float tabWidth = inRect.width / 4f;

            // Auto Selection Tab
            GUI.color = (currentTab == TacticTab.GeneralTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x, inRect.y, tabWidth, tabHeight), "VFM_GeneralTab_Label".Translate()))
            {
                currentTab = TacticTab.GeneralTab;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            // Precision Tab
            GUI.color = (currentTab == TacticTab.PrecisionTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x + tabWidth, inRect.y, tabWidth, tabHeight), "VFM_PrecisionMode".Translate()))
            {
                currentTab = TacticTab.PrecisionTab;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            // Burst Tab
            GUI.color = (currentTab == TacticTab.BurstTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x + tabWidth * 2, inRect.y, tabWidth, tabHeight), "VFM_ShortBurstMode".Translate()))
            {
                currentTab = TacticTab.BurstTab;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            // Suppression Tab
            GUI.color = (currentTab == TacticTab.SuppressionTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x + tabWidth * 3, inRect.y, tabWidth, tabHeight), "VFM_SuppressionMode".Translate()))
            {
                currentTab = TacticTab.SuppressionTab;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }
            GUI.color = Color.white;

            float footerHeight = 45f;
            Rect scrollOutRect = new Rect(inRect.x, inRect.y + tabHeight + 10f, inRect.width, inRect.height - tabHeight - footerHeight - 15f);
            //Rect viewRect = new Rect(0f, 0f, inRect.width - 24f, lastCalculatedHeight);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 24f, lastCalculatedHeight);
            Widgets.BeginScrollView(scrollOutRect, ref scrollPos, viewRect);
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(viewRect);

            switch (currentTab)
            {
                case TacticTab.GeneralTab:
                    // 通用设置
                    DrawGeneralGroup(ls, ref settings);
                    break;
                case TacticTab.PrecisionTab:
                    // 精确射击
                    DrawGroup(ls,
                        "VFM_PrecisionMode".Translate(),
                        ref settings.precisionAccuracy, ref settings.precisionWarmup, ref settings.precisionCooldown,
                        ref settings.precisionBurstOption,
                        ref settings.precisionBurstLinearMultiplier,
                        ref settings.precisionBurstAdditiveBonus,
                        ref settings.precisionBurstTentMaxMultiplier, ref settings.precisionBurstTentSlopeK, ref settings.precisionBurstTentPeakOffset,
                        ref settings.precisionBurstAdaptiveBonus, ref settings.precisionBurstAdaptivePeakOffset);
                    break;
                case TacticTab.BurstTab:
                    // 短点射
                    DrawGroup(ls,
                        "VFM_ShortBurstMode".Translate(),
                        ref settings.burstAccuracy, ref settings.burstWarmup, ref settings.burstCooldown,
                        ref settings.burstBurstOption,
                        ref settings.burstBurstLinearMultiplier,
                        ref settings.burstBurstAdditiveBonus,
                        ref settings.burstBurstTentMaxMultiplier, ref settings.burstBurstTentSlopeK, ref settings.burstBurstTentPeakOffset,
                        ref settings.burstBurstAdaptiveBonus, ref settings.burstBurstAdaptivePeakOffset);
                    break;
                case TacticTab.SuppressionTab:
                    // 压制射击
                    DrawGroup(ls,
                        "VFM_SuppressionMode".Translate(),
                        ref settings.suppressionAccuracy, ref settings.suppressionWarmup, ref settings.suppressionCooldown,
                        ref settings.suppressionBurstOption,
                        ref settings.suppressionBurstLinearMultiplier,
                        ref settings.suppressionBurstAdditiveBonus,
                        ref settings.suppressionBurstTentMaxMultiplier, ref settings.suppressionBurstTentSlopeK, ref settings.suppressionBurstTentPeakOffset,
                        ref settings.suppressionBurstAdaptiveBonus, ref settings.suppressionBurstAdaptivePeakOffset);
                    break;
            }



            lastCalculatedHeight = ls.CurHeight + 20f;
            ls.End();
            Widgets.EndScrollView();


            Rect footerRect = new Rect(inRect.x, inRect.yMax - footerHeight + 5f, inRect.width, footerHeight);

            GUI.color = new Color(1f, 1f, 1f, 0.3f);
            Widgets.DrawLineHorizontal(footerRect.x, footerRect.y, footerRect.width);
            GUI.color = Color.white;
            // Reset button
            Rect resetRect = new Rect(footerRect.xMax - 240f, footerRect.y + 5f, 240f, 30f); ;
            if (Widgets.ButtonText(resetRect, "VFM_ResetButton_Label".Translate()))
            {
                settings.ResetSetting();
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }
            GUI.color = Color.white;

        }
    }
}
