namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// When performed, sets Sleepiness to 0.
    /// </summary>
    public class SleepAction : UtilityAction
    {
        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }
        
        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            var exampleDataContext = agent.GetContext<ExampleDataContext>();
            exampleDataContext.Stats.Sleepiness = 0;
        }
    }
}