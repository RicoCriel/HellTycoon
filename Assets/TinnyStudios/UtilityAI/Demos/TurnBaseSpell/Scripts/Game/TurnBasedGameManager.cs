using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// An example game manager for a turn base agent.
    /// This is not production code so please use it just as an example on a starting point on understanding how to execute and set up a turn based Agent.
    /// </summary>
    public class TurnBasedGameManager : MonoBehaviour
    {
        public Agent Agent;

        public TextMeshProUGUI DecisionLabel;

        public int TurnCount = 0;

        /// <summary>
        /// A table for weakness and strength for each element types.
        /// </summary>
        public static Dictionary<EElementType, ElementTrait> ElementTraitTable = new Dictionary<EElementType, ElementTrait>()
        {
            {EElementType.Water, new ElementTrait(EElementType.Water, EElementType.None, EElementType.Fire)},
            {EElementType.Normal, new ElementTrait(EElementType.Normal,EElementType.None, EElementType.None)},
            {EElementType.Fire, new ElementTrait(EElementType.Fire,EElementType.Water, EElementType.None)},
        };

        /// <summary>
        /// Choose attack will find a plan and check if its valid and then execute the plan immediately.
        /// This is called by a UnityEvent on the Attack! button.
        /// </summary>
        public void ChooseAttack()
        {
            var success = Agent.FindPlan();
            if (!success) 
                return;

            if (Agent.IsCurrentActionValid())
            {
                // We need to move the state to planning to immediately execute the plan.
                // If your game has an update loop, i.e your character moves to a spot and then attack, you may want to use this on an update loop instead.
                Agent.GoToState(EAgentState.Performing);
                Agent.ExecutePlan();
            }
            else
            {
                DecisionLabel.text = $"No more moves available.";
            }
        }

        /// <summary>
        /// When a spell action performs, it calls this method which then evaluate all the result of the attack.
        /// </summary>
        public void Choose(SpellAttackAction spellAttackAction)
        {
            var gameContext = Agent.GetContext<TurnBaseSpellGameContext>();

            if (gameContext.Foe.IsDead)
            {
                Debug.Log($"Foe is already dead.");
                return;
            }

            TurnCount++;
            DecisionLabel.text = $"({TurnCount}) - Familar Agent Used {spellAttackAction.Name} Action!";
            var attackTrait = ElementTraitTable[spellAttackAction.RuntimeData.SpellAttackData.ElementType];
            var effectiveness = attackTrait.GetEffectiveness(gameContext.Foe.ElementType);
            var damage = 1 + (effectiveness);
            gameContext.Foe.TakeDamage(damage);

            Debug.Log($"Deals {damage:F2} damage!");

            if (gameContext.Foe.IsDead)
                gameContext.Foe.gameObject.SetActive(false);
        }
    }
}