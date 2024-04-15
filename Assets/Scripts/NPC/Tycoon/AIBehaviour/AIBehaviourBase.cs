using System.Collections.Generic;
using Economy;

namespace Tycoons
{
    public abstract class AIBehaviourBase
    {
        public virtual List<DemonStatsInt> SellBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            return new List<DemonStatsInt>();
        }
        public virtual float BuyBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            return 0;
        }
        public virtual float AutoCostBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            return 0;
        }
    }
}
