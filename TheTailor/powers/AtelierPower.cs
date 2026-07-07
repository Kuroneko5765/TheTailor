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

namespace TheTailor.Powers
{
    public sealed class AtelierPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/atelierSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/atelier.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/atelier.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override Task AfterCombatEnd(CombatRoom room)
        {
            for (int i = 0; i < Amount; i++)
            {
                room.AddExtraReward(Owner.Player, new RandomCardUpgradeReward(Owner.Player));
            }
            return Task.CompletedTask;
        }
    }
}