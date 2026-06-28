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
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheTailor.Powers
{
    public sealed class SteadyHandPower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/steadyHandSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/steadyHand.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/steadyHand.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power.Owner == Owner && power is SteadyHandPower && Owner.IsPlayer)
            {
                IEnumerable<CardModel> enumerable = Owner.Player.PlayerCombatState.AllCards.Where((CardModel c) => c.DynamicVars.ContainsKey("Delicate"));
                foreach (CardModel card in enumerable)
                {
                    card.UpdateDynamicVarPreview(CardPreviewMode.Normal, card.Owner.Creature, card.DynamicVars);
                }
            }
        }

        public override async Task AfterCardEnteredCombat(CardModel card)
        {
            if (card.Owner.HasPower<SteadyHandPower>() && card.DynamicVars.ContainsKey("Delicate"))
            {
                card.UpdateDynamicVarPreview(CardPreviewMode.Normal, card.Owner.Creature, card.DynamicVars);
            }
        }
    }

    [HarmonyPatch]
    internal static class DelicateNumberDisplayPatch
    {
        [HarmonyPatch(typeof(DynamicVar), "UpdateCardPreview")]
        internal static async void Postfix(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks, DynamicVar __instance)
        {
            if (__instance.Name == "Delicate" && card.Owner.HasPower<SteadyHandPower>())
            {
                __instance.PreviewValue = __instance.BaseValue + card.Owner.Creature.GetPowerAmount<SteadyHandPower>();
            }
        }
    }
}