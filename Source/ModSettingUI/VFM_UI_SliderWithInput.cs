using UnityEngine;
using Verse;

namespace VFM_VanillaFireModes.ModSettingUI
{
    internal static class VFM_UI_SliderWithInput
    {
        public static void DrawSliderWithInput_Float(
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

        public static void DrawSliderWithInput_Int(
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

    }
}
