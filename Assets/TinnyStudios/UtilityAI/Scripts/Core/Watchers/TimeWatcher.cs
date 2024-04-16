namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// This is used to track how long as action has performed for.
    /// </summary>
    public abstract class TimeWatcher
    {
        public abstract double GetTotalSeconds();
        public abstract void Restart();
        public abstract void Stop();
    }
}