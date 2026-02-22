using LudeonTK;
using RimWorld;
using Verse;
using VFM_VanillaFireModes.Comps;
using VFM_VanillaFireModes.Settings;

namespace VFM_VanillaFireModes
{
    public static class VFM_DevMenu
    {
        private const string Category = "VFM";

        private static bool IsValidTarget(Pawn p)
        {
            return p != null && (p.RaceProps.Humanlike || p.RaceProps.ToolUser);
        }

        // 重置所有VFM Comp数据
        [DebugAction(Category, "Reset VFM Comps", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void ResetAllVFMComps()
        {
            var allTargetPawns = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive.Where(IsValidTarget);

            int count = 0;
            foreach (Pawn p in allTargetPawns)
            {
                var comp = p.GetComp<VFM_PawnCompFireMode>();
                if(comp != null)
                {
                    comp.curMode = FireMode.Default;
                    comp.curEnableAutoSelection = false;
                    count++;
                }
            }
            Log.Message($"Reset VFM Comps to default from {count} Pawns");
        }
    }
}
