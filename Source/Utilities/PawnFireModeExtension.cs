using Verse;
using VFM_VanillaFireModes.Comps;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes.Utilities
{
    public static class PawnFireModeExtension
    {
        // 只控制模式效果是否应用，不改变 Pawn 保存的模式或自动选择状态。
        public static bool VFM_ShouldApplyFireMode(this Pawn pawn)
        {
            return pawn.IsColonistPlayerControlled
                || pawn.IsColonyMechPlayerControlled
                || FireModeDB.Settings.enableFireModeForNPC;
        }

        public static VFM_FireMode VFM_GetFireMode(this Pawn pawn)
        {
            if (pawn == null)
                return VFM_FireMode.Default;

            var comp = pawn.TryGetComp<VFM_PawnCompFireMode>();

            return comp?.curMode ?? VFM_FireMode.Default;
        }

        public static void VFM_SetFireMode(this Pawn pawn, VFM_FireMode fireMode) {
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
