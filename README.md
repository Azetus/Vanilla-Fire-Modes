# Vanilla-Fire-Modes

一个《边缘世界》游戏 Mod，受到 Combat Extended 启发，在不改变原版战斗框架的前提下，为远程武器加入可选射击模式。

## 主要功能

- 提供4种射击模式：默认 / 精确射击 / 短点射 / 压制射击
- 影响精准度、瞄准时间、冷却时间、连射次数
- 突破原版武器连射次数限制，让远程武器像真正的“全自动”武器一样射击
- 根据距离自动切换射击模式（玩家与NPC均适用，可关闭）
- 支持机械族（含Mod添加）
- 所有参数可在 Mod 设置中调整，支持每种武器单独设置

## 注意事项

1. 连射加成不会作用于单发武器以及一次性武器
2. 部分武器的连射加成可能无法生效，比如异象DLC中的"焚烬者喷射器"
3. 仅装备远程主武器时生效

## 兼容性

该模组采用了尽可能低侵入性的实现方式，采用 Harmony Patch 仅在如下几个节点注入逻辑：`Verb.WarmupComplete`（缓存连射次数）、
`Verb.BurstShotCount`(修改连射次数)、`Verb.VerbTick`（清除连射次数缓存）、`Verb.TryStartCastOn`（自动切换射击模式的判断入口）。
精准度、瞄准时间、冷却时间等属性的影响通过运行时 `StatPart` 注入的方式叠加，不覆盖原版 StatWorker，与其他修改同类属性的 Mod 自然共存。
理论上可以兼容绝大部分 Mod，包括但不限于各类武器、种族、机械体模组，以及类似"Vanilla Combat Reloaded"、"Yayo'sCombat3" 或 "RunAndGun" 这样的战斗机制模组。

## FAQ

**Q：CE？**  
A：仅保证无报错，***不推荐*** 同时使用。

**Q: 可以中途加入/移除吗？**  
A: 可以（移除理论上安全）。


---

# Vanilla-Fire-Modes

A Rimworld mod, inspired by Combat Extended, this mod expands vanilla ranged combat by adding selectable fire modes to ranged weapons — without
overhauling the core combat system.

## Features

- 4 firing modes: Default / Precision / Short Burst / Suppression
- Affects accuracy, aim time, cooldown, and burst count
- Breaks the vanilla burst limit, allowing ranged weapons to behave like a true “full auto” firearms
- Auto-Selection based on distance (for player and NPCs, optional)
- Supports mechanoids (including those added by other mods)
- Fully configurable, including per-weapon customization

## Notes

1. Burst shot bonuses do not apply to single-shot or one-time-use weapons
2. Some weapons may not benefit from burst shot bonuses, such as the "Incinerator" from the Anomaly DLC
3. Only pawns or mechanoids equipped with a ranged primary weapon can use firing modes

## Compatibility

This mod is implemented with minimal invasiveness. It uses Harmony patches to inject logic only at the following methods: Verb.WarmupComplete (caches
burst count), Verb.BurstShotCount (modifies burst count), Verb.VerbTick (clears burst cache), and Verb.TryStartCastOn (entry point for automatic mode
switching). Effects on accuracy, aim time, cooldown, etc., are applied via runtime StatPart injection, without overriding vanilla StatWorker, allowing
natural coexistence with other mods that modify similar stats. Theoretically, it is compatible with most mods, including but not limited to weapon
mods, race mods, mechanoid mods, and combat overhaul mods such as 'Vanilla Combat Reloaded', 'Yayo's Combat 3', or 'RunAndGun'.

## FAQ

**Q: CE support?**  
A: Only ensures no errors, but using both is ***NOT*** recommended.

**Q: Safe to add/remove mid-save?**  
A: Yes (removal is theoretically safe).
