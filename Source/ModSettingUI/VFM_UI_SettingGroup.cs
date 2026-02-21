using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_BurstSection;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_SliderWithInput;

namespace VFM_VanillaFireModes.ModSettingUI
{
    internal static class VFM_UI_SettingGroup
    {
        private const float MinDistance = 1f;
        private const float MaxDistance = 100f;
        public static void DrawGeneralGroup(
           Listing_Standard ls,
           string title,
           ref bool enableAutoSelection,
           ref float burstMinDistance,
           ref float precisionMinDistance,
           ref bool enableFireModeForNPC
            )
        {
            float startY = ls.CurHeight;
            Rect groupRect = ls.GetRect(0f);

            Listing_Standard innerLs = new Listing_Standard();
            Rect innerRect = new Rect(groupRect.x + 10f, startY + 10f, ls.ColumnWidth - 20f, 9999f);
            innerLs.Begin(innerRect);

            Text.Font = GameFont.Medium;
            innerLs.Label(title);
            Text.Font = GameFont.Small;
            innerLs.Gap(6f);
            innerLs.CheckboxLabeled(
                "VFM_EnableAutoSelection_Player_Label".Translate(),
                ref enableAutoSelection,
                "VFM_EnableAutoSelection_Player_Desc".Translate()
            );

            innerLs.CheckboxLabeled(
                "VFM_EnableAutoSelection_NPC_Label".Translate(),
                ref enableFireModeForNPC,
                "VFM_EnableAutoSelection_NPC_Desc".Translate()
            );
            innerLs.GapLine();

            innerLs.Label("VFM_AutoSelectionThresholds_Label".Translate());

            // ===== Burst Min =====
            innerLs.Label($"{"VFM_Burst_Min_Distance".Translate()}: {burstMinDistance:F1}");
            float newBurstMin = innerLs.Slider(
                burstMinDistance,
                MinDistance,
                precisionMinDistance - 1f
            );

            burstMinDistance = newBurstMin;
            // ===== Precision Min =====
            innerLs.Label($"{"VFM_Precision_Min_Distance".Translate()}: {precisionMinDistance:F1}");
            float newPreciseMin = innerLs.Slider(
                precisionMinDistance,
                burstMinDistance + 1f,
                MaxDistance
            );
            precisionMinDistance = newPreciseMin;
            innerLs.GapLine();

            DrawPreview(innerLs, burstMinDistance,precisionMinDistance);

            float contentHeight = innerLs.CurHeight;
            innerLs.End();
            float finalHeight = contentHeight + 20f;
            Widgets.DrawBox(new Rect(groupRect.x, startY, ls.ColumnWidth, finalHeight), 1);
            ls.Gap(finalHeight + 15f);
        }

        private static void DrawPreview(Listing_Standard listing, float burstMinDistance, float precisionMinDistance)
        {
            listing.Label("VFM_AutoSelection_Range_Preview".Translate() + ":");

            listing.Label(
                $"{"VFM_SuppressionMode".Translate()}: 0  ~ {burstMinDistance:F1}"
            );

            listing.Label(
                $"{"VFM_ShortBurstMode".Translate()}: {burstMinDistance:F1} ~ {precisionMinDistance:F1}"
            );

            listing.Label(
                $"{"VFM_PrecisionMode".Translate()}: {precisionMinDistance:F1} +"
            );
        }

        public static void DrawGroup(
           Listing_Standard ls,
           string title,
           ref float accuracy,
           ref float warmup,
           ref float cooldown,
           ref BurstShotOption option,
           ref float linearMult,
           ref int addBonus,
           ref float tentMaxMult,
           ref float tentSlopeK,
           ref int tentPeak,
           ref int adaptBonus,
           ref int adaptPeak)
        {
            float startY = ls.CurHeight;
            Rect groupRect = ls.GetRect(0f);

            Listing_Standard innerLs = new Listing_Standard();
            Rect innerRect = new Rect(groupRect.x + 10f, startY + 10f, ls.ColumnWidth - 20f, 9999f);
            innerLs.Begin(innerRect);

            Text.Font = GameFont.Medium;
            innerLs.Label(title);
            Text.Font = GameFont.Small;
            innerLs.Gap(6f);

            // 精度
            DrawSliderWithInput_Float(innerLs, "VFM_Accuracy_Label".Translate(), ref accuracy);
            // 瞄准时间
            DrawSliderWithInput_Float(innerLs, "VFM_Warmup_Label".Translate(), ref warmup);
            // 冷却时间
            DrawSliderWithInput_Float(innerLs, "VFM_Cooldown_Label".Translate(), ref cooldown);

            innerLs.Gap(10f);

            // 连射次数
            DrawBurstSection(innerLs, ref option, ref linearMult, ref addBonus, ref tentMaxMult, ref tentSlopeK, ref tentPeak, ref adaptBonus, ref adaptPeak);

            float contentHeight = innerLs.CurHeight;
            innerLs.End();

            float finalHeight = contentHeight + 20f;
            Widgets.DrawBox(new Rect(groupRect.x, startY, ls.ColumnWidth, finalHeight), 1);
            ls.Gap(finalHeight + 15f);
        }


    }
}
