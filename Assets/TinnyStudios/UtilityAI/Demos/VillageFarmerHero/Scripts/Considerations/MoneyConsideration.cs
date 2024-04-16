using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/Money")]
    public class MoneyConsideration : Consideration
    {
        [Tooltip("The factor determines the result of the score to the amount of money. i.e Factor of 1 is 1:1 ratio. Where a factor of 10, means 1 gold = 0.1 score")]
        public float Factor = 1;

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var exampleContext = agent.GetContext<ExampleDataContext>();
            return ResponseCurve.Evaluate(Mathf.Clamp01(exampleContext.Inventory.Money/ Factor));
        }
    }
}