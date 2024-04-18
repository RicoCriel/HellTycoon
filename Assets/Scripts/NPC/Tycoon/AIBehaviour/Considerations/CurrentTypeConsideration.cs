using System.Collections;
using System.Collections.Generic;
using Economy;
using TinnyStudios.AIUtility;
using UnityEngine;

namespace Tycoons
{
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Considerations/CurrentProduction")]
    public class CurrentTypeConsideration : Consideration
    {
        [SerializeField] private StatType _statType;

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<TycoonDataContext>();
            if (context != null && context.CurrentProduction == _statType)
            {
                return ResponseCurve.Evaluate(1);
            }

            return ResponseCurve.Evaluate(0f);
        }
    }
}

