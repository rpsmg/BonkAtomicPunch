using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using UnityEngine;

namespace BonkAtomicPunch;

[BepInPlugin(Guid, "BonkAtomicPunch", Version)]
[BepInDependency("net.cucorelib", BepInDependency.DependencyFlags.HardDependency)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "boofi.cu.bonkatomicpunch";
    public const string Version = "1.0.0";
    public const string CanId = "bonkcan";
    public const string LiquidId = "bonk";

    internal static ManualLogSource Log;
    internal static Sprite CanSprite, MoodleIcon;

    private void Awake()
    {
        Log = Logger;
        CanSprite = AssetLoader.LoadEmbeddedSprite("BonkAtomicPunch.bonkcan.png");
        MoodleIcon = AssetLoader.LoadEmbeddedSprite("BonkAtomicPunch.bonked.png", 33.33f); // vanilla moodle scale
        RegisterLiquid();
        RegisterCan();
        new Harmony(Guid).PatchAll();
        Log.LogInfo($"BonkAtomicPunch {Version} loaded");
    }

    // Everything scales with how much is drunk; numbers are per 250 ml can.
    private static void RegisterLiquid()
    {
        LiquidRegistry.Register(LiquidId, new CustomLiquidInfo
        {
            name = "Bonk!",
            description = "Cherry-flavoured and faintly glowing. Makes you untouchable and lets you jump once more in mid-air for a few seconds.",
            color = new Color32(200, 24, 48, 255), // cherry
            valuePerLiter = 48f, // 12 per 250 ml can
            injectionSickness = 0.6f,
            qualities = new List<CraftingQuality> { new CraftingQuality("water", 0.5f) },
            onDrink = (ml, body) =>
            {
                float cans = ml / 250f;
                body.Drink(5f * cans); // thirst = hydration; Drink keeps the gulp + Together sync
                body.caffeinated += 1000f * cans;   // same as an energy drink
                body.radiationSickness += 12f * cans; // wound view shows radiation x0.3, so 3.6 gy
                body.adrenaline += 40f * cans;
                body.happiness += 6f * cans;
                body.talker.EatGood();
                if (!body.TryGetComponent<Bonked>(out var bonked)) bonked = body.gameObject.AddComponent<Bonked>();
                bonked.Begin(8f * cans);
            },
        });
    }

    private static void RegisterCan()
    {
        // Name and description must be on the info at Register time; CUCoreLib turns them into locale entries.
        ItemRegistry.Register(CanId, new CustomItemInfo
        {
            fullName = "Bonk! Atomic Punch",
            description = "Ingredients: Water, Radiation, Sugar",
            category = "water",
            slotRotation = -45f,
            usable = true,
            usableOnLimb = false,
            destroyAtZeroCondition = false,
            combineable = true,
            weight = 0.75f,
            scaleWeightWithCondition = true,
            capacity = 250f,
            defaultContents = new List<LiquidStack> { new LiquidStack(LiquidId, 250f) },
            useAction = (body, item) => item.GetComponent<WaterContainerItem>().Drink(body, 250f), // chug the whole can
            value = 3, // + 12 from a full can of liquid = trade value 15
            rec = new Recognition(3),
            DropPool = CUCoreLib.Data.DropPool.FoodCrate | CUCoreLib.Data.DropPool.AllTraders,
            SpawnFrequency = 1, // ponytail: lowest pool weight = rare; raise if it never shows up
        }, CanSprite);
    }
}

// Bonked: injuries are frozen (only improvements stick), no attacking, then a slowdown. See docs\adr\0001.
// The component is enabled exactly while bonked.
public class Bonked : MonoBehaviour
{
    public float timeLeft;
    private Body body;
    private float[] skin, muscle, pain, bleed;
    private int[] shrapnel;
    private float blood, brain, internalBleeding;

    public static bool IsBonked(Body body) => body && body.TryGetComponent<Bonked>(out var b) && b.enabled;

    public void Begin(float seconds)
    {
        body = GetComponent<Body>();
        if (!enabled || skin == null) Snapshot();
        enabled = true;
        timeLeft = Mathf.Max(timeLeft, seconds); // drinking again resets to full, never adds
    }

    private void Snapshot()
    {
        var limbs = body.limbs;
        skin = new float[limbs.Length]; muscle = new float[limbs.Length];
        pain = new float[limbs.Length]; bleed = new float[limbs.Length]; shrapnel = new int[limbs.Length];
        for (int i = 0; i < limbs.Length; i++)
        {
            skin[i] = limbs[i].skinHealth; muscle[i] = limbs[i].muscleHealth;
            pain[i] = limbs[i].pain; bleed[i] = limbs[i].bleedAmount; shrapnel[i] = limbs[i].shrapnel;
        }
        blood = body.bloodVolume; brain = body.brainHealth; internalBleeding = body.internalBleeding;
    }

