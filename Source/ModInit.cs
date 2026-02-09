using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes
{
    public class VanillaFireModes : Mod
    {
        public static VanillaFireModesModSetting settings;

        public VanillaFireModes(ModContentPack contentPack) : base(contentPack)
        {
            settings = GetSettings<VanillaFireModesModSetting>();
            Log.Message("[VanillaFireModes] is loaded!");
            new Harmony("Aliza.VanillaFireModes").PatchAll();
        }

        public override string SettingsCategory()
        {
            return "VFM_ModTitle".Translate();
        }

        private enum TacticTab { PrecisionTab, BurstTab, SuppressionTab }
        private TacticTab currentTab = TacticTab.PrecisionTab;

        private Vector2 scrollPos;
        private float lastCalculatedHeight = 1000f;
        public override void DoSettingsWindowContents(Rect inRect)
        {
            float tabHeight = 30f;
            float tabWidth = inRect.width / 3f;

            // Precision Tab
            GUI.color = (currentTab == TacticTab.PrecisionTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x, inRect.y, tabWidth, tabHeight), "VFM_PrecisionMode".Translate()))
            {
                currentTab = TacticTab.PrecisionTab;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            // Burst Tab
            GUI.color = (currentTab == TacticTab.BurstTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x + tabWidth, inRect.y, tabWidth, tabHeight), "VFM_ShortBurstMode".Translate()))
            {
                currentTab = TacticTab.BurstTab;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            // Suppression Tab
            GUI.color = (currentTab == TacticTab.SuppressionTab) ? Color.yellow : Color.white;
            if (Widgets.ButtonText(new Rect(inRect.x + tabWidth * 2, inRect.y, tabWidth, tabHeight), "VFM_SuppressionMode".Translate()))
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
            Rect resetRect = new Rect(footerRect.xMax - 240f, footerRect.y + 5f, 240f, 30f);;
            if (Widgets.ButtonText(resetRect, "VFM_ResetButton_Label".Translate()))
            {
                settings.ResetSetting();
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }
            GUI.color = Color.white;

        }


        private void DrawGroup(
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

        private void DrawBurstSection(
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

        private void DrawSliderWithInput_Float(
            Listing_Standard ls,
            string label,
            ref float value,
            float min = 0.1f,
            float max = 3f)
        {
            // Rect row = ls.GetRect(30f);

            float labelWidth = 120f;
            float fieldWidth = 60f;
            float gap = 10f;

            float labelHeight = Text.CalcHeight(label, labelWidth);
            float rowHeight = Math.Max(labelHeight, 30f);
            Rect row = ls.GetRect(rowHeight);

            Rect labelRect = new Rect(row.x, row.y, labelWidth, rowHeight);
            Rect sliderRect = new Rect(labelRect.xMax + gap, row.y, row.width - labelWidth - fieldWidth - gap * 2, rowHeight);
            Rect fieldRect = new Rect(sliderRect.xMax + gap, row.y, fieldWidth, 30f);

            Color oldColor = GUI.color;
            GUI.color = Color.white;
            Widgets.Label(labelRect, label);
            GUI.color = oldColor;

            value = Widgets.HorizontalSlider(
                sliderRect,
                value,
                min,
                max,
                true,
                value.ToString("0.00"));

            string buffer = value.ToString("0.00");

            Widgets.TextFieldNumeric(
                fieldRect,
                ref value,
                ref buffer,
                min,
                max);
        }

        private void DrawSliderWithInput_Int(
            Listing_Standard ls,
            string label,
            ref int value,
            int min = 0,
            int max = 20)
        {
            float labelWidth = 120f;
            float fieldWidth = 60f;
            float gap = 10f;

            float labelHeight = Text.CalcHeight(label, labelWidth);
            float rowHeight = Math.Max(labelHeight, 30f);
            Rect row = ls.GetRect(rowHeight);

            Rect labelRect = new Rect(row.x, row.y, labelWidth, rowHeight);
            Rect sliderRect = new Rect(labelRect.xMax + gap, row.y, row.width - labelWidth - fieldWidth - gap * 2, rowHeight);
            Rect fieldRect = new Rect(sliderRect.xMax + gap, row.y, fieldWidth, 30f);

            Color oldColor = GUI.color;
            GUI.color = Color.white;
            Widgets.Label(labelRect, label);
            GUI.color = oldColor;

            float tempFloat = (float)value;
            tempFloat = Widgets.HorizontalSlider(
                sliderRect,
                tempFloat,
                (float)min,
                (float)max,
                true,
                value.ToString("0"));
            value = (int)tempFloat;

            string buffer = value.ToString("0");

            Widgets.TextFieldNumeric(
                fieldRect,
                ref value,
                ref buffer,
                min,
                max);
        }


        private void DrawModFunctionGraph(Rect rect, float extraShot, float peakOffset)
        {
            Widgets.DrawBoxSolid(rect, new Color(0.1f, 0.1f, 0.1f, 0.6f));
            Widgets.DrawBox(rect, 1);

            // 内边距
            Rect chartArea = rect.ContractedBy(18f, 10f);
            // 坐标轴留白
            chartArea.x += 10f;
            chartArea.width -= 15f;
            chartArea.height -= 10f;

            // Y 轴
            Widgets.DrawLine(new Vector2(chartArea.x, chartArea.yMin), new Vector2(chartArea.x, chartArea.yMax), Color.gray, 1f);
            // X 轴
            Widgets.DrawLine(new Vector2(chartArea.x, chartArea.yMax), new Vector2(chartArea.xMax, chartArea.yMax), Color.gray, 1f);

            // 坐标标签
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;

            // Y 轴标签 (100% 效率位置)
            float y100 = chartArea.yMax - (1.0f / 1.1f) * chartArea.height;
            Widgets.Label(new Rect(rect.x + 2f, y100 - 7f, 25f, 15f), "1.0");
            // Y=1.0的水平参考线
            Widgets.DrawLine(new Vector2(chartArea.x, y100), new Vector2(chartArea.xMax, y100), new Color(1, 1, 1, 0.1f), 1f);


            float maxX = 30f;
            float maxY = 1.1f;

            // X 轴标签
            for (int i = 0; i <= 30; i += 5)
            {
                // 实际横坐标从1开始
                int labelValue = (i == 0) ? 1 : i;
                float xPos = chartArea.x + ((labelValue - 1) / (maxX - 1)) * chartArea.width;

                // 绘制刻度小短线
                Widgets.DrawLine(new Vector2(xPos, chartArea.yMax), new Vector2(xPos, chartArea.yMax + 3f), Color.gray, 1f);

                // 绘制标签内容
                Rect labelRect = new Rect(xPos - 10f, chartArea.yMax + 3f, 20f, 15f);
                Text.Anchor = TextAnchor.UpperCenter;
                Widgets.Label(labelRect, labelValue.ToString());
                Text.Anchor = TextAnchor.UpperLeft;
            }


            GUI.color = Color.white;

            // 曲线
            Vector2? lastPoint = null;

            for (float x = 0; x <= chartArea.width; x += 2f)
            {
                // 映射逻辑坐标
                float B = 1f + (x / chartArea.width) * (maxX - 1f);
                float gainFactor = 0f;
                float denom = peakOffset - 1f;

                if (denom > 0)
                {
                    float ratio = (B - 1f) / denom;
                    if (ratio > 0) gainFactor = ratio * (float)Math.Exp(1f - ratio);
                }

                // 映射 UI 坐标
                float py = chartArea.yMax - (gainFactor / maxY) * chartArea.height;
                // 裁剪 Y 轴
                py = Mathf.Clamp(py, chartArea.yMin, chartArea.yMax);

                Vector2 currentPoint = new Vector2(chartArea.x + x, py);

                if (lastPoint.HasValue)
                {
                    Widgets.DrawLine(lastPoint.Value, currentPoint, new Color(0.3f, 1f, 0.3f), 1.5f); // 亮绿色
                }
                lastPoint = currentPoint;
            }

            // 标记当前 Peak 位置 (垂直线)
            float peakXPos = chartArea.x + ((peakOffset - 1) / (maxX - 1)) * chartArea.width;
            if (peakXPos <= chartArea.xMax)
            {
                GUI.color = new Color(0.2f, 0.6f, 1f, 0.4f); // 淡蓝色
                Widgets.DrawLine(new Vector2(peakXPos, chartArea.yMin), new Vector2(peakXPos, chartArea.yMax), GUI.color, 1f);
            }

            Text.Font = GameFont.Small;
        }

        private void DrawTentFunctionGraph(Rect rect, float maxMultiplierSetting, float slopeK, float peakOffset)
        {
            Widgets.DrawBoxSolid(rect, new Color(0.1f, 0.1f, 0.1f, 0.6f));
            Widgets.DrawBox(rect, 1);

            // 内边距
            Rect chartArea = rect.ContractedBy(18f, 10f);
            // 坐标轴留白
            chartArea.x += 10f;
            chartArea.width -= 15f;
            chartArea.height -= 10f;

            // Y 轴
            Widgets.DrawLine(new Vector2(chartArea.x, chartArea.yMin), new Vector2(chartArea.x, chartArea.yMax), Color.gray, 1f);
            // X 轴
            Widgets.DrawLine(new Vector2(chartArea.x, chartArea.yMax), new Vector2(chartArea.xMax, chartArea.yMax), Color.gray, 1f);
            // 绘制坐标标签
            Text.Font = GameFont.Tiny;
            GUI.color = Color.gray;

            float maxX = 30f;
            float maxY = 5.1f;

            // Y 轴标签
            for (int i = 1; i <= 5; i++)
            {
                float yLine = chartArea.yMax - (i / maxY) * chartArea.height;
                Widgets.Label(new Rect(rect.x + 2f, yLine - 7f, 25f, 15f), i.ToString());
                Widgets.DrawLine(new Vector2(chartArea.x, yLine), new Vector2(chartArea.xMax, yLine), new Color(1, 1, 1, 0.1f), 1f);
            }

            float yPointFive = chartArea.yMax - (0.5f / maxY) * chartArea.height;
            Widgets.Label(new Rect(rect.x + 2f, yPointFive - 7f, 25f, 15f), "0.5");
            Widgets.DrawLine(new Vector2(chartArea.x, yPointFive), new Vector2(chartArea.xMax, yPointFive), new Color(1, 1, 1, 0.1f), 1f);

            

            // X 轴标签
            for (int i = 0; i <= 30; i += 5)
            {
                // 实际横坐标从1开始
                int labelValue = (i == 0) ? 1 : i;
                float xPos = chartArea.x + ((labelValue - 1) / (maxX - 1)) * chartArea.width;

                // 绘制刻度小短线
                Widgets.DrawLine(new Vector2(xPos, chartArea.yMax), new Vector2(xPos, chartArea.yMax + 3f), Color.gray, 1f);

                // 绘制标签内容
                Rect labelRect = new Rect(xPos - 10f, chartArea.yMax + 3f, 20f, 15f);
                Text.Anchor = TextAnchor.UpperCenter;
                Widgets.Label(labelRect, labelValue.ToString());
                Text.Anchor = TextAnchor.UpperLeft;
            }

            GUI.color = new Color(0.2f, 0.8f, 0.2f); // 曲线颜色：淡绿色
            Vector2? lastPoint = null;

            // 1 单位步进
            for (float x = 1f; x <= maxX; x += 1f)
            {
                // f(x) = max(0.5, M - k * |x - p|)
                float multiplier = Mathf.Max(0.5f, maxMultiplierSetting - slopeK * Mathf.Abs(x - peakOffset));

                // 映射到 UI 像素坐标
                float screenX = chartArea.x + ((x - 1) / (maxX - 1)) * chartArea.width;
                float screenY = chartArea.yMax - (multiplier / maxY) * chartArea.height;

                Vector2 currentPoint = new Vector2(screenX, screenY);

                if (lastPoint.HasValue)
                {
                    // 绘制线段
                    Widgets.DrawLine(lastPoint.Value, currentPoint, GUI.color, 1.5f);
                }
                lastPoint = currentPoint;
            }

            // 标记当前 Peak 位置 (垂直线)
            float peakXPos = chartArea.x + ((peakOffset - 1) / (maxX - 1)) * chartArea.width;
            if (peakXPos <= chartArea.xMax)
            {
                GUI.color = new Color(0.2f, 0.6f, 1f, 0.4f); // 淡蓝色
                Widgets.DrawLine(new Vector2(peakXPos, chartArea.yMin), new Vector2(peakXPos, chartArea.yMax), GUI.color, 1f);
            }


            GUI.color = Color.white;
        }
    }
}
