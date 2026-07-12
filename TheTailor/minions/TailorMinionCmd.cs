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
using MegaCrit.Sts2.Core.Models.Powers;
using MinionLib.Commands;
using MinionLib.Minion;
using MinionLib.Utilities;
using TheTailor.Cards.Token;
using TheTailor.Powers;

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

                if (await ReplaceMinion<T>(playerChoiceContext, owner, replaceIndex))
                {
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
        ///     Replaces a minion in their current position. Set convert to true to retain its max HP for the new minion
        /// </summary>
        public static async Task<bool> ReplaceMinion<T>(PlayerChoiceContext playerChoiceContext, Player owner, int replaceIndex, bool convert = false) where T : MinionModel
        {
            PetsOrderAccessor accessor = new PetsOrderAccessor(owner);
            if (accessor != null && accessor.Pets != null)
            {
                int oldMinionMaxHp = accessor.Pets[replaceIndex].MaxHp;

                await CreatureCmd.Kill(accessor.Pets[replaceIndex], true);
                var newMinion = await MinionCmd.AddMinion<T>(playerChoiceContext, owner, new MinionSummonOptions(Position: MinionPosition.Front));
                accessor.Pets.Remove(newMinion);
                accessor.Pets.Insert(replaceIndex, newMinion);
                _ = MinionAnimCmd.Rearrange(duration: 0.5f);
                accessor.SetManualRearranged();
                PetOrderSnapshotManager.TakeSnapshot(owner);

                if (convert)
                {
                    await CreatureCmd.SetMaxHp(newMinion, oldMinionMaxHp);
                    await CreatureCmd.SetCurrentHp(newMinion, oldMinionMaxHp);
                }

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

        public static async Task TriggerMinionAbility(PlayerChoiceContext choiceContext, Player player, int minionIndex)
        {
            PetsOrderAccessor accessor = new PetsOrderAccessor(player);
            if (accessor != null && accessor.Pets != null && accessor.Pets.Count > 0 && accessor.Pets[minionIndex] != null)
            {
                if (accessor.Pets[minionIndex].Monster is MinionLinen)
                {
                    Creature creature = player.RunState.Rng.CombatTargets.NextItem(player.Creature.CombatState?.HittableEnemies ?? Array.Empty<Creature>());
                    if (creature != null)
                    {
                        await PowerCmd.Apply<VulnerablePower>(choiceContext, creature, 2, accessor.Pets[0], null);
                        await Cmd.Wait(0.2f);
                    }
                }
                else if (accessor.Pets[minionIndex].Monster is MinionCotton)
                {
                    await CreatureCmd.Heal(player.Creature, accessor.Pets[minionIndex].GetPowerAmount<CottonPower>());
                    await Cmd.Wait(0.2f);
                }
                else if (accessor.Pets[minionIndex].Monster is MinionDenim)
                {
                    await PowerCmd.Apply<DenimStrengthPower>(choiceContext, player.Creature, 2m, accessor.Pets[minionIndex], null);
                    await Cmd.Wait(0.2f);
                }
                else if (accessor.Pets[minionIndex].Monster is MinionWool)
                {
                    await PowerCmd.Apply<WoolWeakPower>(choiceContext, player.Creature, 1m, accessor.Pets[minionIndex], null);
                    await Cmd.Wait(0.2f);
                }
                else if (accessor.Pets[minionIndex].Monster is MinionSilk)
                {
                    await PowerCmd.Apply<SilkDexterityPower>(choiceContext, player.Creature, 2m, accessor.Pets[minionIndex], null);
                    await Cmd.Wait(0.2f);
                }
            }
        }
    }
}