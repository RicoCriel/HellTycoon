using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.Onboarding
{
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/Onboarding/ShouldSave")]
    public class ShouldSaveConsideration : Consideration
    {
        public bool Inverted;

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<SaveFriendDataContext>();
            if(!Inverted)
                return context.IsFriend ? 1 : 0;
            else
                return context.IsFriend ? 0 : 1;
        }
    }
}