namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// Unlike a TreeObject, Monster wants to be an entity base as well so here we use the IManagerObject interface instead.
    /// This will need to handle the binding process by itself which is just about keeping track of the monster manager..
    /// </summary>
    public class Monster : EntityBase, IManagerObject<MonsterManager>
    {
        private MonsterManager _manager;

        public void Bind(MonsterManager manager)
        {
            _manager = manager;
        }

        public void Die()
        {
            _manager.Remove(this);
            Destroy(gameObject);
        }
    }
}