    private void LateUpdate()
    {
        var limbs = body.limbs;
        for (int i = 0; i < limbs.Length; i++)
        {
            var l = limbs[i];
            l.skinHealth = skin[i] = Mathf.Max(skin[i], l.skinHealth);
            l.muscleHealth = muscle[i] = Mathf.Max(muscle[i], l.muscleHealth);
            l.pain = pain[i] = Mathf.Min(pain[i], l.pain);
            l.bleedAmount = bleed[i] = Mathf.Min(bleed[i], l.bleedAmount);
            l.shrapnel = shrapnel[i] = Mathf.Min(shrapnel[i], l.shrapnel);
        }
        body.bloodVolume = blood = Mathf.Max(blood, body.bloodVolume);
        body.brainHealth = brain = Mathf.Max(brain, body.brainHealth);
        body.internalBleeding = internalBleeding = Mathf.Min(internalBleeding, body.internalBleeding);

        timeLeft -= Time.deltaTime;
        if (PlayerCamera.main && PlayerCamera.main.body == body)
            MoodleRegistry.AddMoodle(5, Plugin.MoodleIcon, "Bonked!",
                $"Nothing can hurt you and you can jump once more in mid-air, but you can't attack. {Mathf.Max(timeLeft, 0f):0.0}s left.",
                chippedOnly: true, key: "bonkatomicpunch.bonked");
        if (timeLeft > 0f) return;
        body.temporarySlowdown = Mathf.Max(body.temporarySlowdown, 0.5f); // game fades it out over 5 s
        enabled = false;
    }
}

[HarmonyPatch]
internal static class BonkedPatches
{
    // No breaking, dislocating or losing limbs while bonked.
    [HarmonyPrefix, HarmonyPatch(typeof(Limb), nameof(Limb.BreakBone))]
    private static bool NoBreak(Limb __instance) => !Bonked.IsBonked(__instance.body);

    [HarmonyPrefix, HarmonyPatch(typeof(Limb), nameof(Limb.Dislocate))]
    private static bool NoDislocate(Limb __instance) => !Bonked.IsBonked(__instance.body);

    [HarmonyPrefix, HarmonyPatch(typeof(Limb), nameof(Limb.Dismember))]
    private static bool NoDismember(Limb __instance) => !Bonked.IsBonked(__instance.body);

    // No attacking: blocks the attack button (unarmed, melee, guns, tools) at the input, so Together's attack sync never sees it.
    // ponytail: also blocks LMB-use items like flashlights for those 8 s; split by item if that annoys.
    [HarmonyPrefix, HarmonyPatch(typeof(PlayerCamera), "HandleAttacks")]
    private static bool NoAttack(PlayerCamera __instance) => !Bonked.IsBonked(__instance.body);

    // No throwing: a throw becomes a gentle drop (zero force) instead.
    [HarmonyPrefix, HarmonyPatch(typeof(Body), nameof(Body.ThrowItem))]
    private static void NoThrow(Body __instance, ref float force)
    {
        if (Bonked.IsBonked(__instance)) force = 0f;
    }
}

// One extra jump in mid-air while bonked.
[HarmonyPatch(typeof(Body))]
internal static class BonkedAirJump
{
    // ponytail: one flag, only the local player's body ever air-jumps.
    private static bool airJumpUsed;

    private static readonly AccessTools.FieldRef<Body, float> JumpCooldown = AccessTools.FieldRefAccess<Body, float>("jumpCooldown");
    private static readonly AccessTools.FieldRef<Body, bool> MovingAllowed = AccessTools.FieldRefAccess<Body, bool>("movingAllowed");

    [HarmonyPostfix, HarmonyPatch("HandlePhysics")]
    private static void Recharge(Body __instance)
    {
        if (__instance.grounded || __instance.currentClimbable) airJumpUsed = false;
    }

    // Runs before Together's Jump postfix so it sees the jump and relays it to other players.
    [HarmonyPostfix, HarmonyPatch(nameof(Body.Jump)), HarmonyPriority(Priority.High)]
    private static void AirJump(Body __instance)
    {
        var cam = PlayerCamera.main;
        if (!cam || cam.body != __instance || !Bonked.IsBonked(__instance)) return;
        // Vanilla ground/wall jump succeeded (it clears the input), or nothing pressed this frame.
        if (!cam.wantsJumpInput || !Input.GetKeyDown(KeyBinds.GetBind("jump"))) return;
        if (airJumpUsed || !__instance.standing || __instance.forceWalk || !MovingAllowed(__instance)
            || JumpCooldown(__instance) > 0f || __instance.currentClimbable) return;

        airJumpUsed = true;
        JumpCooldown(__instance) = 0.25f;
        var rb = __instance.rb;
        rb.velocity = new Vector2(rb.velocity.x, __instance.actualJumpSpeed);
        __instance.stamina -= 1f + __instance.overEncumberance;
        __instance.TryExertSound(0.6f, 0.45f);
        __instance.CreateCloudBig(__instance.transform.position);
        cam.wantsJumpInput = false;
    }
}
