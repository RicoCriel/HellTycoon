using System.Diagnostics;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// An implementation of <see cref="TimeWatcher"/> as a time counter using Stopwatch.
    /// This is used to track how long as action has performed for.
    /// </summary>
    public class StopwatchWatcher : TimeWatcher
    {
        public Stopwatch Stopwatch = new Stopwatch();
        public override double GetTotalSeconds()
        {
            return Stopwatch.Elapsed.TotalSeconds;
        }

        public override void Restart()
        {
            Stopwatch.Restart();
        }

        public override void Stop()
        {
            Stopwatch.Stop();
        }
    }
}