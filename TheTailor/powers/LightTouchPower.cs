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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Combat;

namespace TheTailor.Powers
{
    public sealed class LightTouchPower : CustomPowerModel
    {
        private class Data
        {
            public bool thisTurn;
        }
        protected override object InitInternalData()
        {
            return new Data();
        }

        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/lightTouchSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/lightTouch.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/lightTouch.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(Keywords.Delicate)];
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.DynamicVars.ContainsKey("Delicate"))
            {
                Data internalData = GetInternalData<Data>();
                if (internalData.thisTurn == false)
                {
                    Flash();
                    await PlayerCmd.GainEnergy(Amount, Owner.Player);
                    internalData.thisTurn = true;
                }
            }
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (participants.Contains(Owner))
            {
                Data data = GetInternalData<Data>();
                data.thisTurn = false;
            }
        }
    }
}