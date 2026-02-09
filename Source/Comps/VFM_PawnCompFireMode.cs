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

        public FireMode curMode
        {
            get => mode;
            set => mode = value;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref mode, "VFM_fireMode", FireMode.Default);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent is Pawn pawn && pawn.IsColonistPlayerControlled && pawn.Drafted && HasRemoteWeapon(pawn))
            {
                yield return new Command_Action
                {
                    icon = GetIconFor(curMode),
                    defaultLabel = GetLabelFor(curMode),
                    defaultDesc = "VFM_SwitchGizmoDesc".Translate(),
                    action = delegate
                    {
                        curMode = (FireMode)(((int)curMode + 1) % 4);
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                };
            }
        }

        private bool HasRemoteWeapon(Pawn pawn)
        {
            return pawn.equipment?.Primary?.def.IsRangedWeapon ?? false;
        }

        private string GetLabelFor(FireMode mode)
        {
            switch (mode)
            {
                case FireMode.Precision: return "VFM_PrecisionMode".Translate();
                case FireMode.Burst: return "VFM_ShortBurstMode".Translate();
                case FireMode.Suppression: return "VFM_SuppressionMode".Translate();
                default: return "VFM_DefaultMode".Translate();
            }
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
