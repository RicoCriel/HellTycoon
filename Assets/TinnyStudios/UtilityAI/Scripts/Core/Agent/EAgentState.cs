namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The state the Agent is currently in. 
    /// </summary>
    public enum EAgentState
    {
        /// <summary> The agent is not doing anything here. This is a good place for idle animation</summary>
        Idle,
        /// <summary> The agent has begun planning and may find a good action. </summary>
        Plan,
        /// <summary> The agent found a plan/action that needs to move. </summary>
        Moving,
        /// <summary> The agent is now performing the action, this can be over time or immediately depending on <see cref="UtilityAction"/> </summary>
        Performing,
        /// <summary> The agent wants to abort the current action. This will take effect the next frame. </summary>
        Aborting,
    }
}