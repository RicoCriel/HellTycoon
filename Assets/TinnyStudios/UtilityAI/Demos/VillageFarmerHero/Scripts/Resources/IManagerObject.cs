namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// An interface to define it as a ManagerObject to be used by <see cref="ObjectManager{T,TManager}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IManagerObject<in T>
    {
        void Bind(T manager);
    }
}