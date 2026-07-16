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
    public sealed class SilkTouchPower : CustomPowerModel
    {
        private class Data
        {
            public int attacksPlayed;

            public int triggerCount;
        }
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/silkTouchSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/silkTouch.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/silkTouch.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
        public override int DisplayAmount => Amount - GetInternalData<Data>().attacksPlayed % Amount;
        protected override object InitInternalData()
        {
            return new Data();
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.Type == CardType.Attack)
            {
                Data data = GetInternalData<Data>();
                data.attacksPlayed++;
                int triggers = data.attacksPlayed / 4 - data.triggerCount;
                if (triggers > 0)
                {
                    Flash();
                    await TailorMinionCmd.AddOrReplaceMinion<MinionSilk>(choiceContext, cardPlay.Card.Owner, true);
                    data.triggerCount += triggers;
                }
                InvokeDisplayAmountChanged();
            }
        }
    }
}