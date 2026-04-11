using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.ModSettingUI.WeaponProfileSetting;

public class VFM_UI_AddAllWeaponsConfirmDialog : Window
{
    private readonly IEnumerable<ThingDef> weapons;
    
    public override Vector2 InitialSize => new Vector2(500f, 250f);

    public VFM_UI_AddAllWeaponsConfirmDialog(IEnumerable<ThingDef> weapons)
    {
        this.weapons = weapons;
        
        doCloseX = true;
        // doCloseButton = true;
        // draggable = true;
        closeOnClickedOutside = false;
    }

    public override void DoWindowContents(Rect inRect)
    {
        float y = inRect.y;

        // 提示文字
        Rect labelRect = new Rect(inRect.x, y, inRect.width, 100f);
        Text.Anchor = TextAnchor.UpperLeft;

        Widgets.Label(labelRect,
            "你即将将所有远程武器加入自定义列表。\n\n" +
            "该操作可能导致列表过长，并影响性能或可读性。\n\n" +
            "是否继续？");

        y += 110f;

        // 按钮区域
        float buttonWidth = 120f;
        float spacing = 20f;
        float totalWidth = buttonWidth * 2 + spacing;

        float startX = inRect.x + (inRect.width - totalWidth) / 2f;

        // 确认按钮
        Rect confirmRect = new Rect(startX, y, buttonWidth, 35f);
        if (Widgets.ButtonText(confirmRect, "确认"))
        {
            DoAddAllWeapons();
            Close();
        }

        // 取消按钮
        Rect cancelRect = new Rect(startX + buttonWidth + spacing, y, buttonWidth, 35f);
        if (Widgets.ButtonText(cancelRect, "取消"))
        {
            Close();
        }
    }

    private void DoAddAllWeapons()
    {
        foreach (var def in weapons)
        {
            if (VanillaFireModes.settings.CustomWeaponProfiles.ContainsKey(def.defName))
                continue;

            VerbProperties? weaponVerb = GetPrimaryVerb(def);
            if (weaponVerb != null)
            {
                AddSingleWeapon(def.defName, weaponVerb.burstShotCount);
            }
        }

        VanillaFireModes.settings.Write();
    }

    // TODO: 这几个方法记得抽取到工具类里
    private VerbProperties? GetPrimaryVerb(ThingDef def)
    {
        return def.Verbs?.FirstOrDefault(v => v.defaultProjectile != null);
    }

    private void AddSingleWeapon(string defName, int baseBurstShotCount)
    {
        VFM_WeaponProfile newProfile = new VFM_WeaponProfile(
            defName,
            VFM_FireModeProfile.CreateDefault(baseBurstShotCount),
            VFM_FireModeProfile.CreatePrecision(baseBurstShotCount),
            VFM_FireModeProfile.CreateBurst(baseBurstShotCount),
            VFM_FireModeProfile.CreateSuppression(baseBurstShotCount)
        );

        VanillaFireModes.settings.CustomWeaponProfiles.Add(defName, newProfile);
    }
}