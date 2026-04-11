using RimWorld;
using Verse;
using VFM_VanillaFireModes.Settings.CustomWeaponProfile;

namespace VFM_VanillaFireModes.Utilities;

internal static class WeaponProfileUtils
{
    /**
     * BurstShotCount 定义在 Verb 上，但是 Verb 并没有全局唯一的标签来用于区分。
     * 而一个ThingDef可能包含多个 Verb，所以选择从 ThingDef 的 Verbs 列表选择第一个 isPrimary == true 的作为主 Verb。
     * 自定义 WeaponProfile 的连发次数直接和武器本身绑定。
     */
    public static VerbProperties? GetPrimaryVerb(ThingDef def)
    {
        return def.Verbs?.FirstOrDefault(v => v.defaultProjectile != null);
    }

    /**
     * 添加武器至 CustomWeaponProfiles 列表，使用当前全局设置为默认参数
     */
    public static void AddSingleWeapon(string defName, int baseBurstShotCount)
    {
        VFM_WeaponProfile newProfile = new VFM_WeaponProfile(
            defName,
            VFM_FireModeProfile.CreateDefault(baseBurstShotCount),
            VFM_FireModeProfile.CreatePrecision(baseBurstShotCount),
            VFM_FireModeProfile.CreateBurst(baseBurstShotCount),
            VFM_FireModeProfile.CreateSuppression(baseBurstShotCount)
        );

        if (VanillaFireModes.settings?.CustomWeaponProfiles != null)
            VanillaFireModes.settings.CustomWeaponProfiles.Add(defName, newProfile);
    }

    public static IEnumerable<ThingDef> GetAllRangedWeaponsWithSearch(string leftSearch)
    {
        return GetAllRangedWeapons().Where(d => leftSearch.NullOrEmpty() || d.label.ToLower().Contains(leftSearch.ToLower()));
    }

    public static IEnumerable<ThingDef> GetAllRangedWeapons()
    {
        return DefDatabase<ThingDef>.AllDefs
            .Where(d =>
                d != null &&
                !d.defName.NullOrEmpty() &&
                d.IsRangedWeapon &&
                GetPrimaryVerb(d) != null &&
                IsActualPawnRangedWeapon(d)
            );
    }

    /**
     * 仅依靠 def.IsRangedWeapon 找出远程武器是不够的。炮塔的武器也会包含在内。
     * 炮塔用的武器没有单独的标签，需要先找出所有的 Turret 建筑，然后从建筑上找出对应的 turretGunDef
     */
    public static bool IsActualPawnRangedWeapon(ThingDef def)
    {
        // 基础过滤：必须是远程
        if (def == null || !def.IsRangedWeapon)
            return false;

        // 如果这个 Def 在武器名单里，排除掉
        if (TurretWeaponSet.Contains(def))
            return false;

        // NOTE: 会排除掉原版机械体的部分默认武器，暂时注释掉
        // if (def.destroyOnDrop && def.tradeability == Tradeability.None)
        //     return false;

        return true;
    }

    private static HashSet<ThingDef> _turretWeaponCache;

    internal static HashSet<ThingDef> TurretWeaponSet
    {
        get
        {
            if (_turretWeaponCache == null)
            {
                _turretWeaponCache = new HashSet<ThingDef>();
                foreach (var d in DefDatabase<ThingDef>.AllDefs)
                {
                    if (d != null && !d.defName.NullOrEmpty() && IsTurret(d))
                    {
                        if (d.building?.turretGunDef != null)
                            _turretWeaponCache.Add(d.building.turretGunDef);
                    }
                }
            }

            return _turretWeaponCache;
        }
    }


    public static bool IsTurret(ThingDef? t)
    {
        if (t == null)
            return false;

        return (t.thingClass != null && t.thingClass == typeof(Building_TurretGun)) || (t.building != null && t.building.turretGunDef != null);
    }
}