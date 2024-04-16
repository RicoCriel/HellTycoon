namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// This action gives a sword to another agent.
    /// </summary>
    public class GiveSwordAction : UtilityAction
    {
        public Agent HeroAgent;

        public ExampleDataContext HeroAgentContext => HeroAgent.GetContext<ExampleDataContext>();

        /// <summary>
        /// In this case, we override IsAvailable because we don't want to give a weapon to the agent if he already has one.
        /// Other alternative is we actually deduct the number of swords this agent carries and use it in Consideration.
        /// But this is simple and illustrate a usage for IsAvailable that is easy to read.
        /// </summary>
        /// <returns></returns>
        public override bool IsAvailable()
        {
            if (HeroAgentContext.Inventory.HasWeapon)
                return false;

            return base.IsAvailable();
        }

        public override EActionStatus Perform(Agent agent)
        {
            HeroAgent.GetContext<ExampleDataContext>().Inventory.HasWeapon = true;
            return EActionStatus.Completed;
        }

        public override void OnMove(MoveSystemBase moveSystem)
        {
            MoveData.DestinationTransform = HeroAgent.transform;

            // If you're no longer needed, just abort. This prevents you from forever following the hero agent around.
            if(!IsAvailable())
                Agent.AbortPlan();

            base.OnMove(moveSystem);
        }
    }
}