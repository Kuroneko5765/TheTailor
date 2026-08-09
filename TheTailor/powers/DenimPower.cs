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
    /// <summary>
    ///     Applies temp. strength to the owner when hit
    ///     Also applies temp. strength on death because AfterDamageReceived doesn't work properly with killing blows on minions
    /// </summary>
    public sealed class DenimPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/denimSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/denim.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/denim.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

        public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (dealer != null && Owner == target && result.UnblockedDamage > 0)
            {
                Flash();
                await PowerCmd.Apply<DenimStrengthPower>(choiceContext, Owner.PetOwner.Creature, 2, Owner, null);
            }
        }

        public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
        {
            if (Owner == creature && !wasRemovalPrevented)
            {
                Flash();
                await PowerCmd.Apply<DenimStrengthPower>(choiceContext, Owner.PetOwner.Creature, 2, Owner, null);
            }
        }
    }
}