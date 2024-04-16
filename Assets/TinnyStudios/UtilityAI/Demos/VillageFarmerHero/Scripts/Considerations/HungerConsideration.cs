using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Consider if you a hungry.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/Hunger")]
    public class HungerConsideration : Consideration
    {
        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var exampleContext = agent.GetContext<ExampleDataContext>();
            return ResponseCurve.Evaluate(Mathf.Clamp01(exampleContext.Stats.Hunger));
        }
    }
}