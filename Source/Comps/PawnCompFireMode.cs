using RimWorld;
using Verse.Sound;
using Verse;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes.Comps
{
    public class PawnCompFireMode : ThingComp
    {
        private FireMode mode = FireMode.Default;

        public FireMode curMode
        {
            get => mode;
            set => mode = value;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref mode, "VFC_fireMode", FireMode.Default);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent is Pawn pawn && pawn.IsColonistPlayerControlled && pawn.Drafted && HasRemoteWeapon(pawn))
            {
                yield return new Command_Action
                {
                    defaultLabel = GetLabelFor(curMode),
                    defaultDesc = "Switch Fire Mode",
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
                case FireMode.Precision: return "Precision";
                case FireMode.Burst: return "Burst";
                case FireMode.Suppression: return "Suppression";
                default: return "Default";
            }
        }
    }
}
