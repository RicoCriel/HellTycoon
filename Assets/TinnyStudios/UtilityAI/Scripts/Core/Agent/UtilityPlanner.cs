using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// Finds the best action based on a list of considerations from <see cref="Agent"/>
    /// </summary>
    public class UtilityPlanner : MonoBehaviour
    {
        [Header("Plan Settings")]
        [Tooltip("Set by the Agent or external classes. This tells Execute Plan to pause. Enabling this will trigger a new plan automatically.")]
        public bool Paused;
        public EUpdateType UpdateType;

        [Tooltip("The duration an agent stay idle for.")]
        public float IdleDuration;

        public bool StartPlanOnAwake = true;
        public float StartPlanDelay;

        [Header("Action Settings")]
        [Tooltip("Enabling this will find actions automatically, this is useful to turn off if you want to control it instead.")]
        public bool FindActionsOnAwake = true;

        [Tooltip("Enabling this will include inactive actions in your search.")]
        public bool IncludeInActiveActions = false;

        [Tooltip("By default this is disabled. Enabling it will allow actions with 0 score to execute, meaning even if it's not a meaningful action.")]
        public bool ExecuteEvenZeroScoreActions;

        [Header("Log Settings")]
        [Tooltip("Show more detailed logs of the planner. Useful for debugging completion or failure of actions.")]
        public bool ShowLogs;

        public EAgentLogType LogTypes;

        /// <summary>
        /// Returns the best action from agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public virtual IUtilityAction GetBestAction(Agent agent)
        {
            var actions = agent.Actions;

            if (actions.Count == 0)
            {
                agent.Events.OnActionSkipped?.Invoke();
                agent.Log("Skipped", "No actions found.", EAgentLogType.Skip);
                return null;
            }

            var score = 0.0f;
            IUtilityAction bestAction = null;
            for (var i = 0; i < actions.Count; i++)
            {
                if (!actions[i].IsAvailable())
                    continue;

                if (!(GetActionScore(actions[i], agent) > score)) 
                    continue;

                bestAction = actions[i];
                score = actions[i].Score;
            }

            return bestAction;
        }

        /// <summary>
        /// Scores an individual action.
        /// </summary>
        /// <returns></returns>
        public virtual float GetActionScore(IUtilityAction action, Agent agent)
        {
            var score = 1.0f;
            for (var i = 0; i < action.Considerations.Count; i++)
            {
                var consideration = action.Considerations[i];
                action.OnConsiderationCheck(consideration);
                var considerationScore = consideration.GetScore(agent, action);
                score *= considerationScore;

                // If the score reaches 0, no point continuing the evaluation.
                if (score == 0)
                    break;
            }

            action.SetScore(GetRescaleScore(action, score) * action.Weight);
            return action.Score;
        }

        /// <summary>
        /// Get rescale score using Dave's Averaging Scheme.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        protected virtual float GetRescaleScore(IUtilityAction action, float score)
        {
            var modificationFactor = 1 - (1 / action.Considerations.Count);
            var makeupValue = (1 - score) * modificationFactor;
            var rescaledScore = score + (makeupValue * score);

            if (rescaledScore <= 0.004f)
                rescaledScore = 0;

            return rescaledScore;
        }
    }

    /// <summary>
    /// Decides how the Agent update is handled. 
    /// </summary>
    public enum EUpdateType
    {
        /// <summary> Unity's update loop </summary>
        Update,
        /// <summary> Unity's Fixed Update loop</summary>
        FixedUpdate,
        /// <summary> Unity's Late Update loop </summary>
        LateUpdate,
        /// <summary> Basically this means you will handle it yourself via script. </summary>
        Script
    }
}
