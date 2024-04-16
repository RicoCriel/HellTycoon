namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// Provides an interface to inject a dependency. See <see cref="SpellElementConsideration"/> Bind method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataBind<in T>
    {
        void Bind(T dependency);
    }
}