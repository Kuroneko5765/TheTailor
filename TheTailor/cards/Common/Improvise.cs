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
using TheTailor.Character;
using MinionLib.Utilities;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Immutable;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Improvise() : CustomCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/improviseBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/improviseBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/improviseBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            IEnumerable<CardModel> enumerable = PileType.Draw.GetPile(Owner).Cards.Where(cm => StitchCmd.CanBeStitched(cm) == true).TakeRandom(1, Owner.RunState.Rng.CombatCardSelection);
            IEnumerable<CardModel> enumerable2 = PileType.Discard.GetPile(Owner).Cards.Where(cm => StitchCmd.CanBeStitched(cm) == true).TakeRandom(1, Owner.RunState.Rng.CombatCardSelection);

            if (enumerable.Any() && enumerable2.Any())
            {
                CardModel cm1 = enumerable.First();
                CardModel cm2 = enumerable2.First();

                await StitchCmd.StitchCards(cm1, cm2);

                if (IsUpgraded)
                {
                    if (cm1.IsUpgradable) { CardCmd.Upgrade(cm1); }
                    if (cm2.IsUpgradable) { CardCmd.Upgrade(cm2); }
                }

                IReadOnlyList<CardModel> list = enumerable.Concat(enumerable2).ToImmutableArray();

                CardCmd.Preview(list, 0.75f);
                await Cmd.Wait(0.75f);
            }
        }
    }
}