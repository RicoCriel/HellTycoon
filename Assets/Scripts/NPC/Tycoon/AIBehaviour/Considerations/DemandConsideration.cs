using System.Collections;
using System.Collections.Generic;
using Economy;
using TinnyStudios.AIUtility;
using Tycoons;
using UnityEngine;

namespace Tycoons
{
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Considerations/Demand")]
    public class DemandConsideration : Consideration
    {
        [SerializeField] private StatType _statType;
        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<TycoonDataContext>();
            if (context != null && context.EconomyManager != null)
            {
                var demand = context.EconomyManager.GetDemand(_statType);
                return ResponseCurve.Evaluate(demand);
            }

            return 0.1f;
        }
    }
}

