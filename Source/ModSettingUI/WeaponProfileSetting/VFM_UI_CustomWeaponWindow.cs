using RimWorld;
using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.ModSettingUI.WeaponProfileSetting;

public class VFM_UI_CustomWeaponWindow : Window
{
    private const float LeftWidthRatio = 0.33f;
    private const float Padding = 10f;
    private const float LeftRowHeight = 50f;
    private const float RightRowHeight = 100f;

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
        // this.closeOnClickedOutside = true;
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
        if (Widgets.ButtonText(addAllRect, "添加全部"))
        {
            // 二次确认弹窗
            Find.WindowStack.Add(
                new VFM_UI_AddAllWeaponsConfirmDialog(GetAllRangedWeapons())
            );
        }

        y += 35f;

        // 滚动列表
        Rect listRect = new Rect(inner.x, y, inner.width, inner.height - (y - inner.y));
        Rect viewRect = new Rect(0, 0, listRect.width - 16f, GetLeftListHeight());

        Widgets.BeginScrollView(listRect, ref leftScrollPos, viewRect);

        Listing_Standard listing = new Listing_Standard();
        listing.Begin(viewRect);

        foreach (var def in GetAllRangedWeapons())
        {
            DrawLeftItem(listing.GetRect(LeftRowHeight), def);
        }

