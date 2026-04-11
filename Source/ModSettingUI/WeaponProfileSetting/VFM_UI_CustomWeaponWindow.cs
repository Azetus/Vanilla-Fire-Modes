using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;
using static VFM_VanillaFireModes.Utilities.WeaponProfileUtils;

namespace VFM_VanillaFireModes.ModSettingUI.WeaponProfileSetting;

public class VFM_UI_CustomWeaponWindow : Window
{
    private const float LeftWidthRatio = 0.33f;
    private const float Padding = 10f;
    private const float LeftRowHeight = 50f;
    private const float RightRowHeight = 100f;

    private const float iconSize = 40f;
    private const float iconTotalWidth = 45f;

    private Vector2 leftScrollPos;
    private Vector2 rightScrollPos;

    private string leftSearch = "";
    private string rightSearch = "";

    public override Vector2 InitialSize => new Vector2(1200f, 700f);

    private static VanillaFireModesModSetting Settings => VanillaFireModes.settings;

    public VFM_UI_CustomWeaponWindow()
    {
        this.doCloseX = true;
        // this.doCloseButton = true;
        this.absorbInputAroundWindow = true;
        this.closeOnClickedOutside = true;
    }

    public override void DoWindowContents(Rect inRect)
    {
        GUI.BeginGroup(inRect);
        Rect leftRect = new Rect(
            inRect.x,
            inRect.y,
            inRect.width * LeftWidthRatio - Padding,
            inRect.height
        );

        Rect rightRect = new Rect(
            leftRect.xMax + Padding,
            inRect.y,
            inRect.width * (1f - LeftWidthRatio) - Padding,
            inRect.height
        );

        DrawLeftBlock(leftRect);
        DrawRightBlock(rightRect);
        GUI.EndGroup();
    }


    private IEnumerable<KeyValuePair<string, VFM_WeaponProfile>> GetProfiles()
    {
        return Settings.CustomWeaponProfiles
            .Where(kv =>
                {
                    ThingDef? def = DefDatabase<ThingDef>.GetNamedSilentFail(kv.Value.defName);
                    return def != null && (rightSearch.NullOrEmpty() ||
                                           def.label.ToLower().Contains(rightSearch.ToLower()));
                }
            );
    }


    #region LeftBlock

    private void DrawLeftBlock(Rect rect)
    {
        Widgets.DrawMenuSection(rect);

        Rect inner = rect.ContractedBy(Padding);

        float y = inner.y;

        // 搜索框
        Rect searchRect = new Rect(inner.x, y, inner.width, 30f);
        leftSearch = Widgets.TextField(searchRect, leftSearch);
        y += 35f;

        // “添加全部”按钮（右对齐）
        Rect addAllRect = new Rect(inner.x + inner.width - 120f, y, 120f, 30f);
        if (Widgets.ButtonText(addAllRect, "VFM_Profile_AddAllWeapons_Label".Translate()))
        {
            // 二次确认弹窗
            Find.WindowStack.Add(
                new VFM_UI_AddAllWeaponsConfirmDialog()
            );
        }

        y += 35f;

        // 滚动列表
        Rect listRect = new Rect(inner.x, y, inner.width, inner.height - (y - inner.y));
        Rect viewRect = new Rect(0, 0, listRect.width - 16f, GetLeftListHeight());

        Widgets.BeginScrollView(listRect, ref leftScrollPos, viewRect);

        Listing_Standard listing = new Listing_Standard();
        listing.Begin(viewRect);

        foreach (var def in GetAllRangedWeaponsWithSearch(leftSearch))
        {
            DrawLeftItem(listing.GetRect(LeftRowHeight), def);
        }

        listing.End();
        Widgets.EndScrollView();
    }


