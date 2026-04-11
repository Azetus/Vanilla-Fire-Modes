using UnityEngine;
using Verse;
using static VFM_VanillaFireModes.Utilities.WeaponProfileUtils;

namespace VFM_VanillaFireModes.ModSettingUI.WeaponProfileSetting;

public class VFM_UI_AddAllWeaponsConfirmDialog : Window
{
    private const float ButtonWidth = 120f;
    private const float ButtonHeight = 35f;
    private const float Spacing = 20f;
    private const float BottomPadding = 10f;

    public override Vector2 InitialSize => new Vector2(400f, 300f);

    public VFM_UI_AddAllWeaponsConfirmDialog()
    {
        doCloseX = true;
        // doCloseButton = true;
        // draggable = true;
        closeOnClickedOutside = false;
    }

    public override void DoWindowContents(Rect inRect)
    {
        // 按钮预留空间
        Rect contentRect = new Rect(
            inRect.x,
            inRect.y,
            inRect.width,
            inRect.height - ButtonHeight - BottomPadding
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
        float totalWidth = ButtonWidth * 2 + Spacing;

        float startX = inRect.x + (inRect.width - totalWidth) / 2f;
        float y = inRect.yMax - ButtonHeight - BottomPadding;

        Rect confirmRect = new Rect(startX, y, ButtonWidth, ButtonHeight);
        Rect cancelRect = new Rect(startX + ButtonWidth + Spacing, y, ButtonWidth, ButtonHeight);

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
        foreach (var def in GetAllRangedWeapons())
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
}