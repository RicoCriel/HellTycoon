using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Considers if the agent has a weapon already or not.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/has-weapon")]
    public class HasWeaponConsideration : Consideration
    {
        // If you have a sword and there is a monster, slay it.
        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<ExampleDataContext>();
            return context.Inventory.HasWeapon ? ResponseCurve.Evaluate(1) : ResponseCurve.Evaluate(0);
        }
    }
}