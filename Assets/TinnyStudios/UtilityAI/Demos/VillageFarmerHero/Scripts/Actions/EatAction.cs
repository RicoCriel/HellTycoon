namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Sets hunger to 0 when performed.
    /// </summary>
    public class EatAction : UtilityAction
    {
        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }

        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            var data = agent.GetContext<ExampleDataContext>();
            data.Stats.Hunger = 0;
        }
    }
}