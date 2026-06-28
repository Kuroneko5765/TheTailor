using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MinionLib.Minion;
using MinionLib.Powers;
using TheTailor.Powers;

namespace TheTailor.Minions
{
    public sealed class MinionLinen : MinionModel
    {
        public override int MinInitialHp => 6;
        public override int MaxInitialHp => 6;

        protected override string VisualsPath => "res://TheTailor/scenes/minions/minionLinen.tscn";

        public override async Task OnSummon(PlayerChoiceContext choiceContext, Player owner, MinionSummonOptions options)
        {
            await PowerCmd.Apply<TailorMadePower>(choiceContext, Creature, 1m, owner.Creature, options.Source);
            await PowerCmd.Apply<LinenPower>(choiceContext, Creature, 1m, owner.Creature, options.Source);
        }
    }
}