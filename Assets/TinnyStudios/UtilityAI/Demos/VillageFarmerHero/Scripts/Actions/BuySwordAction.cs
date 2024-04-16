namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Buys the sword. In this case, sets Inventory.HasWeapon to true.
    /// </summary>
    public class BuySwordAction : UtilityAction
    {
        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }

        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            var context = agent.GetContext<ExampleDataContext>();
            context.Inventory.HasWeapon = true;
        }
    }
}