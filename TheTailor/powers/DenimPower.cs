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

namespace TheTailor.Powers
{
    public sealed class DenimPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/denimSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/denim.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/denim.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

        /*
        public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (target.HasPower<DenimPower>() && dealer != null && result.UnblockedDamage > 0 && target.IsPet)
            {
                await PowerCmd.Apply<DenimStrengthPower>(choiceContext, target.PetOwner.Creature, Amount, Owner, null);
            }
        }
        */

        public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (target.HasPower<LinenPower>() && amount > 0 && dealer != null && target.IsPet)
            {
                await PowerCmd.Apply<DenimStrengthPower>(choiceContext, target.PetOwner.Creature, Amount, Owner, null);
            }
        }
    }
}