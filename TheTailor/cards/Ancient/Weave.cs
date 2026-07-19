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
using TheTailor.Character;

namespace TheTailor.Cards.Ancient
{
    [Pool(typeof(TheTailorCardPool))]
    public class Weave() : CustomCardModel(2, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/weaveBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/weaveBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/weaveBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new DynamicVar("Delicate", 2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            IEnumerable<CardModel> cardModel = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefsExtensions.StitchSelectionPrompt, 2), context: choiceContext, player: base.Owner, filter: StitchCmd.CanBeStitched, source: this);
            if (cardModel != null && cardModel.Count() == 2)
            {
                await StitchCmd.StitchCards(cardModel.ElementAt(0), cardModel.ElementAt(1));
            }
        }

        protected override CardLocation GetResultLocationForCardPlay()
        {
            CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
            if (resultLocationForCardPlay.pileType == PileType.Discard)
            {
                resultLocationForCardPlay.pileType = PileType.Draw;
                resultLocationForCardPlay.position = CardPilePosition.Top;
            }
            return resultLocationForCardPlay;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5);
            DynamicVars["Delicate"].UpgradeValueBy(1);
        }
    }
}