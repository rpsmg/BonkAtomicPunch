# BonkAtomicPunch

Adds **Bonk! Atomic Punch** (`bonkcan`), a soda can holding 250 ml of the cherry-red liquid **Bonk!** (`bonk`). One use chugs the whole can. A rare find in food crates and at all traders (can value 3 + liquid 12 per 250 ml = trade value 15).

Per can (everything scales with the amount drunk, so Bonk! poured into another container still works):
- Energized +16:40 (same as an energy drink), thirst +5, radiation +12 (3.6 gy), adrenaline +40, happiness +6.
- **Bonked!** for 8 s (moodle, chipped only): injuries are frozen (no wounds, bleeding, pain, broken/dislocated/lost limbs, blood loss or death; healing still works), you can't attack or throw, and you can jump once more in mid-air (no other mods needed). Drinking again while bonked resets the timer.
- When bonked ends: 50% slowdown, fading over 5 s.

Sprites: `bonkcan.png` (4x9, vanilla soda can size) and the `bonked.png` moodle icon (20x20), both painted from `bonkcan.art` / `bonked.art`. Why injuries are frozen rather than attacks blocked: [docs/0001-bonked-freezes-injuries.md](docs/0001-bonked-freezes-injuries.md).

- **Plugin ID:** `boofi.cu.bonkatomicpunch`
- **Requires:** BepInEx 5.4.23.5, CUCoreLib (ships with Casualties Together)
- **Multiplayer:** registered content + own-body. The item and liquid need the mod on the host and every client (hosts: enable "enforce mod list"); being bonked runs on the drinker's machine.

## Install
Copy `BonkAtomicPunch.dll` to `BepInEx\plugins\BonkAtomicPunch\`.

## Build
Requires .NET SDK and the game's managed DLLs, referenced straight from the install (no copies).
```
dotnet build -p:GameDir="C:\Program Files (x86)\Steam\steamapps\common\Casualties Unknown\"
```
The build copies the DLL to `BepInEx\plugins\BonkAtomicPunch\`; pass `-p:Deploy=false` to compile only.
