using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_SliderWithInput;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_BurstSection;

namespace VFM_VanillaFireModes.ModSettingUI
{
    internal static class VFM_UI_SettingGroup
    {
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
