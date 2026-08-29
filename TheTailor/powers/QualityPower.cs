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
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheTailor.Powers
{
    public sealed class QualityPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/qualitySmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/quality.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/quality.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature == Owner && cardPlay != null && cardPlay.Card.IsUpgraded)
            {
                Flash();
                for (int i = 0; i < Amount; i++)
                {
                    CardPile pile = PileType.Hand.GetPile(Owner.Player);
                    CardModel cardModel = Owner.Player.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(cm => cm.MaxUpgradeLevel > cm.CurrentUpgradeLevel && cm.Type != CardType.Status && cm.Type != CardType.Curse));
                    if (cardModel != null)
                    {
                        CardCmd.Upgrade(cardModel);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}