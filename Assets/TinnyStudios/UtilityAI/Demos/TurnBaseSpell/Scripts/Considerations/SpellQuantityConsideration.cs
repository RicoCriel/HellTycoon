using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// Considers if a spell has any uses left. 
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/TurnBasedSpell/Considerations/Spell Quantity")]
    public class SpellQuantityConsideration : Consideration, IDataBind<RuntimeSpellAttackData>
    {
        private RuntimeSpellAttackData _runtimeSpellData;

        /// <summary>
        /// Bind/Inject the Action's Spell Attack Data to this consideration as an action spell data can change at runtime.
        /// </summary>
        /// <param name="runtimeSpellData"></param>
        public void Bind(RuntimeSpellAttackData runtimeSpellData)
        {
            _runtimeSpellData = runtimeSpellData;
        }

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            return _runtimeSpellData.Used >= _runtimeSpellData.SpellAttackData.MaxUse ? 0 : 1;
        }
    }
}