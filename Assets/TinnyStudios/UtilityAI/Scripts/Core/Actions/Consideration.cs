using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// A scriptable object with a method GetScore returning a value between 0-1, this score is used by <see cref="UtilityPlanner"/> to determine if an action should be considered or not.
    /// Think of this as a question "How Hungry am I?" 1 = Hungry, 0 = Not Hungry.
    /// </summary>
    public abstract class Consideration : ScriptableObject
    {
        [TextArea]
        public string Description;

        public AnimationCurve ResponseCurve = AnimationCurve.Linear(0,0,1,1);

        /// <summary>
        /// Return between 0 and 1.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public abstract float GetScore(Agent agent, IUtilityAction action);

        /// <summary>
        /// Used in Editor view to get a simulated score. Helpful for understanding what these curve responses actually output.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual float GetSimulatedScore(float value)
        {
            return ResponseCurve.Evaluate(Mathf.Clamp01(value));
        }
    }
}