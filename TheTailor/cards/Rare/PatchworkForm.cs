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
using TheTailor.Cards.Token;
using TheTailor.Character;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class PatchworkForm() : CustomCardModel(9, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/patchworkFormBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/patchworkFormBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/patchworkFormBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch), HoverTipFactory.FromCard<Cards.Token.Patch>()];
        protected override IEnumerable<DynamicVar> CanonicalVars => [];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
            if (IsUpgraded)
            {
                await PowerCmd.Apply<PatchworkFormPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-6);
        }
    }
}