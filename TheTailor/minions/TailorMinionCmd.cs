using BaseLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MinionLib.Commands;
using MinionLib.Minion;
using MinionLib.Utilities;
using TheTailor.Cards.Token;

namespace TheTailor.Minions
{
    public static class TailorMinionCmd
    {
        public static bool CanMinionBeAdded(Player owner)
        {   
            if (GetMinionCount<TailorMinion>(owner) >= 3)
            {
                return false;
            }
            return true;
        }

        public static int GetMinionCount<T>(Player owner)
        {
            int ret = 0;
            if (owner.Creature.Pets.Count > 0)
            {
                foreach (Creature minion in owner.Creature.Pets)
                {
                    if (minion?.Monster is T)
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        ///     Adds a minion. If too many exist, prompts the player to select one for replacing. Returns true if the minion was added
        /// </summary>
        public static async Task<bool> AddOrReplaceMinion<T>(PlayerChoiceContext playerChoiceContext, Player owner, bool canSkip) where T : MinionModel
        {
            if (!CanMinionBeAdded(owner))
            {
                int replaceIndex = await SelectionPromptFromCurrentMinions(playerChoiceContext, owner, canSkip);

                if (replaceIndex < 0)
                {
                    return false;
                }

                PetsOrderAccessor accessor = new PetsOrderAccessor(owner);
                if (accessor != null && accessor.Pets != null)
                {
                    await CreatureCmd.Kill(accessor.Pets[replaceIndex], true);
                    var newMinion = await MinionCmd.AddMinion<T>(playerChoiceContext, owner, new MinionSummonOptions(Position: MinionPosition.Front));
                    accessor.Pets.Remove(newMinion);
                    accessor.Pets.Insert(replaceIndex, newMinion);
                    _ = MinionAnimCmd.Rearrange(duration: 0.5f);
                    accessor.SetManualRearranged();
                    PetOrderSnapshotManager.TakeSnapshot(owner);
                    return true;
                }
            }
            else
            {
                await MinionCmd.AddMinion<T>(playerChoiceContext, owner, new MinionSummonOptions(Position: MinionPosition.Front));
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Allows the player to select from cards representing their minions, and returns the index of the minion's index in accessor.Pets
        /// </summary>
        public static async Task<int> SelectionPromptFromCurrentMinions(PlayerChoiceContext playerChoiceContext, Player owner, bool canSkip)
        {
            List<CardModel> choices = new();

            PetsOrderAccessor accessor = new PetsOrderAccessor(owner);
            if (accessor != null && accessor.Pets != null)
            {
                foreach (Creature minion in accessor.Pets)
                {
                    CardModel cm = null;
                    switch (minion.Monster)
                    {
                        case MinionLeather:
                            cm = ModelDb.Card<LeatherMinionToken>().ToMutable();
                            cm.Owner = owner;
                            cm.DynamicVars["ChoiceIndex"].BaseValue = accessor.Pets.IndexOf(minion);
                            choices.Add(cm);
                            break;
                        case MinionCotton:
                            cm = ModelDb.Card<CottonMinionToken>().ToMutable();
                            cm.Owner = owner;
                            cm.DynamicVars["ChoiceIndex"].BaseValue = accessor.Pets.IndexOf(minion);
                            choices.Add(cm);
                            break;
                        case MinionDenim:
                            cm = ModelDb.Card<DenimMinionToken>().ToMutable();
                            cm.Owner = owner;
                            cm.DynamicVars["ChoiceIndex"].BaseValue = accessor.Pets.IndexOf(minion);
                            choices.Add(cm);
                            break;
                        case MinionLinen:
                            cm = ModelDb.Card<LinenMinionToken>().ToMutable();
                            cm.Owner = owner;
                            cm.DynamicVars["ChoiceIndex"].BaseValue = accessor.Pets.IndexOf(minion);
                            choices.Add(cm);
                            break;
                        case MinionSilk:
                            cm = ModelDb.Card<SilkMinionToken>().ToMutable();
                            cm.Owner = owner;
                            cm.DynamicVars["ChoiceIndex"].BaseValue = accessor.Pets.IndexOf(minion);
                            choices.Add(cm);
                            break;
                        case MinionWool:
                            cm = ModelDb.Card<WoolMinionToken>().ToMutable();
                            cm.Owner = owner;
                            cm.DynamicVars["ChoiceIndex"].BaseValue = accessor.Pets.IndexOf(minion);
                            choices.Add(cm);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (choices.Count <= 0)
            {
                return -1;
            }
            
            choices.Reverse();
            CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(playerChoiceContext, choices, owner, canSkip);
            
            if (cardModel == null)
            {
                return -1;
            }
            else
            {
                return cardModel.DynamicVars["ChoiceIndex"].IntValue;
            }
        }
    }
}