    private void DrawLeftItem(Rect rect, ThingDef def)
    {
        Widgets.DrawHighlightIfMouseover(rect);

        // Tooltip 显示 defName
        TooltipHandler.TipRegion(rect, def.defName);

        float x = rect.x;

        // 图标
        Rect iconRect = new Rect(x, rect.y + 5f, iconSize, iconSize);
        Widgets.DrawTextureFitted(iconRect, def.uiIcon, 1.0f);
        x += iconTotalWidth;

        // 名字
        Rect labelRect = new Rect(x, rect.y, 200f, rect.height);
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(labelRect, def.LabelCap);
        Text.Anchor = TextAnchor.UpperLeft;
        x += 210f;

        // 添加单个武器（右箭头）
        Rect arrowRect = new Rect(rect.xMax - 30f, rect.y + 10f, 24f, 24f);
        if (Widgets.ButtonImage(arrowRect, TexButton.Reveal))
        {
            if (!Settings.CustomWeaponProfiles.ContainsKey(def.defName))
            {
                VerbProperties? weaponVerb = GetPrimaryVerb(def);
                if (weaponVerb != null)
                    AddSingleWeapon(def.defName, weaponVerb.burstShotCount);
            }
        }
    }

    private float GetLeftListHeight()
    {
        return GetAllRangedWeaponsWithSearch(leftSearch).Count() * LeftRowHeight;
    }

    #endregion

    #region RightBlock

    private const float rightRowPadding = 5f;

    private void DrawRightBlock(Rect rect)
    {
        Widgets.DrawMenuSection(rect);

        Rect inner = rect.ContractedBy(Padding);

        float y = inner.y;

        // 搜索框
        Rect searchRect = new Rect(inner.x, y, inner.width, 30f);
        rightSearch = Widgets.TextField(searchRect, rightSearch);
        y += 35f;

        // 清除全部按钮
        Rect clearRect = new Rect(inner.x + inner.width - 120f, y, 120f, 30f);
        if (Widgets.ButtonText(clearRect, "VFM_Profile_RemoveAllProfile_Label".Translate()))
        {
            Settings.CustomWeaponProfiles.Clear();
        }

        y += 35f;

        // 参数表头
        Rect headerRect = new Rect(inner.x, y, inner.width, 30f);
        DrawRightHeader(headerRect);
        y += 35f;

        // 列表
        Rect listRect = new Rect(inner.x, y, inner.width, inner.height - (y - inner.y));
        Rect viewRect = new Rect(0, 0, listRect.width - 16f, GetRightListHeight());

        Widgets.BeginScrollView(listRect, ref rightScrollPos, viewRect);

        float curY = 0f;
        var keysToDelete = new List<string>();
        foreach (var kv in GetProfiles())
        {
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(kv.Value.defName);
            if (def == null) continue;
            curY += rightRowPadding;
            Rect row = new Rect(0, curY, viewRect.width, RightRowHeight);
            DrawRightItem(row, kv.Key, kv.Value, def, keysToDelete);
            curY += RightRowHeight;
        }

        foreach (var key in keysToDelete)
        {
            if (key != null)
                Settings.CustomWeaponProfiles.Remove(key);
        }

        Widgets.EndScrollView();
    }

    private const float ModeLabelWidth = 60f;
    private const float ModeValueWidth = 90f;
    private const float InfoBlockWidth = 210f;

