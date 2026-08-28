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
using MegaCrit.Sts2.Core.Entities.Cards;
using BaseLib.Extensions;
using TheTailor.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.CardSelection;
using TheTailor.Extensions;
using MegaCrit.Sts2.Core.Combat;

namespace TheTailor.Powers
{
    public sealed class DiscerningEyePower : CustomPowerModel
    {
        private class Data
        {
            public int playsThisTurn;
        }
        protected override object InitInternalData()
        {
            return new Data();
        }
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/discerningEyeSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/discerningEye.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/discerningEye.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Player == Owner.Player)
            {
                Data data = GetInternalData<Data>();
                if (data.playsThisTurn < Amount && cardPlay.Card.Type == CardType.Skill)
                {
                    data.playsThisTurn++;
                    CardModel? cardModel = await CardSelectCmd.FromHandForUpgrade(choiceContext, Owner.Player, this);
                    if (cardModel != null)
                    {
                        CardCmd.Upgrade(cardModel);
                    }
                }
            }
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (participants.Contains(Owner))
            {
                Data data = GetInternalData<Data>();
                data.playsThisTurn = 0;
            }
        }
    }
}