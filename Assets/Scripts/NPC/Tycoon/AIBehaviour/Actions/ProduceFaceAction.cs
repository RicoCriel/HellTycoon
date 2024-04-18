using Economy;
using System.Collections;
using System.Collections.Generic;
using TinnyStudios.AIUtility;
using UnityEngine;

namespace Tycoons
{
    public class ProduceFaceAction : UtilityAction
    {
        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }

        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            var context = agent.GetContext<TycoonDataContext>();

            if (context != null)
            {
                int bodyInt = Random.Range(0, 1);
                int hornInt = Random.Range(0, 1);
                int wingInt = Random.Range(0, 1);
                int tailInt = Random.Range(0, 1);
                int eyeInt = Random.Range(2, 4);

                var demon = new DemonStatsInt(bodyInt, hornInt, wingInt, tailInt, eyeInt);
                context.EconomyManager.SellDemon(demon, context.TycoonType);

                context.CurrentProduction = StatType.Face;
            }
        }
    }
}
