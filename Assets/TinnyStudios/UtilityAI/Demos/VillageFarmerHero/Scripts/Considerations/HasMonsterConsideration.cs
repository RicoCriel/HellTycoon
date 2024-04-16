using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Considers if there are any monsters around.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/has-monster")]
    public class HasMonsterConsideration : Consideration
    {
        // If you have a sword and there is a monster, slay it.
        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<ExampleDataContext>();
            var monsterCount = context.MonsterManager.Count;
            return monsterCount >= 1 ? ResponseCurve.Evaluate(1) : ResponseCurve.Evaluate(0);
        }
    }
}