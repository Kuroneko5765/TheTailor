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

namespace TheTailor.Powers
{
    public sealed class PartingGiftPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/partingGiftSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/partingGift.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/partingGift.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
        {
            if (creature.Monster is TailorMinion && creature.IsPet && creature.PetOwner == Owner.Player && TailorMinionCmd.GetMinionCount<TailorMinion>(Owner.Player) <= 0)
            {
                Flash();
                await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
            }
        }
    }
}