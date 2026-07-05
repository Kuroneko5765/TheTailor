using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTailor;
using TheTailor.Extensions;
using TheTailor.Cards;
using TheTailor.Minions;
using MinionLib.Commands;
using MinionLib.Minion;
using HarmonyLib;
using TheTailor.Powers;
using BaseLib.Extensions;

namespace TheTailor.Cards
{
    [HarmonyPatch]
    internal static class DelicatePatch
    {
        [HarmonyPatch(typeof(AbstractModel), "AfterCardPlayed")]
        internal static async void Postfix(PlayerChoiceContext choiceContext, CardPlay cardPlay, AbstractModel __instance)
        {
            if (__instance is CardModel && cardPlay.Card == __instance && cardPlay.Card.DynamicVars.ContainsKey("Delicate"))
            {
                int extraDelicacies = 0;
                extraDelicacies += cardPlay.Card.Owner.Creature.GetPowerAmount<SteadyHandPower>();

                cardPlay.Card.DynamicVars["Delicate"].BaseValue -= 1 + cardPlay.Card.BaseReplayCount;
                if (cardPlay.Card.DynamicVars["Delicate"].IntValue + extraDelicacies <= 1)
                {
                    cardPlay.Card.AddKeyword(CardKeyword.Exhaust);
                }
            }
        }
    }
}