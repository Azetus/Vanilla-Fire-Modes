using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_SliderWithInput;
using static VFM_VanillaFireModes.ModSettingUI.VFM_UI_Graph;

namespace VFM_VanillaFireModes.ModSettingUI
{
    internal static class VFM_UI_BurstSection
    {
        public static void DrawBurstSection(
           Listing_Standard ls,
           ref BurstShotOption option,
           ref float linearMult,
           ref int addBonus,
           ref float tentMaxMult,
           ref float tentSlopeK,
           ref int tentPeak,
           ref int adaptBonus,
           ref int adaptPeak)
        {
            ls.Label("VFM_Burst_Calculation_Mode_Label".Translate());

            ls.GapLine(6f);

            if (ls.RadioButton("VFM_Burst_Linear_Label".Translate(), option == BurstShotOption.Linear)) option = BurstShotOption.Linear;
            if (ls.RadioButton("VFM_Burst_Additive_Label".Translate(), option == BurstShotOption.Additive)) option = BurstShotOption.Additive;
            if (ls.RadioButton("VFM_Burst_Tent_Label".Translate(), option == BurstShotOption.Tent)) option = BurstShotOption.Tent;
            if (ls.RadioButton("VFM_Burst_Adaptive_Label".Translate(), option == BurstShotOption.Adaptive)) option = BurstShotOption.Adaptive;

            ls.Gap(6f);

            switch (option)
            {
                case BurstShotOption.Linear:
                    DrawSliderWithInput_Float(ls, "VFM_Burst_Linear_input_Label".Translate(), ref linearMult, 0.1f, 5f);
                    break;

                case BurstShotOption.Additive:
                    DrawSliderWithInput_Int(ls, "VFM_Burst_Additive_input_Label".Translate(), ref addBonus, 0, 50);
                    break;

                case BurstShotOption.Tent:
                    DrawSliderWithInput_Float(ls, "VFM_Burst_Tent_MaxMult_input_Label".Translate(), ref tentMaxMult, 1, 5);
                    DrawSliderWithInput_Float(ls, "VFM_Burst_Tent_SlopeK_input_Label".Translate(), ref tentSlopeK, 0f, 5f);
                    DrawSliderWithInput_Int(ls, "VFM_Burst_Tent_PeakOffset_input_Label".Translate(), ref tentPeak, 2, 30);

                    ls.Gap(4f);
                    Rect tentGraphRect = ls.GetRect(150f).ContractedBy(2f);
                    DrawTentFunctionGraph(tentGraphRect, tentMaxMult, tentSlopeK, tentPeak);

                    Text.Font = GameFont.Tiny;
                    GUI.color = Color.gray;
                    ls.Label("VFM_Burst_Tent_Graph_Label".Translate());
                    GUI.color = Color.white;
                    Text.Font = GameFont.Small;

                    break;
                case BurstShotOption.Adaptive:
                    DrawSliderWithInput_Int(ls, "VFM_Burst_Adaptive_ExtraBonus_input_Label".Translate(), ref adaptBonus, 0, 50);
                    DrawSliderWithInput_Int(ls, "VFM_Burst_Adaptive_PeakOffset_input_Label".Translate(), ref adaptPeak, 2, 30);

                    // 绘制预览图
                    ls.Gap(4f);
                    Rect adaptGraphRect = ls.GetRect(150f).ContractedBy(2f);
                    DrawModFunctionGraph(adaptGraphRect, adaptBonus, adaptPeak);

                    Text.Font = GameFont.Tiny;
                    GUI.color = Color.gray;
                    ls.Label("VFM_Burst_Adaptive_Graph_Label".Translate());
                    GUI.color = Color.white;
                    Text.Font = GameFont.Small;
                    break;
            }
            ls.GapLine(6f);
        }



    }
}
