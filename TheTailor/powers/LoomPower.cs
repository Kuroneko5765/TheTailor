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

namespace TheTailor.Powers
{
    public sealed class LoomPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/loomSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/loom.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/loom.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromKeyword(Keywords.LinenMinion)];

        public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == player.Creature)
            {
                int minions = TailorMinionCmd.GetMinionCount<MinionLinen>(player);
                if (minions > 0)
                {
                    var targets = CombatState.HittableEnemies.ToList();
                    await PowerCmd.Apply<WeakPower>(choiceContext, targets, minions * Amount, Owner, null);
                }
            }
        }
    }
}