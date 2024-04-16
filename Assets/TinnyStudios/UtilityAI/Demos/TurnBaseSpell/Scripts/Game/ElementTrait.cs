namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// Determines the pros and cons of an element.
    /// </summary>
    public class ElementTrait
    {
        public EElementType Type;
        public EElementType WeakTo;
        public EElementType StrongTo;

        public ElementTrait(EElementType type, EElementType weakTo, EElementType strongTo)
        {
            Type = type;
            WeakTo = weakTo;
            StrongTo = strongTo;
        }

        public float GetEffectiveness(EElementType type)
        {
            if (StrongTo == type)
                return 1;

            if (WeakTo == type || type == Type)
                return 0;

            return 0.5f;
        }
    }
}