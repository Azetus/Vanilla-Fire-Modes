using Verse;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Comps;

namespace VFM_VanillaFireModes.Utilities
{
    public static class PawnFireModeExtension
    {
        public static FireMode GetFireMode(this Pawn pawn)
        {
            if (pawn == null)
                return FireMode.Default;

            var comp = pawn.TryGetComp<PawnCompFireMode>();

            return comp?.curMode ?? FireMode.Default;
        }
    }
}
