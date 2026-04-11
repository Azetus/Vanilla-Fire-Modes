using UnityEngine;
using Verse;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.ModSettingUI.WeaponProfileSetting;

public class VFM_UI_AddAllWeaponsConfirmDialog : Window
{
    private readonly IEnumerable<ThingDef> weapons;

    public override Vector2 InitialSize => new Vector2(400f, 300f);

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
        float buttonHeight = 35f;
        float spacing = 20f;
        float bottomPadding = 10f;

        // 1. 先给按钮预留底部空间
        Rect contentRect = new Rect(
            inRect.x,
            inRect.y,
            inRect.width,
            inRect.height - buttonHeight - bottomPadding
        );
        var listing = new Listing_Standard();
        listing.Begin(contentRect);

        listing.Label("VFM_AddAllWeapons_Dialog_1".Translate());
        listing.Gap(10f);
        Color oldColor = GUI.color;
        GUI.color = Color.yellow;
        listing.Label("VFM_AddAllWeapons_Dialog_2".Translate());
        GUI.color = oldColor;

        listing.End();

        // 按钮区域
        float buttonWidth = 120f;
        float totalWidth = buttonWidth * 2 + spacing;

        float startX = inRect.x + (inRect.width - totalWidth) / 2f;
        float y = inRect.yMax - buttonHeight - bottomPadding;

        Rect confirmRect = new Rect(startX, y, buttonWidth, buttonHeight);
        Rect cancelRect = new Rect(startX + buttonWidth + spacing, y, buttonWidth, buttonHeight);

        if (Widgets.ButtonText(confirmRect, "VFM_Confirm_Button_Label".Translate()))
        {
            DoAddAllWeapons();
            Close();
        }

        if (Widgets.ButtonText(cancelRect, "VFM_Cancel_Button_Label".Translate()))
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