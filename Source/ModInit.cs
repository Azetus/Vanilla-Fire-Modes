using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using VFM_VanillaFireModes.ModSettingUI;
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


        public override void DoSettingsWindowContents(Rect inRect)
        {
            VFM_SettingsWindowContents.SettingsWindowContents(inRect, ref settings);
        }

    }
}
