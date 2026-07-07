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
using TheTailor.Minions;
using MegaCrit.Sts2.Core.HoverTips;
using TheTailor.Cards.Uncommon;

namespace TheTailor.Powers
{
    public sealed class DisarmingSlapStrengthPower : CustomTemporaryPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/disarmingSlapStrengthSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/disarmingSlapStrength.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/disarmingSlapStrength.png";
        public override AbstractModel OriginModel => ModelDb.Card<DisarmingSlap>();
        public override PowerModel InternallyAppliedPower => ModelDb.Power<StrengthPower>();
        protected override Func<PlayerChoiceContext, Creature, decimal, Creature?, CardModel?, bool, Task> ApplyPowerFunc => PowerCmd.Apply<StrengthPower>;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
        protected override bool InvertInternalPowerAmount => true;
    }
}