    private void DrawRightHeader(Rect rect)
    {
        // 平移一下 header
        float x = rect.x + 5f + iconTotalWidth + InfoBlockWidth + ModeLabelWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "VFM_Accuracy_Abbr".Translate(), "VFM_Accuracy_Label".Translate());
        x += ModeValueWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "VFM_Warmup_Abbr".Translate(), "VFM_Warmup_Label".Translate());
        x += ModeValueWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "VFM_Cooldown_Abbr".Translate(), "VFM_Cooldown_Label".Translate());
        x += ModeValueWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "VFM_BurstCount_Abbr".Translate(), "VFM_BurstCount_Label".Translate());
    }

    /**
     * label 用缩写，完整名称用 tooltip显示
     */
    private void DrawHeaderCell(Rect rect, string label, string tooltip)
    {
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect, label);
        Text.Anchor = TextAnchor.UpperLeft;

        TooltipHandler.TipRegion(rect, tooltip);
    }

    private void DrawRightItem(Rect rect, string key, VFM_WeaponProfile profile, ThingDef def, List<string> keysToDelete)
    {
        Widgets.DrawHighlightIfMouseover(rect);

        float x = rect.x + 5f;

        // 图标
        Rect iconRect = new Rect(x, rect.y + (rect.height - iconSize) / 2f, iconSize, iconSize);
        Widgets.ThingIcon(iconRect, def);
        x += iconTotalWidth;

        // 名字 + defName(tooltip)
        const float labelWidth = 200f;
        Text.Anchor = TextAnchor.MiddleLeft;
        Rect labelRect = new Rect(x, rect.y, labelWidth, rect.height);
        Widgets.Label(labelRect, def.LabelCap);
        TooltipHandler.TipRegion(labelRect, profile.defName);
        Text.Anchor = TextAnchor.UpperLeft;

        x += InfoBlockWidth;

        // 模式名称 + 参数（4行）
        float lineHeight = rect.height / 4f;
        float tableWidth = ModeLabelWidth + ModeValueWidth * 4;

        DrawModeRow(new Rect(x, rect.y + lineHeight * 0, tableWidth, lineHeight),
            "VFM_DefaultMode_Abbr".Translate(),
            "VFM_DefaultMode".Translate(),
            profile.Default);
        DrawModeRow(new Rect(x, rect.y + lineHeight * 1, tableWidth, lineHeight),
            "VFM_PrecisionMode_Abbr".Translate(),
            "VFM_PrecisionMode".Translate(),
            profile.Precision);
        DrawModeRow(new Rect(x, rect.y + lineHeight * 2, tableWidth, lineHeight),
            "VFM_ShortBurstMode_Abbr".Translate(),
            "VFM_ShortBurstMode".Translate(),
            profile.Burst);
        DrawModeRow(new Rect(x, rect.y + lineHeight * 3, tableWidth, lineHeight),
            "VFM_SuppressionMode_Abbr".Translate(),
            "VFM_SuppressionMode".Translate(),
            profile.Suppression);

        // 右侧按钮
        float btnX = rect.xMax - 40f;

        Rect deleteRect = new Rect(btnX, rect.y + 5f, 24f, 24f);
        if (Widgets.ButtonImage(deleteRect, TexButton.Delete))
        {
            // 删除
            keysToDelete.Add(key);
        }

        TooltipHandler.TipRegion(deleteRect, "VFM_Delete_Button_Label".Translate());

        Rect editRect = new Rect(btnX, rect.y + rect.height - 30f, 24f, 24f);
        if (Widgets.ButtonImage(editRect, TexButton.Rename))
        {
            // 打开编辑弹窗
            VerbProperties? weaponVerb = GetPrimaryVerb(def);
            if (weaponVerb != null)
                Find.WindowStack.Add(new VFM_UI_EditWeaponProfileDialog(profile, weaponVerb.burstShotCount));
        }

        TooltipHandler.TipRegion(editRect, "VFM_Edit_Button_Label".Translate());

        Widgets.DrawBox(rect, 1); // 外框线
    }

    private void DrawModeRow(Rect rect, string label, string tooltipStr, VFM_FireModeProfile data)
    {
        float x = rect.x;
        // 模式名
        Rect labelRect = new Rect(x, rect.y, ModeLabelWidth, rect.height);
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(labelRect, label);
        Text.Anchor = TextAnchor.UpperLeft;
        TooltipHandler.TipRegion(labelRect, tooltipStr);

        x += ModeLabelWidth;

        // 设置数据
        float acc = data?.accuracyMultiplier ?? 1f;
        float warm = data?.warmupMultiplier ?? 1f;
        float cool = data?.cooldownMultiplier ?? 1f;
        int burst = data?.burstShotCount ?? 1;

        // 参数分四列展示
        DrawValueCell(new Rect(x, rect.y, ModeValueWidth, rect.height), acc.ToString("0.##"));
        x += ModeValueWidth;

        DrawValueCell(new Rect(x, rect.y, ModeValueWidth, rect.height), warm.ToString("0.##"));
        x += ModeValueWidth;

        DrawValueCell(new Rect(x, rect.y, ModeValueWidth, rect.height), cool.ToString("0.##"));
        x += ModeValueWidth;

        DrawValueCell(new Rect(x, rect.y, ModeValueWidth, rect.height), burst.ToString());
    }

    private void DrawValueCell(Rect rect, string text)
    {
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect, text);
        Text.Anchor = TextAnchor.UpperLeft;
    }


    private float GetRightListHeight()
    {
        return GetProfiles().Count() * (RightRowHeight + rightRowPadding);
    }

    #endregion
}