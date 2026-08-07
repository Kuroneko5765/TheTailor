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
using MegaCrit.Sts2.Core.Entities.Cards;
using BaseLib.Extensions;
using TheTailor.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.CardSelection;
using TheTailor.Extensions;

namespace TheTailor.Powers
{
    public sealed class PatchworkFormPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/patchworkFormSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/patchworkForm.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/patchworkForm.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == player.Creature)
            {
                Flash();
                foreach (CardModel card in await Cards.Token.Patch.CreateInHand(player, Amount, CombatState))
                {
                    card.AddModifier<StitchBlockModifier>();

                    IEnumerable<CardModel> cardModel = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(TheTailor.Extensions.CardSelectorPrefsExtensions.StitchPatchSelectionPrompt, 1), context: choiceContext, player: Owner.Player, filter: StitchCmd.CanBeStitchedExcluding, source: this);
                    if (cardModel != null && cardModel.Count() == 1)
                    {
                        await StitchCmd.StitchCards(card, cardModel.ElementAt(0));
                    }

                    StitchBlockModifier? stitchBlockModifier = card.GetModifier<StitchBlockModifier>();
                    if (stitchBlockModifier != null)
                    {
                        CardModifier.RemoveModifier(card, stitchBlockModifier);
                    }
                }
            }
        }
    }
}