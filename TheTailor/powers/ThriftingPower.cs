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
using MegaCrit.Sts2.Core.Combat;

namespace TheTailor.Powers
{
    public sealed class ThriftingPower : CustomPowerModel
    {
        private class Data
        {
            public int etherealCount; // Handles Ethereal draw-after-turn
            public int exhaustsThisTurn; // Handles overall exhaust
        }
        protected override object InitInternalData()
        {
            return new Data();
        }
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/thriftingSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/thrifting.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/thrifting.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if (card.Owner.Creature == Owner)
            {
                Data data = GetInternalData<Data>();
                if (data.exhaustsThisTurn < Amount)
                {
                    Flash();
                    if (causedByEthereal)
                    {
                        GetInternalData<Data>().etherealCount++;
                    }
                    else
                    {
                        await CardPileCmd.Draw(choiceContext, 1m, Owner.Player);
                    }
                    data.exhaustsThisTurn++;
                }
            }
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (participants.Contains(Owner))
            {
                Data data = GetInternalData<Data>();
                await CardPileCmd.Draw(choiceContext, data.etherealCount, Owner.Player);
                data.etherealCount = 0;
                data.exhaustsThisTurn = 0;
            }
        }
    }
}