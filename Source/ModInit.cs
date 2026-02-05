using HarmonyLib;
using UnityEngine;
using Verse;
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
            return "Vanilla Fire Modes";
        }
        private Vector2 scrollPos;
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect viewRect = new Rect(0, 0, inRect.width - 20, 600);

            Widgets.BeginScrollView(inRect, ref scrollPos, viewRect);

            Listing_Standard ls = new Listing_Standard();
            ls.Begin(viewRect);

            DrawGroup(ls, "Precision",
                ref settings.precisionWarmup,
                ref settings.precisionCooldown,
                ref settings.precisionBurst,
                ref settings.precisionAccuracy);

            DrawGroup(ls, "Burst",
                ref settings.burstWarmup,
                ref settings.burstCooldown,
                ref settings.burstBurst,
                ref settings.burstAccuracy);

            DrawGroup(ls, "Suppression",
                ref settings.suppressionWarmup,
                ref settings.suppressionCooldown,
                ref settings.suppressionBurst,
                ref settings.suppressionAccuracy);

            ls.End();
            Widgets.EndScrollView();
        }

        private void DrawGroup(
            Listing_Standard ls,
            string title,
            ref float warmup,
            ref float cooldown,
            ref float burst,
            ref float accuracy)
        {
            float estimatedHeight = 180f;

            Rect rect = ls.GetRect(estimatedHeight);

            Widgets.DrawBox(rect);

            Rect inner = rect.ContractedBy(8f);

            Listing_Standard innerLs = new Listing_Standard();
            innerLs.Begin(inner);

            Text.Font = GameFont.Medium;
            innerLs.Label(title);
            Text.Font = GameFont.Small;

            innerLs.Gap(6f);

            DrawSliderWithInput(innerLs, "Warmup Time Multiplier", ref warmup);
            DrawSliderWithInput(innerLs, "Cooldown Multiplier", ref cooldown);
            DrawSliderWithInput(innerLs, "Burst Count Multiplier", ref burst);
            DrawSliderWithInput(innerLs, "Accuracy Multiplier", ref accuracy);

            innerLs.End();

            ls.Gap(10f);
        }

        private void DrawSliderWithInput(
            Listing_Standard ls,
            string label,
            ref float value,
            float min = 0.25f,
            float max = 4f)
        {
            Rect row = ls.GetRect(30f);

            float labelWidth = 120f;
            float fieldWidth = 60f;
            float gap = 10f;

            Rect labelRect = new Rect(row.x, row.y, labelWidth, row.height);
            Rect sliderRect = new Rect(labelRect.xMax + gap, row.y, row.width - labelWidth - fieldWidth - gap * 2, row.height);
            Rect fieldRect = new Rect(sliderRect.xMax + gap, row.y, fieldWidth, row.height);

            Widgets.Label(labelRect, label);

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
    }
}
