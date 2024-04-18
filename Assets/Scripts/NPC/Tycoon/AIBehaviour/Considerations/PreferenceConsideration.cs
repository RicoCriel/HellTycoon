using Economy;
using System.Collections;
using System.Collections.Generic;
using TinnyStudios.AIUtility;
using UnityEngine;

namespace Tycoons
{
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Considerations/Preference")]
    public class PreferenceConsideration : Consideration
    {
        [SerializeField] private StatType _statType;

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<TycoonDataContext>();
            if (context != null && context.Preference == _statType)
            {
                return ResponseCurve.Evaluate(1f);
            }

            return ResponseCurve.Evaluate(0.1f);
        }
    }
}

