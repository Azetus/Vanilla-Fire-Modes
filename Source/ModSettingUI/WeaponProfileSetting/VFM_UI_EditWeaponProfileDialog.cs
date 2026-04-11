using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.ModSettingUI.WeaponProfileSetting;

public class VFM_UI_EditWeaponProfileDialog : Window
{
    private readonly VFM_WeaponProfile profile;
    private readonly int baseBurstShotCount;

    private const float labelHeight = 25f;
    private const float fieldHeight = 50f;
    private const float blockHeight = 120f;

    private const float floatInputMin = 0.1f;
    private const float floatInputMax = 100.0f;
    private const int intInputMin = 1;
    private const int intInputMax = 100;

    public override Vector2 InitialSize => new Vector2(700f, 600f);

    public VFM_UI_EditWeaponProfileDialog(VFM_WeaponProfile profile, int baseBurstShotCount)
    {
        this.profile = profile;
        this.baseBurstShotCount = baseBurstShotCount;

        doCloseX = true;
        draggable = true;
        closeOnClickedOutside = false;
        doCloseButton = true;
    }

    public override void DoWindowContents(Rect inRect)
    {
        float y = inRect.y;
        const float blockPadding = 10f;

        DrawModeEditor(new Rect(inRect.x, y, inRect.width, blockHeight), "VFM_DefaultMode".Translate(), profile.Default);
        y += blockHeight + blockPadding;

        DrawModeEditor(new Rect(inRect.x, y, inRect.width, blockHeight), "VFM_PrecisionMode".Translate(), profile.Precision);
        y += blockHeight + blockPadding;

        DrawModeEditor(new Rect(inRect.x, y, inRect.width, blockHeight), "VFM_ShortBurstMode".Translate(), profile.Burst);
        y += blockHeight + blockPadding;

        DrawModeEditor(new Rect(inRect.x, y, inRect.width, blockHeight), "VFM_SuppressionMode".Translate(), profile.Suppression);
        y += blockHeight + blockPadding;

        Rect cancelRect = new Rect(inRect.x + inRect.width - 130f, y, 120f, 35f);
        if (Widgets.ButtonText(cancelRect, "VFM_ResetButton_Label".Translate()))
        {
            profile.Default = VFM_FireModeProfile.CreateDefault(baseBurstShotCount);
            profile.Precision = VFM_FireModeProfile.CreatePrecision(baseBurstShotCount);
            profile.Burst = VFM_FireModeProfile.CreateBurst(baseBurstShotCount);
            profile.Suppression = VFM_FireModeProfile.CreateSuppression(baseBurstShotCount);
        }
    }

    public override void PreClose()
    {
        base.PreClose();
        VanillaFireModes.settings.Write();
    }

    private void DrawModeEditor(Rect rect, string label, VFM_FireModeProfile data)
    {
        Widgets.DrawMenuSection(rect);

        Rect inner = rect.ContractedBy(8f);

        float y = inner.y;

        // 标题
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(new Rect(inner.x, y, inner.width, 30f), label);
        Text.Anchor = TextAnchor.UpperLeft;

        y += 35f;

        float colWidth = inner.width / 4f;

        // 四个输入框
        DrawFloatField(new Rect(inner.x + colWidth * 0, y, colWidth, fieldHeight),
            "VFM_Accuracy_Label".Translate(), ref data.accuracyMultiplier, floatInputMin, floatInputMax);

        DrawFloatField(new Rect(inner.x + colWidth * 1, y, colWidth, fieldHeight),
            "VFM_Warmup_Label".Translate(), ref data.warmupMultiplier, floatInputMin, floatInputMax);

        DrawFloatField(new Rect(inner.x + colWidth * 2, y, colWidth, fieldHeight),
            "VFM_Cooldown_Label".Translate(), ref data.cooldownMultiplier, floatInputMin, floatInputMax);

        DrawIntField(new Rect(inner.x + colWidth * 3, y, colWidth, fieldHeight),
            "VFM_BurstCount_Label".Translate(), ref data.burstShotCount, intInputMin, intInputMax);
    }

    private void DrawFloatField(Rect rect, string label, ref float value, float min, float max)
    {
        Widgets.Label(new Rect(rect.x, rect.y, rect.width, labelHeight), label);

        Rect fieldRect = new Rect(rect.x, rect.y + labelHeight, rect.width, rect.height - labelHeight);

        string buffer = value.ToString("0.##");
        string newBuffer = Widgets.TextField(fieldRect, buffer);

        if (float.TryParse(newBuffer, out float result))
        {
            value = Mathf.Clamp(result, min, max);
        }
    }

    private void DrawIntField(Rect rect, string label, ref int value, int min, int max)
    {
        Widgets.Label(new Rect(rect.x, rect.y, rect.width, labelHeight), label);

        Rect fieldRect = new Rect(rect.x, rect.y + labelHeight, rect.width, rect.height - labelHeight);

        string buffer = value.ToString();
        string newBuffer = Widgets.TextField(fieldRect, buffer);

        if (int.TryParse(newBuffer, out int result))
        {
            value = Mathf.Clamp(result, min, max);
        }
    }
}