using Verse;
using VFM_VanillaFireModes.Comps;

namespace VFM_VanillaFireModes.Utilities
{
    [StaticConstructorOnStartup]
    public static class VFM_CompsPatcher
    {
        static VFM_CompsPatcher()
        {
            foreach (var def in DefDatabase<ThingDef>.AllDefs)
            {
                if (def.race != null) { 
                    if(def.race.Humanlike || def.race.ToolUser)
                    {
                        if (def.comps == null)
                            def.comps = new List<CompProperties>();

                        if (!def.comps.Any(c => c is VFM_CompProperties_FireMode))
                        {
                            def.comps.Add(new VFM_CompProperties_FireMode());
                        }
                    }
                }
            }
        }
    }
}
