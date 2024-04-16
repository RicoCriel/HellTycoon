using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Consider if there are any trees remaining.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Examples/FarmerHero/Considerations/HasWork")]
    public class HasWorkConsideration : Consideration
    {
        // Unused as this is just here to show a way to inject TreeObjectManager.
        private TreeObjectManager _treeObjectManager;

        /// <summary>
        /// Shows an example of how to inject <see cref="TreeObjectManager"/>
        /// </summary>
        /// <param name="treeObjectManager"></param>
        public void Bind(TreeObjectManager treeObjectManager)
        {
            _treeObjectManager = treeObjectManager;
        }

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var exampleContext = agent.GetContext<ExampleDataContext>();

            // Important to note how we got the TreeObjectManager here. 
            // If your game is simple, I would suggest just having the TreeManager inside of ExampleDataContext or a method to 'GetTreeManager'. 
            // If you like to have a very clear separation, see the Bind method above and see the pattern in WorkAction. 

            return ResponseCurve.Evaluate(exampleContext.TreeObjectManager.Count/10f);
        }
    }
}