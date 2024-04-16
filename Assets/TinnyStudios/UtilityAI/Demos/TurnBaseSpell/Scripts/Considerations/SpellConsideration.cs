namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// Considers the effectiveness of the attack and receiver's element.
    /// </summary>
    public class SpellElementConsideration : Consideration, IDataBind<RuntimeSpellAttackData>
    {
        private RuntimeSpellAttackData _runtimeSpellData;

        /// <summary>
        /// Bind/Inject the Action's Spell Attack Data to this consideration as an action spell data can change at runtime.
        /// </summary>
        /// <param name="runtimeSpellData"></param>
        public void Bind(RuntimeSpellAttackData runtimeSpellData)
        {
            _runtimeSpellData = runtimeSpellData;
        }

        public override float GetScore(Agent agent, IUtilityAction action)
        {
            var context = agent.GetContext<TurnBaseSpellGameContext>();
            var foeElementType = context.Foe.ElementType;
            var attackTrait = TurnBasedGameManager.ElementTraitTable[_runtimeSpellData.SpellAttackData.ElementType];
            var effectiveness = attackTrait.GetEffectiveness(foeElementType);
            return effectiveness == 0 ? 0.1f : effectiveness;
        }
    }
}

