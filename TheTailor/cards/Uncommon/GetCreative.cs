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
using HarmonyLib;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class GetCreative() : CustomCardModel(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/getCreativeBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/getCreativeBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/getCreativeBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Delicate", -999)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [IsUpgraded ? HoverTipFactory.FromKeyword(TheTailor.Keywords.Delicate) : HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            for (int i = 0; i < 2; i++)
            {
                int random = Owner.RunState.Rng.CombatCardGeneration.NextInt(6);

                switch (random)
                {
                    case 0:
                        await TailorMinionCmd.AddOrReplaceMinion<MinionLeather>(choiceContext, Owner, true);
                        break;
                    case 1:
                        await TailorMinionCmd.AddOrReplaceMinion<MinionLinen>(choiceContext, Owner, true);
                        break;
                    case 2:
                        await TailorMinionCmd.AddOrReplaceMinion<MinionDenim>(choiceContext, Owner, true);
                        break;
                    case 3:
                        await TailorMinionCmd.AddOrReplaceMinion<MinionSilk>(choiceContext, Owner, true);
                        break;
                    case 4:
                        await TailorMinionCmd.AddOrReplaceMinion<MinionCotton>(choiceContext, Owner, true);
                        break;
                    case 5:
                        await TailorMinionCmd.AddOrReplaceMinion<MinionWool>(choiceContext, Owner, true);
                        break;
                }
            }
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
            DynamicVars["Delicate"].BaseValue = 2;
        }
    }
}