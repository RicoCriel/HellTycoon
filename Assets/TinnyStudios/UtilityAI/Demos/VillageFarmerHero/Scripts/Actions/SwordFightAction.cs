namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Attacks and Destroy a monster immediately when performed.
    /// </summary>
    public class SwordFightAction : UtilityAction
    {
        public MonsterManager MonsterManager => Agent.GetContext<ExampleDataContext>().MonsterManager;

        private Monster _monsterTarget;

        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }

        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            if (_monsterTarget != null)
                _monsterTarget.Die();
        }

        public override void OnMove(MoveSystemBase moveSystem)
        {
            if (_monsterTarget != null)
            {
                MoveData.DestinationTransform = _monsterTarget.transform;

                moveSystem.SetDestination(MoveData.DestinationTransform);
                moveSystem.SetProperties(MoveData);
            }
            else
            {
                // When the monster is destroyed, let's abort the plan.
                Agent.AbortPlan();
            }
        }

        public override void OnMoveStarted(MoveSystemBase moveSystem)
        {
            // Decide how you want to pick the monster here. 
            // i.e get first will probably find the same monster, 2v1. 
            // You can filter to one that isn't being targeted etc. 
            _monsterTarget = MonsterManager.GetFirstOrDefault();
            base.OnMoveStarted(moveSystem);
        }
    }
}