using System.Collections.Generic;
using Economy;
using UnityEngine;

namespace Tycoons
{
    public class AIBehaviourTycoon1 : AIBehaviourBase
    {
        public override List<DemonStatsInt> SellBehaviour(EconomyManager economyManager, Tycoon tycoon)
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
        }

        public override float BuyBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            float hoManyToBuy = Random.Range(tycoon.TycoonData.MinBuyAmount, tycoon.TycoonData.MaxBuyAmount);

            return hoManyToBuy;
        }

        public override float AutoCostBehaviour(EconomyManager economyManager, Tycoon tycoon)
        {
            float autoCost = Random.Range(tycoon.TycoonData.MinAutoCostAmount, tycoon.TycoonData.MaxAutoCostAmount);

            return autoCost;
        }
    }

}
