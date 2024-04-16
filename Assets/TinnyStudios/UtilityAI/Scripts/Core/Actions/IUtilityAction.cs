using System.Collections.Generic;
using TinnyStudios.AIUtility.Core.Properties;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The base implementation for Utility Action to be used by the planner.
    /// </summary>
    public interface IUtilityAction
    {
        /// <summary>
        /// Sets up the action, useful for any initialization.
        /// </summary>
        /// <param name="agent"></param>
        void Setup(Agent agent);

        /// <summary>
        /// Set up has been completed so we won't do it a second time.
        /// </summary>
        bool Initialized { get; set; }

        /// <summary>
        /// A list of unity events for ease of use.
        /// </summary>
        UtilityAction.ActionEvents Events { get; }

        /// <summary>
        /// The last evaluated Score of the Action. The higher score will get chosen by the Planner.
        /// </summary>
        float Score { get; }
        
        /// <summary>
        /// The list of considerations. Think of a consideration as a question of how much you want to do something.
        /// i.e a consideration for EatAction is how hungry are you? If you are very hungry, it will return 1.
        /// The list of consideration total score average is the Action's Score.
        /// </summary>
        List<Consideration> Considerations { get; }

        /// <summary>
        /// The action score is multiplied by the weight. This determines its max score output and can be use for classifying priorities.
        /// </summary>
        float Weight { get; }

        /// <summary>
        /// The Action Move Data stores useful data to pass to the Move System.
        /// </summary>
        ActionMoveData MoveData { get; }

        /// <summary>
        /// The name of the action, mostly used for debugging purposes.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The last known state of the action. Useful for debugging purposes.
        /// </summary>
        EActionStatus State { get; }

        /// <summary>
        /// This is checked before the action is scored. This is useful for simply telling an action to not occur via code.
        /// It's basically a coded approach to considerations.
        /// </summary>
        /// <returns></returns>
        bool IsAvailable();

        /// <summary>
        /// This is called just before a consideration is checked. This gives time for us to bind any neccessary data.
        /// </summary>
        void OnConsiderationCheck(Consideration consideration);

        void SetScore(float getRescaleScore);

        /// <summary>
        /// Is called when the Agent just started moving to its target to perform the action.
        /// </summary>
        /// <param name="moveSystem"></param>
        void OnMoveStarted(MoveSystemBase moveSystem);

        /// <summary>
        /// Called Once Per Frame.
        /// Assign destination to Destination Transform by default.
        /// Override this method to change the target destination, i.e a location without a transform.
        /// You can use this to override Move System or simply modify its speed.
        /// Transform is kept default for convenience sake
        /// </summary>
        void OnMove(MoveSystemBase moveSystem);

        /// <summary>
        /// Is called when the Agent reached the destination. This is not called if MoveData Required field is not enabled.
        /// </summary>
        void OnReachedDestination();

        /// <summary>
        /// Sets the ActionStatus of the Agent. This does not transition and is used more for tracking.
        /// </summary>
        /// <param name="state"></param>
        void SetState(EActionStatus state);

        /// <summary>
        /// Is called when the action can be executed. i.e within range.
        /// Returning EActionStatus.Completed will end the action and the agent will find a new plan.
        /// Returning EActionStatus.Running will continue.
        /// Returning EActionStatus.Failed will allow to Agent to know to Abort the action.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        EActionStatus Perform(Agent agent);

        /// <summary>
        /// Stores the time elapsed of the performing action.
        /// </summary>
        TimeWatcher TimeWatch { get; }

        /// <summary>
        /// To show more info, only used for editor purposes.
        /// </summary>
        bool FoldOutEnabled { get; set; }

        PropertySetRuntime PropertySet { get; }
    }
}