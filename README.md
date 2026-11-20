# BonkAtomicPunch

## Description
Adds **Bonk! Atomic Punch** (`bonkcan`), a cherry-red soda that makes you briefly untouchable — at a price. Chug a can before the leap, the gunfight or the fall you probably shouldn't survive.

## Installation instructions
1. Install BepInEx 5.4.23.5 (or newer 5.x) into the game folder.
2. Install Casualties Together (it provides CUCoreLib).
3. Copy `BonkAtomicPunch.dll` to `BepInEx\plugins\BonkAtomicPunch\`.
4. Launch the game. The item is in food crates and at traders.

## Main features
- **Bonk! Atomic Punch** — a 250 ml can, one use chugs the whole can. Rare in food crates and at all traders.
- **Bonk!** — the liquid: energized +8:00, hydration +5, radiation +12 (3.6 gy), adrenaline +40, happiness +6 per can. Everything scales with the amount drunk, so Bonk! poured into another container still works.
- **Bonked!** for 8 s (chipped-only moodle): injuries are frozen — no wounds, bleeding, pain, broken or dislocated limbs, blood loss or death, while healing still works. You can't attack or throw, but you get one extra mid-air jump.
- Drinking another can while bonked resets the timer. When it ends, a 50% slowdown fades over 5 s.

## Requirements
- BepInEx 5.4.23.5 or newer.
- Casualties Together (includes CUCoreLib).
- Multiplayer: the item and liquid need the mod on the host and every client; hosts can enable "enforce mod list". The Bonked effect runs on the drinker's machine. The air jump works without any other mod.

## Shout outs
- Orsoniks for Casualties: Unknown.
- creaturefeaturelarry for Casualties Together and CUCoreLib.
- Valve for Team Fortress 2, where Bonk! Atomic Punch comes from.
- The BepInEx and HarmonyX maintainers.

## Notes
- **Plugin ID:** `sootiee.cu.bonkatomicpunch`
- Sprites: `bonkcan.png` (4x9, vanilla soda can size) and `bonked.png` (20x20 moodle icon), painted from the `.art` files. Why injuries are frozen rather than attacks blocked: [docs/0001-bonked-freezes-injuries.md](docs/0001-bonked-freezes-injuries.md).

## Building from source
Requires the .NET SDK and the game's managed DLLs, referenced straight from the install (no copies):
```
dotnet build -p:GameDir="C:\Program Files (x86)\Steam\steamapps\common\Casualties Unknown\"
```
The build copies the DLL to `BepInEx\plugins\BonkAtomicPunch\`; pass `-p:Deploy=false` to compile only.
