using System.Linq;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// When performed, cuts a tree.
    /// Earns Money and Wood immediately.
    /// </summary>
    public class WorkAction : UtilityAction
    {
        public TreeObjectManager TreeObjectManager;

        private TreeObject _targetTree;

        /// <summary>
        /// This shows a way of dynamically adding a consideration and binding / injecting any required dependencies.
        /// </summary>
        public override void InitializeDynamicConsideration()
        {
            base.InitializeDynamicConsideration();

            var hasWorkConsideration = ScriptableObject.CreateInstance<HasWorkConsideration>();
            hasWorkConsideration.name = "has-work";
            hasWorkConsideration.Bind(TreeObjectManager);

            AddConsideration(hasWorkConsideration);
        }

        public override EActionStatus Perform(Agent agent)
        {
            return PerformByDuration(agent);
        }

        protected override void OnPerformByDurationCompleted(Agent agent)
        {
            if (_targetTree == null)
                return;

            var data = Agent.GetContext<ExampleDataContext>();
            data.Inventory.Wood += 1;
            data.Inventory.Money += 0.3f;

            _targetTree.OnChopped();
        }

        public override void OnMove(MoveSystemBase moveSystem)
        {
            if (_targetTree != null)
            {
                MoveData.DestinationTransform = _targetTree.transform;

                moveSystem.SetDestination(MoveData.DestinationTransform);
                moveSystem.SetProperties(MoveData);
            }
            else
            {
                // When the tree is destroyed, let's abort the plan.
                Agent.AbortPlan();
            }
        }

        public override void OnMoveStarted(MoveSystemBase moveSystem)
        {
            _targetTree = TreeObjectManager.Objects.FirstOrDefault(x => !x.Taken);
            if(_targetTree != null)
                _targetTree.Taken = true;
        }
    }
}