        listing.End();
        Widgets.EndScrollView();
    }

    private const float iconSize = 40f;
    private const float iconTotalWidth = 45f;

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

    /**
     * BurstShotCount 定义在 Verb 上，但是 Verb 并没有全局唯一的标签来用于区分。
     * 而一个ThingDef可能包含多个 Verb，所以选择从 ThingDef 的 Verbs 列表选择第一个 isPrimary == true 的作为主 Verb。
     * 自定义 WeaponProfile 的连发次数直接和武器本身绑定。
     */
    private static VerbProperties? GetPrimaryVerb(ThingDef def)
    {
        return def.Verbs.FirstOrDefault(v => v.isPrimary);
    }

    /**
     * 添加武器至 CustomWeaponProfiles 列表，使用当前全局设置为默认参数
     */
    private static void AddSingleWeapon(string defName, int baseBurstShotCount)
    {
        VFM_WeaponProfile newProfile = new VFM_WeaponProfile(
            defName,
            VFM_FireModeProfile.CreateDefault(baseBurstShotCount),
            VFM_FireModeProfile.CreatePrecision(baseBurstShotCount),
            VFM_FireModeProfile.CreateBurst(baseBurstShotCount),
            VFM_FireModeProfile.CreateSuppression(baseBurstShotCount)
        );
        Settings.CustomWeaponProfiles.Add(defName, newProfile);
    }

    // TODO: 这里可能还需要改一下, 添加校验一下Verbs不为空
    private IEnumerable<ThingDef> GetAllRangedWeapons()
    {
        return DefDatabase<ThingDef>.AllDefs
            .Where(d =>
                d.IsRangedWeapon &&
                IsActualPawnWeapon(d) &&
                (leftSearch.NullOrEmpty() || d.label.ToLower().Contains(leftSearch.ToLower()))
            );
    }

    // TODO：这个cache和方法最好换个位置放
    private static HashSet<ThingDef> _turretWeaponCache;

    /**
     * 仅依靠 def.IsRangedWeapon 找出远程武器是不够的。炮塔的武器也会包含在内。
     * 炮塔用的武器没有单独的标签，需要先找出所有的 Turret 建筑，然后从建筑上找出对应的 turretGunDef
     */
    public static bool IsActualPawnWeapon(ThingDef def)
    {
        // 基础过滤：必须是远程
        if (def == null || !def.IsRangedWeapon)
            return false;

        // 懒加载初始化缓存，找出全游戏所有炮塔正在使用的 turretGunDef
        if (_turretWeaponCache == null)
        {
            _turretWeaponCache = new HashSet<ThingDef>();
            foreach (var d in DefDatabase<ThingDef>.AllDefs)
            {
                if (IsTurret(d))
                {
                    _turretWeaponCache.Add(d.building.turretGunDef);
                }
            }
        }

        // 如果这个 Def 在武器名单里，排除掉
        if (_turretWeaponCache.Contains(def))
            return false;

        // NOTE: 会排除掉原版机械体的部分默认武器，暂时注释掉
        // if (def.destroyOnDrop && def.tradeability == Tradeability.None)
        //     return false;

        return true;
    }

    private static bool IsTurret(ThingDef? t)
    {
        if (t == null)
        {
            return false;
        }

        return t.thingClass == typeof(Building_TurretGun) ||
               (t.thingClass != null && t.thingClass.ToString().Contains("Building_TurretGun")) ||
               (t.building != null && t.building.turretGunDef != null);
    }

    private float GetLeftListHeight()
    {
        return GetAllRangedWeapons().Count() * LeftRowHeight;
    }

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
        if (Widgets.ButtonText(clearRect, "清除全部"))
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

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "精度", "精度倍率");
        x += ModeValueWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "瞄准", "瞄准时间倍率");
        x += ModeValueWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "冷却", "冷却时间倍率");
        x += ModeValueWidth;

        DrawHeaderCell(new Rect(x, rect.y, ModeValueWidth, rect.height), "连发", "连发次数");
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

        // 名字 + defName
        const float labelWidth = 200f;
        float startY = rect.y + rect.height / 2f;
        Text.Anchor = TextAnchor.MiddleLeft;
        float labelHeight = Text.CalcHeight(def.LabelCap, labelWidth);
        Rect labelRect = new Rect(x, startY - labelHeight, 200f, labelHeight);
        Widgets.Label(labelRect, def.LabelCap);
        Text.Font = GameFont.Tiny;
        float defNameHeight = Text.CalcHeight(profile.defName, labelWidth);
        Rect defNameRect = new Rect(x, startY + 1f, 200f, defNameHeight);
        Widgets.Label(defNameRect, profile.defName);

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        x += InfoBlockWidth;

        // 模式名称 + 参数（4行）
        float lineHeight = rect.height / 4f;
        float tableWidth = ModeLabelWidth + ModeValueWidth * 4;

        DrawModeRow(new Rect(x, rect.y + lineHeight * 0, tableWidth, lineHeight), "默认", profile.Default);
        DrawModeRow(new Rect(x, rect.y + lineHeight * 1, tableWidth, lineHeight), "精准", profile.Precision);
        DrawModeRow(new Rect(x, rect.y + lineHeight * 2, tableWidth, lineHeight), "点射", profile.Burst);
        DrawModeRow(new Rect(x, rect.y + lineHeight * 3, tableWidth, lineHeight), "压制", profile.Suppression);

        // 右侧按钮
        float btnX = rect.xMax - 40f;

        Rect deleteRect = new Rect(btnX, rect.y + 5f, 24f, 24f);
        if (Widgets.ButtonImage(deleteRect, TexButton.Delete))
        {
            // 删除
            keysToDelete.Add(key);
        }

        Rect editRect = new Rect(btnX - 10f, rect.y + rect.height - 30f, 40f, 20f);
        if (Widgets.ButtonText(editRect, "编辑"))
        {
            // 打开编辑弹窗
            VerbProperties? weaponVerb = GetPrimaryVerb(def);
            if (weaponVerb != null)
                Find.WindowStack.Add(new VFM_UI_EditWeaponProfileDialog(profile, weaponVerb.burstShotCount));
        }

        Widgets.DrawBox(rect, 1); // 外框线
    }

    private void DrawModeRow(Rect rect, string label, VFM_FireModeProfile data)
    {
        float x = rect.x;
        // TODO：最好改成和header一样： label 用缩写，完整名称用 tooltip显示
        // 模式名
        Rect labelRect = new Rect(x, rect.y, ModeLabelWidth, rect.height);
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(labelRect, label);
        Text.Anchor = TextAnchor.UpperLeft;
        TooltipHandler.TipRegion(labelRect, label);

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

    private float GetRightListHeight()
    {
        return GetProfiles().Count() * (RightRowHeight + rightRowPadding);
    }
}