using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// A scriptable object containing the element type of the attack.
    /// Just an example, no strength or abilities included.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/TurnBasedSpell/Spell Attack")]
    public class SpellAttackData : ScriptableObject
    {
        public EElementType ElementType;

        /// <summary>
        /// Number of times you can use this move as a base.
        /// </summary>
        public int MaxUse = 5;
    }
}