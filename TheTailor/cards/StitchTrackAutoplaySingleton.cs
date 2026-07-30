using BaseLib;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
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
using TheTailor;
using TheTailor.Cards.Token;

/// <summary>
///     Used for tracking which enemy is attacking, for a Minion applying debuffs or effects on-death.
/// </summary>
public class StitchTrackAutoplaySingleton() : CustomSingletonModel(HookType.Combat)
{
    public static List<CardModel> BlockedFromAutoplay = new();

    public override async Task BeforeCardAutoPlayed(CardModel card, Creature? target, AutoPlayType type)
    {
        if (type == StitchedAutoPlayType.Stitched)
        {
            BlockedFromAutoplay.Add(card);
        }
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        BlockedFromAutoplay.Remove(cardPlay.Card);
    }
}
