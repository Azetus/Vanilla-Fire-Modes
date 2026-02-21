using UnityEngine;
using Verse;
using VFM_VanillaFireModes.ModSettingUI;

namespace VFM_VanillaFireModes.ModSettingUI
{
    internal static class VFM_UI_Graph
    {
        public static void DrawModFunctionGraph(Rect rect, float extraShot, float peakOffset)
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
        public static void DrawTentFunctionGraph(Rect rect, float maxMultiplierSetting, float slopeK, float peakOffset)
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
