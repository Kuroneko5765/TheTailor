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

namespace TheTailor.Powers
{
    public sealed class LightTouchPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/lightTouchSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/lightTouch.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/lightTouch.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Delicate)];
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
        {
            if (cardSource.Owner.Creature != Owner)
            {
                return 0m;
            }
            if (!cardSource.DynamicVars.ContainsKey("Delicate"))
            {
                return 0m;
            }
            if (cardSource.DynamicVars["Delicate"].BaseValue <= 0)
            {
                return 0m;
            }

            return Amount;
        }

        public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource, CardPlay cardPlay)
        {
            if (cardSource.Owner.Creature != Owner)
            {
                return 0m;
            }
            if (!cardSource.DynamicVars.ContainsKey("Delicate"))
            {
                return 0m;
            }
            if (cardSource.DynamicVars["Delicate"].BaseValue <= 0)
            {
                return 0m;
            }

            return Amount;
        }
    }
}