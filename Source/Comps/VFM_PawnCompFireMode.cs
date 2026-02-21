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
        private FireMode mode = FireMode.Default;
        private bool enableAutoSelection = false;
        public FireMode curMode
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
            Scribe_Values.Look(ref mode, "VFM_fireMode", FireMode.Default);
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
                            curMode = (FireMode)(((int)curMode + 1) % 4);
                            SoundDefOf.Tick_High.PlayOneShotOnCamera();
                        }
                    };
                }
                
                if (FireModeDB.Settings.enableAutoSelectionForPlayer)
                {
                    yield return new Command_Toggle
                    {
                        defaultLabel = "VFM_AutoSelection".Translate(),
                        defaultDesc = "VFM_AutoSelection_Desc".Translate(),
                        isActive = () => curEnableAutoSelection,
                        toggleAction = () =>
                        {
                            curEnableAutoSelection = !curEnableAutoSelection;
                            curMode = FireMode.Default;
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

        private Texture2D GetIconFor(FireMode mode)
        {
            switch (mode)
            {
                case FireMode.Precision: return VFM_IconTexture.VFM_Precision_Icon;
                case FireMode.Burst: return VFM_IconTexture.VFM_Burst_Icon;
                case FireMode.Suppression: return VFM_IconTexture.VFM_Suppression_Icon;
                default: return VFM_IconTexture.VFM_Default_Icon;
            }
        }
    }
}
