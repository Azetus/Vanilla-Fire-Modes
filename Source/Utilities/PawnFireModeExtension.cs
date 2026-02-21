using Verse;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Comps;

namespace VFM_VanillaFireModes.Utilities
{
    public static class PawnFireModeExtension
    {
        public static FireMode VFM_GetFireMode(this Pawn pawn)
        {
            if (pawn == null)
                return FireMode.Default;

            var comp = pawn.TryGetComp<VFM_PawnCompFireMode>();

            return comp?.curMode ?? FireMode.Default;
        }

        public static void VFM_SetFireMode(this Pawn pawn, FireMode fireMode) {
            if (pawn == null)
                return;
            var comp = pawn.TryGetComp<VFM_PawnCompFireMode>();

            if (comp is VFM_PawnCompFireMode VFM_comp) {
                VFM_comp.curMode = fireMode;
            }
        }

        public static bool VFM_enableAutoSelection(this Pawn pawn) {
            if(pawn == null)
                return false;

            var comp = pawn.TryGetComp<VFM_PawnCompFireMode>();

            return comp?.curEnableAutoSelection ?? false;
        }
    }
}
