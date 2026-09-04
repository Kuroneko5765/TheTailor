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

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class GetCreative() : CustomCardModel(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/getCreativeBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/getCreativeBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/getCreativeBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new DynamicVar("Triggers", 1)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
                
            int random = Owner.RunState.Rng.CombatCardGeneration.NextInt(5);

            switch (random)
            {
                case 0:
                    await TailorMinionCmd.AddOrReplaceMinion<MinionLinen>(choiceContext, Owner, true, withTriggers: DynamicVars["Triggers"].IntValue);
                    break;
                case 1:
                    await TailorMinionCmd.AddOrReplaceMinion<MinionDenim>(choiceContext, Owner, true, withTriggers: DynamicVars["Triggers"].IntValue);
                    break;
                case 2:
                    await TailorMinionCmd.AddOrReplaceMinion<MinionSilk>(choiceContext, Owner, true, withTriggers: DynamicVars["Triggers"].IntValue);
                    break;
                case 3:
                    await TailorMinionCmd.AddOrReplaceMinion<MinionBurlap>(choiceContext, Owner, true, withTriggers: DynamicVars["Triggers"].IntValue);
                    break;
                case 4:
                    await TailorMinionCmd.AddOrReplaceMinion<MinionWool>(choiceContext, Owner, true, withTriggers: DynamicVars["Triggers"].IntValue);
                    break;
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4);
            DynamicVars["Triggers"].UpgradeValueBy(1);
        }
    }
}