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
using MegaCrit.Sts2.Core.Rooms;
using BaseLib.Common.Rewards;
using MegaCrit.Sts2.Core.Combat;

namespace TheTailor.Powers
{
    public sealed class InspirationPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/inspirationSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/inspiration.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/inspiration.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Cards.Token.Patch>(), HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (!(power is InspirationPower))
            {
                return Task.CompletedTask;
            }
            if (power.Owner != Owner)
            {
                return Task.CompletedTask;
            }
            IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
            foreach (CardModel item in enumerable)
            {
                TryAddReplays(item, (int)amount);
            }
            return Task.CompletedTask;
        }

        public override Task AfterCardEnteredCombat(CardModel card)
        {
            if (card.IsClone)
            {
                return Task.CompletedTask;
            }
            TryAddReplays(card, Amount);
            return Task.CompletedTask;
        }

        public override Task AfterRemoved(Creature oldOwner)
        {
            IEnumerable<CardModel> enumerable = oldOwner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
            foreach (CardModel item in enumerable)
            {
                if (item is Cards.Token.Patch patch)
                {
                    patch.BaseReplayCount -= Amount;
                }
            }
            return Task.CompletedTask;
        }

        private bool TryAddReplays(CardModel card, int amount)
        {
            if (card.Owner != Owner.Player)
            {
                return false;
            }
            if (!(card is Cards.Token.Patch patch))
            {
                return false;
            }
            patch.BaseReplayCount += amount;
            return true;
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Player)
            {
                await PowerCmd.Remove(this);
            }
        }
    }
}