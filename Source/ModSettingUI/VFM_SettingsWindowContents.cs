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
        private enum TacticTab
        {
            AutoSelectionTab,
            PrecisionTab,
            BurstTab,
            SuppressionTab,
            DefaultTab
        }

        private static TacticTab currentTab = TacticTab.AutoSelectionTab;

        private static Vector2 scrollPos;
        private static float lastCalculatedHeight = 1000f;

        public static void SettingsWindowContents(Rect inRect, ref VanillaFireModesModSetting settings)
        {
            GUI.BeginGroup(inRect);
            Rect tabtop = new Rect(0, TabDrawer.TabHeight, inRect.width, 0);
            List<TabRecord> tablist = new List<TabRecord>();
            // Auto Selection Tab
            tablist.Add(new TabRecord("VFM_AutoSelection_Label".Translate(), () => { currentTab = TacticTab.AutoSelectionTab; },
                currentTab == TacticTab.AutoSelectionTab));
            // Precision Tab
            tablist.Add(new TabRecord("VFM_PrecisionMode".Translate(), () => { currentTab = TacticTab.PrecisionTab; },
                currentTab == TacticTab.PrecisionTab));
            // Burst Tab
            tablist.Add(new TabRecord("VFM_ShortBurstMode".Translate(), () => { currentTab = TacticTab.BurstTab; },
                currentTab == TacticTab.BurstTab));
            // Suppression Tab
            tablist.Add(new TabRecord("VFM_SuppressionMode".Translate(), () => { currentTab = TacticTab.SuppressionTab; },
                currentTab == TacticTab.SuppressionTab));
            // Default Tab
            tablist.Add(new TabRecord("VFM_DefaultMode".Translate(), () => { currentTab = TacticTab.DefaultTab; },
                currentTab == TacticTab.DefaultTab));
            TabDrawer.DrawTabs(tabtop, tablist);


            float footerHeight = 45f;
            float headerHeight = TabDrawer.TabHeight + 10f;
            Rect scrollOutRect = new Rect(0, headerHeight, inRect.width, inRect.height - headerHeight - footerHeight);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 24f, lastCalculatedHeight);
            Widgets.BeginScrollView(scrollOutRect, ref scrollPos, viewRect);
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(viewRect);

            switch (currentTab)
            {
                case TacticTab.AutoSelectionTab:
                    // 自动切换
                    DrawGeneralGroup(ls,
                        "VFM_AutoSelection_Label".Translate(),
                        ref settings.enableAutoSelectionForPlayer,
                        ref settings.burstMinDistance,
                        ref settings.precisionMinDistance,
                        ref settings.enableFireModeForNPC
                    );
                    break;
                case TacticTab.PrecisionTab:
                    // 精确射击
                    DrawGroup(ls,
                        "VFM_PrecisionMode".Translate(),
                        ref settings.precisionAccuracy, ref settings.precisionWarmup, ref settings.precisionCooldown,
                        ref settings.precisionBurstOption,
                        ref settings.precisionBurstLinearMultiplier,
                        ref settings.precisionBurstAdditiveBonus,
                        ref settings.precisionBurstTentMaxMultiplier,
                        ref settings.precisionBurstTentSlopeK,
                        ref settings.precisionBurstTentPeakOffset,
                        ref settings.precisionBurstAdaptiveBonus,
                        ref settings.precisionBurstAdaptivePeakOffset
                    );
                    break;
                case TacticTab.BurstTab:
                    // 短点射
                    DrawGroup(ls,
                        "VFM_ShortBurstMode".Translate(),
                        ref settings.burstAccuracy, ref settings.burstWarmup, ref settings.burstCooldown,
                        ref settings.burstBurstOption,
                        ref settings.burstBurstLinearMultiplier,
                        ref settings.burstBurstAdditiveBonus,
                        ref settings.burstBurstTentMaxMultiplier,
                        ref settings.burstBurstTentSlopeK,
                        ref settings.burstBurstTentPeakOffset,
                        ref settings.burstBurstAdaptiveBonus,
                        ref settings.burstBurstAdaptivePeakOffset
                    );
                    break;
                case TacticTab.SuppressionTab:
                    // 压制射击
                    DrawGroup(ls,
                        "VFM_SuppressionMode".Translate(),
                        ref settings.suppressionAccuracy, ref settings.suppressionWarmup, ref settings.suppressionCooldown,
                        ref settings.suppressionBurstOption,
                        ref settings.suppressionBurstLinearMultiplier,
                        ref settings.suppressionBurstAdditiveBonus,
                        ref settings.suppressionBurstTentMaxMultiplier,
                        ref settings.suppressionBurstTentSlopeK,
                        ref settings.suppressionBurstTentPeakOffset,
                        ref settings.suppressionBurstAdaptiveBonus,
                        ref settings.suppressionBurstAdaptivePeakOffset
                    );
                    break;
                case TacticTab.DefaultTab:
                    // 默认模式
                    DrawGroup(ls,
                        "VFM_DefaultMode".Translate(),
                        ref settings.defaultAccuracy, ref settings.defaultWarmup, ref settings.defaultCooldown,
                        ref settings.defaultBurstOption,
                        ref settings.defaultBurstLinearMultiplier,
                        ref settings.defaultBurstAdditiveBonus,
                        ref settings.defaultBurstTentMaxMultiplier,
                        ref settings.defaultBurstTentSlopeK,
                        ref settings.defaultBurstTentPeakOffset,
                        ref settings.defaultBurstAdaptiveBonus,
                        ref settings.defaultBurstAdaptivePeakOffset,
                        "VFM_Default_Warning_Label".Translate(),
                        Color.yellow
                    );
                    break;
            }


            lastCalculatedHeight = ls.CurHeight + 20f;
            ls.End();
            Widgets.EndScrollView();


            Rect footerRect = new Rect(0, scrollOutRect.yMax + 10f, inRect.width, footerHeight - 15f);

            GUI.color = new Color(1f, 1f, 1f, 0.3f);
            Widgets.DrawLineHorizontal(footerRect.x, footerRect.y, footerRect.width);
            GUI.color = Color.white;
            // Reset button
            Rect resetRect = new Rect(footerRect.xMax - 240f, footerRect.y + 5f, 240f, 30f);
            if (Widgets.ButtonText(resetRect, "VFM_ResetButton_Label".Translate()))
            {
                settings.ResetSetting();
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            GUI.color = Color.white;
            GUI.EndGroup();
        }
    }
}