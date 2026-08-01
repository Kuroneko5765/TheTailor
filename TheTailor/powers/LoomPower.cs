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
            if (dealer == null || target.Side != CombatSide.Enemy || !props.IsPoweredAttack())
            {
                return amount;
            }

            return amount + amount * (Amount / 100m) * TailorMinionCmd.GetMinionCount<MinionLinen>(Owner.Player);
        }
    }
}