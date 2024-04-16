using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.Onboarding
{
    public class WalkAwayAction : UtilityAction
    {
        public override EActionStatus Perform(Agent agent)
        {
            Debug.Log("Agent Walk away from person");
            return EActionStatus.Completed;
        }
    }
}