using System.Collections.Generic;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// This class just shows an example of how you can add actions at runtime.
    /// </summary>
    public class AddActionExample : MonoBehaviour
    {
        public Agent Agent;

        [Header("Assign an action to add.")]
        public UtilityAction Action;

        [Header("Assign a list of actions to replace")]
        public List<UtilityAction> ActionsToReplace;

        public List<IUtilityAction> Actions = new List<IUtilityAction>();

        private void Start()
        {
            foreach (var utilityAction in ActionsToReplace)
                Actions.Add(utilityAction);

            if(Action != null)
                Agent.AddAction(Action);

            if (Actions.Count > 0)
            {
                Agent.AbortPlan();
                Agent.ReplaceActions(Actions);
            }
        }
    }
}