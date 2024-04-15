using System.Collections.Generic;
namespace Economy
{
    public abstract class AIBehaviourBase
    {
        public virtual List<DemonStatsInt> SellBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            return new List<DemonStatsInt>();
        }
        public virtual float BuyBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            return 0;
        }
        public virtual float AutoCostBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            return 0;
        }
    }
}
