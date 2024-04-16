using System.Collections.Generic;
using Economy;

namespace Tycoons
{
    public class AIBehaviourTycoon3 : AIBehaviourBase
    {
        public override List<DemonStatsInt> SellBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            List<DemonStatsInt> demons = new List<DemonStatsInt>();

            demons.Add(new DemonStatsInt(2));
            demons.Add(new DemonStatsInt(2));

            return demons;
        }

        public override float BuyBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            return base.BuyBehaviour(economyManager, tycoon);
        }

        public override float AutoCostBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            return base.AutoCostBehaviour(economyManager, tycoon);
        }
    }
}
