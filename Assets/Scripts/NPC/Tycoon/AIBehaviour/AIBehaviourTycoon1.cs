using System.Collections.Generic;
using UnityEngine;
namespace Economy
{
    public class AIBehaviourTycoon1 : AIBehaviourBase
    {
        public override List<DemonStatsInt> SellBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            List<DemonStatsInt> demons = new List<DemonStatsInt>();
            int howmanyToSell = Random.Range(tycoon.TycoonData.MinSellAmount, tycoon.TycoonData.MaxSellAmount);

            for (int i = 0; i < howmanyToSell; i++)
            {
                int bodyInt = Random.Range(1, 3);
                int hornInt = Random.Range(1, 4);
                int wingInt = Random.Range(1, 2);
                int tailInt = Random.Range(1, 1);
                int eyeInt = Random.Range(1, 1);

                demons.Add(new DemonStatsInt(bodyInt, hornInt, wingInt, tailInt, eyeInt));
            }
            
            return demons;

            return base.SellBehaviour(economyManager, market, soulManager, tycoon);
        }

        public override float BuyBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            float hoManyToBuy = Random.Range(tycoon.TycoonData.MinBuyAmount, tycoon.TycoonData.MaxBuyAmount);

            return hoManyToBuy;
            
            return base.BuyBehaviour(economyManager, market, soulManager, tycoon);
        }

        public override float AutoCostBehaviour(EconomyManager economyManager, Market market, SoulManager soulManager, Tycoon tycoon)
        {
            float autoCost = Random.Range(tycoon.TycoonData.MinAutoCostAmount, tycoon.TycoonData.MaxAutoCostAmount);

            return autoCost;
            
            return base.AutoCostBehaviour(economyManager, market, soulManager, tycoon);
        }
    }

}
