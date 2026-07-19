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
using TheTailor.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MinionLib.Commands;
using MinionLib.Minion;
using TheTailor.Character;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class ProductionLine() : CustomCardModel(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/productionLineBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/productionLineBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/productionLineBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.LinenMinion)];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Unpowered)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
            await PowerCmd.Apply<ProductionLinePower>(choiceContext, Owner.Creature, DynamicVars.Damage.IntValue, Owner.Creature, this, false);

            if (IsUpgraded)
            {
                await TailorMinionCmd.AddOrReplaceMinion<MinionLinen>(choiceContext, Owner, true);
            }
        }
    }
}