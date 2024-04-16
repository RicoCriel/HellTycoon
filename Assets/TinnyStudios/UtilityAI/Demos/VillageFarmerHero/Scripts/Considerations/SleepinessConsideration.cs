using System;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Considers how sleepy an agent is.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/Sleepiness")]
    public class SleepinessConsideration : Consideration
    {
        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var exampleContext = agent.GetContext<ExampleDataContext>();
            return ResponseCurve.Evaluate(Mathf.Clamp01(exampleContext.Stats.Sleepiness));
        }
    }
}