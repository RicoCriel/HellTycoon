using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// A static logger extension to help filter out what logs to show or hide.
    /// </summary>
    public static class AgentLogger
    {
        public static void Log(this Agent agent, string title, string message, EAgentLogType logType)
        {
            var logTypes = agent.UtilityPlanner.LogTypes;

            if (agent.UtilityPlanner.ShowLogs && logTypes.HasFlag(logType))
                Debug.Log($"[<b>{agent.name}, {logType}: {title} </b>] -- {message}");
        }
    }
}