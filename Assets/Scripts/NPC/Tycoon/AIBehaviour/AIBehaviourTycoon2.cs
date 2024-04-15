using System.Collections.Generic;
namespace Economy
{
    public class AIBehaviourTycoon2 : AIBehaviourBase
    {
        public override List<DemonStatsInt> SellBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            
            
            
            List<DemonStatsInt> demons = new List<DemonStatsInt>();

            demons.Add(new DemonStatsInt(2));
            demons.Add(new DemonStatsInt(2));

            return demons;

            return base.SellBehaviour(economyManager, market, soulManager, tycoon);
        }

        public override float BuyBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            return base.BuyBehaviour(economyManager, market, soulManager, tycoon);
        }

        public override float AutoCostBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            return base.AutoCostBehaviour(economyManager, market, soulManager, tycoon);
        }
    }
}
