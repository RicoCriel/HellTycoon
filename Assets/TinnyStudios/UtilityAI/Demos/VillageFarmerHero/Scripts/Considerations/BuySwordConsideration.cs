using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Consider if there is a monster then you would want to buy a sword.
    /// This is a very concrete implementation, you could reuse <see cref="HasMonsterConsideration"/>
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/buy-sword")]
    public class BuySwordConsideration : Consideration
    {
        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<ExampleDataContext>();
            var monsterCount = context.MonsterManager.Count;

            return ResponseCurve.Evaluate(monsterCount >= 1 ? 1 : 0);
        }
    }
}