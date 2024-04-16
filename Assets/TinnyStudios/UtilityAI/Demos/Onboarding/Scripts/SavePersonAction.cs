using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.Onboarding
{
    public class SavePersonAction : UtilityAction
    {
        public override EActionStatus Perform(Agent agent)
        {
            Debug.Log("Agent Saved Person");
            return EActionStatus.Completed;
        }
    }
}