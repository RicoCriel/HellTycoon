namespace TinnyStudios.AIUtility.Core.Properties
{
    /// <summary>
    /// A concrete accessor for string literials to be used with <see cref="PropertySet{TProperty,TValue}"/>
    /// The class is partial as a way for you to extend without having to modify the original class. Extend by making another public partial class PropertyConst{}
    /// </summary>
    public partial class PropertyConst
    {
        public const string GOLD = "Inventory.Gold";
    }
}