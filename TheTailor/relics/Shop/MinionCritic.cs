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
using HarmonyLib;
using MinionLib.Utilities;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Extensions;

namespace TheTailor.Relics.Shop
{
    [Pool(typeof(TheTailorRelicPool))]
    public class MinionCritic : CustomRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Shop;
        public override string PackedIconPath => "res://TheTailor/images/relics/minionCritic.png";
        protected override string PackedIconOutlinePath => "res://TheTailor/images/relics/minionCriticOutline.png";
        protected override string BigIconPath => "res://TheTailor/images/relics/minionCriticBig.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

        public override decimal ModifyHandDrawLate(Player player, decimal count)
        {
            if (player != Owner)
            {
                return count;
            }
            return count + DynamicVars.Cards.IntValue;
        }

        public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
        {
            if (player == Owner)
            {
                List<MonsterModel> randomCreatures = new();

                PetsOrderAccessor accessor = new PetsOrderAccessor(player);
                if (accessor != null && accessor.Pets != null && accessor.Pets.Count > 0)
                {
                    foreach (Creature creature in accessor.Pets)
                    {
                        if (creature.Monster is TailorMinion)
                        {
                            randomCreatures.Add(creature.Monster);
                        }
                    }
                }

                if (randomCreatures.Count > 1)
                {
                    randomCreatures.StableShuffle(player.PlayerRng.Transformations);
                }
                else if (randomCreatures.Count <= 0)
                {
                    return;
                }

                int minionIndex = accessor.Pets.IndexOf(randomCreatures[0].Creature);

                accessor.Pets[minionIndex].RemoveAllPowersInternalExcept();

                await CreatureCmd.Kill(accessor.Pets[minionIndex], true);
                _ = MinionAnimCmd.Rearrange(duration: 0.5f);
                accessor.SetManualRearranged();
                PetOrderSnapshotManager.TakeSnapshot(player);
            }
        }
    }
}