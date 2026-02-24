using RimWorld;
using Verse.Sound;
using Verse;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Utilities;
using UnityEngine;

namespace VFM_VanillaFireModes.Comps
{
    public class VFM_PawnCompFireMode : ThingComp
    {
        private VFM_FireMode mode = VFM_FireMode.Default;
        private bool enableAutoSelection = false;
        public VFM_FireMode curMode
        {
            get => mode;
            set => mode = value;
        }

        public bool curEnableAutoSelection
        {
            get => enableAutoSelection;
            set => enableAutoSelection = value;
        }


        public override void PostExposeData()
        {
            Scribe_Values.Look(ref mode, "VFM_fireMode", VFM_FireMode.Default);
            Scribe_Values.Look(ref enableAutoSelection, "VFM_autoSelection", false);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent is Pawn pawn &&
                (pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled) &&
                pawn.Drafted &&
                HasRemoteWeapon(pawn))
            {
                if (!curEnableAutoSelection || !FireModeDB.Settings.enableAutoSelectionForPlayer)
                {
                    yield return new Command_Action
                    {
                        icon = GetIconFor(curMode),
                        defaultLabel = Utils.GetFireModeLabelFor(curMode),
                        defaultDesc = "VFM_SwitchGizmoDesc".Translate(),
                        action = () =>
                        {
                            curMode = (VFM_FireMode)(((int)curMode + 1) % 4);
                            SoundDefOf.Tick_High.PlayOneShotOnCamera();
                        }
                    };
                }
                
                if (FireModeDB.Settings.enableAutoSelectionForPlayer)
                {
                    yield return new Command_Toggle
                    {
                        icon = VFM_IconTexture.VFM_Auto_Icon,
                        defaultLabel = "VFM_AutoSelection".Translate(),
                        defaultDesc = "VFM_AutoSelection_Desc".Translate(),
                        isActive = () => curEnableAutoSelection,
                        toggleAction = () =>
                        {
                            curEnableAutoSelection = !curEnableAutoSelection;
                            curMode = VFM_FireMode.Default;
                            SoundDefOf.Tick_High.PlayOneShotOnCamera();
                        }
                    };
                }
            }
        }

        private bool HasRemoteWeapon(Pawn pawn)
        {
            return pawn.equipment?.Primary?.def.IsRangedWeapon ?? false;
        }

        private Texture2D GetIconFor(VFM_FireMode mode)
        {
            switch (mode)
            {
                case VFM_FireMode.Precision: return VFM_IconTexture.VFM_Precision_Icon;
                case VFM_FireMode.Burst: return VFM_IconTexture.VFM_Burst_Icon;
                case VFM_FireMode.Suppression: return VFM_IconTexture.VFM_Suppression_Icon;
                default: return VFM_IconTexture.VFM_Default_Icon;
            }
        }
    }
}
