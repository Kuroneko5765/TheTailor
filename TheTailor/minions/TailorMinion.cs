using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MinionLib.Minion;
using MinionLib.Powers;
using TheTailor.Powers;
using BaseLib.Extensions;
using TheTailor.BaseLibAdapters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace TheTailor.Minions
{
    public abstract class TailorMinion : CustomMinionModel
    {
        public override string? HurtSfx => "res://TheTailor/audio/clothHit1.ogg";
        public override string CustomDeathSfx => "res://TheTailor/audio/clothRip1.ogg";
        public override string DeathSfx => "res://TheTailor/audio/clothRip1.ogg";
        public override bool HasDeathSfx => true;
        public override float DeathAnimLengthOverride => 0.8f;
        public override float HpBarSizeReduction => 12f;
    }

    /// <summary>
    ///     Makes Minions consume Buffer
    /// </summary>
    [HarmonyPatch]
    internal static class MinionBufferPatch
    {
        [HarmonyPatch(typeof(BufferPower), "ModifyHpLostAfterOstyLate")]
        internal static decimal Postfix(decimal __result, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, BufferPower __instance)
        {
            if (__instance.Owner.Pets.Contains(target) && target.Monster is TailorMinion)
            {
                return 0m;
            }

            return __result;
        }
    }

    /// <summary>
    ///     Makes Minions consume Intangible
    /// </summary>
    [HarmonyPatch]
    internal static class MinionIntangiblePatch
    {
        [HarmonyPatch(typeof(IntangiblePower), "ModifyHpLostAfterOsty")]
        internal static decimal Postfix(decimal __result, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, BufferPower __instance)
        {
            if (__instance.Owner.Pets.Contains(target) && target.Monster is TailorMinion)
            {
                return 1m;
            }

            return __result;
        }
    }

    /// <summary>
    ///     Enables the selection reticle when a Minion is hovered over to make its interactability obvious
    /// </summary>
    [HarmonyPatch]
    internal static class MinionReticlePatch
    {
        [HarmonyPatch(typeof(NCreature), "OnFocus")]
        internal static void Postfix(NCreature __instance)
        {
            if (__instance.Entity.Monster != null && __instance.Entity.Monster is TailorMinion)
            {
                __instance.ShowSingleSelectReticle();
            }
        }
    }

    /// <summary>
    ///     Makes the death fade vfx unclickable so Minions are easier to re-order during the animation
    /// </summary>
    [HarmonyPatch]
    internal static class MinionDeathClickablePatch
    {
        [HarmonyPatch(typeof(NCreature), "DisableInteractionForDeath")]
        internal static void Postfix(NCreature __instance)
        {
            if (__instance.Entity.Monster != null && __instance.Entity.Monster is TailorMinion)
            {
                foreach (Control control in __instance.Visuals.GetChildrenRecursive<Control>())
                {
                    if (control.HasFocus())
                    {
                        ActiveScreenContext.Instance.FocusOnDefaultControl();
                    }
                    control.FocusMode = Control.FocusModeEnum.None;
                    control.MouseFilter = Control.MouseFilterEnum.Ignore;
                }
            }
        }
    }
}