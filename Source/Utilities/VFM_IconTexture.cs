using UnityEngine;
using Verse;

namespace VFM_VanillaFireModes.Utilities
{
    [StaticConstructorOnStartup]
    public static class VFM_IconTexture
    {
        public static readonly Texture2D VFM_Default_Icon = ContentFinder<Texture2D>.Get("Icon/VFM_Default_icon"); 
        public static readonly Texture2D VFM_Precision_Icon = ContentFinder<Texture2D>.Get("Icon/VFM_Precision_icon"); 
        public static readonly Texture2D VFM_Burst_Icon = ContentFinder<Texture2D>.Get("Icon/VFM_Burst_icon");
        public static readonly Texture2D VFM_Suppression_Icon = ContentFinder<Texture2D>.Get("Icon/VFM_Suppression_icon");
        public static readonly Texture2D VFM_Auto_Icon = ContentFinder<Texture2D>.Get("Icon/VFM_Auto_icon");
    }
}
