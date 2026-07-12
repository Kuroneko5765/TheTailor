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
using MegaCrit.Sts2.Core.HoverTips;

namespace TheTailor.Powers
{
    public sealed class WoolWeakPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/woolWeakenSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/woolWeaken.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/woolWeaken.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.HasPower<WoolWeakPower>() && cardPlay.Card.Type == CardType.Attack)
            {
                List<Creature> targets;
                if (cardPlay.Card.TargetType != TargetType.AllEnemies)
                {
                    targets = new List<Creature>() { cardPlay.Target };
                }
                else
                {
                    targets = cardPlay.Card.CombatState.HittableEnemies.ToList();
                }
                await PowerCmd.Apply<WeakPower>(choiceContext, targets, 1m, cardPlay.Card.Owner.Creature, cardPlay.Card);
                await PowerCmd.Decrement(this);
            }
        }
    }
}