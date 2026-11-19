# Bonked freezes injuries instead of blocking attacks

While a character is bonked (BonkAtomicPunch mod), the mod snapshots their injury values and, every frame, puts back any value that got worse: skin and muscle health, pain, bleeding and shrapnel on each limb, plus blood volume, brain health and internal bleeding on the body. Breaking, dislocating and losing limbs are blocked outright. We chose this over blocking each source of harm because the game has no single damage entry point. About 35 files write limb health directly (enemies, falls, explosions, guns, traps, hazards), so a per-source block would miss some and break when the game adds more. Values that improve are kept, so healing still works while bonked.

## Consequences
- Anything that hurts through a value outside that list (such as infection, temperature or radiation) still works while bonked. That's intended.
- A hit that drops brain health to zero within one frame is undone before the death screen can start, because the death screen fades in over time and doesn't trigger instantly.
