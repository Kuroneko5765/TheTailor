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
    public class SturdyStrike() : CustomCardModel(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike, CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/sturdyStrikeBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/sturdyStrikeBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/sturdyStrikeBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Delicate", 2), new DamageVar(9, ValueProp.Move)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.DenimMinion), HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(TheTailor.Keywords.Delicate)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await TailorMinionCmd.AddOrReplaceMinion<MinionDenim>(choiceContext, Owner, true);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5);
            DynamicVars["Delicate"].UpgradeValueBy(1);
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}