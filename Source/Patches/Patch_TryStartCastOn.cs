using HarmonyLib;
using Verse;
using VFM_VanillaFireModes.Settings;
using VFM_VanillaFireModes.Utilities;

namespace VFM_VanillaFireModes.Patches
{
    public static class Patch_TryStartCastOn
    {
        [HarmonyPatch(typeof(Verb))]
        [HarmonyPatch(nameof(Verb.TryStartCastOn), new Type[] { typeof(LocalTargetInfo), typeof(bool), typeof(bool), typeof(bool), typeof(bool) })]
        public static class Patch_TryStartCastOn_1
        {
            static void Prefix(Verb __instance, LocalTargetInfo castTarg)
            {
                PrefixHelper(__instance, castTarg);
            }
        }

        [HarmonyPatch(typeof(Verb))]
        [HarmonyPatch(nameof(Verb.TryStartCastOn), new Type[] { typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(bool), typeof(bool), typeof(bool), typeof(bool) })]
        public static class Patch_TryStartCastOn_2
        {
            static void Prefix(Verb __instance, LocalTargetInfo castTarg, LocalTargetInfo destTarg)
            {
                PrefixHelper(__instance, castTarg);
            }
        }
        private static void PrefixHelper(Verb __instance, LocalTargetInfo castTarg)
        {
            if (__instance.CasterPawn is not Pawn pawn) return;
            if (pawn == null || !castTarg.IsValid)
                return;
            if (__instance.verbProps == null) return;
            if (!__instance.verbProps.IsMeleeAttack && __instance.EquipmentSource != null)
            {
                if (__instance.EquipmentSource.def.IsRangedWeapon)
                {
                    var settings = FireModeDB.Settings;

                    bool enableForPlayer = settings.enableAutoSelectionForPlayer;
                    bool enableForNPC = settings.enableFireModeForNPC;

                    if (pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled)
                    {
                        if (enableForPlayer && pawn.VFM_enableAutoSelection())
                        {
                            float distance = pawn.Position.DistanceTo(castTarg.Cell);
                            FireMode autoMode = Utils.EvaluateByDistance(distance, settings);
                            pawn.VFM_SetFireMode(autoMode);
                        }
                    }

                    if (!pawn.IsColonistPlayerControlled && !pawn.IsColonyMechPlayerControlled)
                    {
                        if (enableForNPC)
                        {
                            float distance = pawn.Position.DistanceTo(castTarg.Cell);
                            FireMode autoMode = Utils.EvaluateByDistance(distance, settings);
                            pawn.VFM_SetFireMode(autoMode);
                        }
                    }
                }
            }

        }
    }
}
