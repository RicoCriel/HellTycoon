using System;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The type of logs we can filter by <see cref="UtilityPlanner"/> log types.
    /// </summary>
    [Flags]
    public enum EAgentLogType
    {
        Perform = 1 << 1,
        Skip = 1 << 2,
        Completed = 1 << 3,
        Abort = 1 << 4,
        Transition = 1 << 5,
        Plan = 1 << 6,
    }
}