using System.Collections.Generic;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Minion;
using MinionLib.Powers.Patches;
using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Entities.Players;
using TheTailor.Minions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheTailor.Powers
{
    public sealed class LoomPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/loomSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/loom.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/loom.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(Keywords.LinenMinion), HoverTipFactory.FromPower<VulnerablePower>()];

        public decimal ModifyVulnerableMultiplier(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (target == Owner || !Owner.IsPlayer)
            {
                return amount;
            }
            if (!props.IsPoweredAttack())
            {
                return amount;
            }
            return amount + ((decimal)base.Amount / 100m) * (decimal)TailorMinionCmd.GetMinionCount<MinionLinen>(Owner.Player);
        }
    }

    [HarmonyPatch]
    internal static class LoomVulnPatch
    {
        [HarmonyPatch(typeof(VulnerablePower), "ModifyDamageMultiplicative")]
        internal static decimal Postfix(decimal __result, Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay, VulnerablePower __instance)
        {
            if (target != __instance.Owner)
            {
                return __result;
            }
            if (!props.IsPoweredAttack())
            {
                return __result;
            }
            if (dealer != null)
            {
                LoomPower? power = dealer.GetPower<LoomPower>();
                if (power != null)
                {
                    __result = power.ModifyVulnerableMultiplier(target, __result, props, dealer, cardSource);
                }
            }

            return __result;
        }
    }
}