# BonkAtomicPunch

Adds **Bonk! Atomic Punch** (`bonkcan`), a soda can holding 250 ml of the cherry-red liquid **Bonk!** (`bonk`). One use chugs the whole can. A rare find in food crates and at all traders (can value 3 + liquid 12 per 250 ml = trade value 15).

Per can (everything scales with the amount drunk, so Bonk! poured into another container still works):
- Energized +16:40 (same as an energy drink), thirst -5 (a little thirstier), radiation +12 (3.6 gy), adrenaline +40, happiness +6.
- **Bonked!** for 8 s (moodle, chipped only): injuries are frozen (no wounds, bleeding, pain, broken/dislocated/lost limbs, blood loss or death; healing still works), you can't attack or throw, and you can jump once more in mid-air (no other mods needed; with DoubleJump's Scout shoes on, it's still one extra jump, not two). Drinking again while bonked resets the timer.
- When bonked ends: 50% slowdown, fading over 5 s.

Sprites: `bonkcan.png` (4x9, vanilla soda can size) and the `bonked.png` moodle icon (20x20), both painted from `bonkcan.art` / `bonked.art` with the cu-mod skill's `paint.py`. Why injuries are frozen rather than attacks blocked: `docs\adr\0001-bonked-freezes-injuries.md` in the project root.

- **Plugin ID:** `boofi.cu.bonkatomicpunch`
- **Requires:** BepInEx 5.4.23.5, CUCoreLib (ships with Casualties Together)
- **Multiplayer:** registered content + own-body. The item and liquid need the mod on the host and every client (hosts: enable "enforce mod list"); being bonked runs on the drinker's machine.

## Install
Copy `BonkAtomicPunch.dll` to `BepInEx\plugins\BonkAtomicPunch\`.
