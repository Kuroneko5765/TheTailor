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
using TheTailor.Character;
using TheTailor.Cards.Token;

namespace TheTailor.Cards.Multiplayer.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class InfinitePatches() : CustomCardModel(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/infinitePatchesBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/infinitePatchesBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/infinitePatchesBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Token.Patch>(IsUpgraded)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            IEnumerable<Creature> enumerable = from c in CombatState.GetTeammatesOf(Owner.Creature)
                                               where c != null && c.IsAlive && c.IsPlayer
                                               select c;
            foreach (Creature item in enumerable)
            {
                if (IsUpgraded)
                {
                    await PowerCmd.Apply<InfinitePatchesPlusPower>(choiceContext, item, 1, Owner.Creature, this);
                }
                else
                {
                    await PowerCmd.Apply<InfinitePatchesPower>(choiceContext, item, 1, Owner.Creature, this);
                }
            }
        }
    }
}