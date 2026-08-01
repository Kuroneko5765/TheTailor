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
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheTailor.Powers
{
    public sealed class HighFashionPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/highFashionSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/highFashion.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/highFashion.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(Keywords.Premium)];
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == player.Creature)
            {
                Flash();
                IEnumerable<CardModel> enumerable = Owner.Player.PlayerCombatState.AllCards.Where(cm => cm.MaxUpgradeLevel > 1 && cm.IsUpgradable && cm.Type != CardType.Status && cm.Type != CardType.Curse);
                foreach (CardModel cm in enumerable)
                {
                    for (int i = 0; i < Amount; i++)
                    {
                        CardCmd.Upgrade(cm);
                    }
                }
            }
        }
    }
}