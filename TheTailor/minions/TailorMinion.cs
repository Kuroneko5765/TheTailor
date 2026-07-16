using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MinionLib.Minion;
using MinionLib.Powers;
using TheTailor.Powers;
using BaseLib.Extensions;
using TheTailor.BaseLibAdapters;

namespace TheTailor.Minions
{
    public abstract class TailorMinion : CustomMinionModel
    {
        public override string? HurtSfx => "res://TheTailor/audio/clothHit1.ogg";
        public override string CustomDeathSfx => "res://TheTailor/audio/clothRip1.ogg";
        public override string DeathSfx => "res://TheTailor/audio/clothRip1.ogg";
        public override bool HasDeathSfx => true;
        public override float DeathAnimLengthOverride => 0.8f;
    }
}