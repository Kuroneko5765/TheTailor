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

namespace TheTailor.Powers
{
    public sealed class UnravelPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/unravelSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/unravel.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/unravel.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.GetModifier<StitchCardModifier>() != null)
            {
                Flash();
                await CreatureCmd.Damage(choiceContext, Owner.CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
            }
        }
    }
}