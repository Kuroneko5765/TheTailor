using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MinionLib.Commands;
using TheTailor;
using TheTailor.Minions;
using MinionLib.Minion;
using TheTailor.Character;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Commands;

namespace TheTailor.Relics.Common
{
    [Pool(typeof(TheTailorRelicPool))]
    public class ToughTrousers : CustomRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Common;
        public override string PackedIconPath => "res://TheTailor/images/relics/toughTrousers.png";
        protected override string PackedIconOutlinePath => "res://TheTailor/images/relics/toughTrousersOutline.png";
        protected override string BigIconPath => "res://TheTailor/images/relics/toughTrousersBig.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(1)];

        public override async Task AfterCreatureAddedToCombat(Creature creature)
        {
            if (creature.Monster is TailorMinion && creature.IsPet && creature.PetOwner == Owner)
            {
                Flash();
                await CreatureCmd.GainMaxHp(creature, DynamicVars.Heal.IntValue);
            }
        }
    }
}