using System;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// The action that choose a spell from <see cref="RuntimeData"/>
    /// Each move will increase the Used Count.
    /// </summary>
    public class SpellAttackAction : UtilityAction
    {
        public RuntimeSpellAttackData RuntimeData;

        /// <summary>
        /// Dyamically Create a consideration for this spell type and inject spell attack data through.
        /// You can also manually make a spell consideration but this is handy if you need to dynamically inject, see OnConsiderationCheck method.
        /// </summary>
        public override void InitializeDynamicConsideration()
        {
            base.InitializeDynamicConsideration();

            var spellElementConsideration = ScriptableObject.CreateInstance<SpellElementConsideration>();
            spellElementConsideration.name = $"spell-element-{RuntimeData.SpellAttackData.ElementType}";
            spellElementConsideration.Bind(RuntimeData);
            AddConsideration(spellElementConsideration);

            var spellQuantityConsideration = ScriptableObject.CreateInstance<SpellQuantityConsideration>();
            spellQuantityConsideration.name = "spell-quantity";
            spellQuantityConsideration.Bind(RuntimeData);
            AddConsideration(spellQuantityConsideration);
        }

        /// <summary>
        /// Allows you to bind runtime data that has changed. 
        /// In this case, we are passing in RuntimeData which would allow the consideration to consider if this action can be used anymore.
        /// </summary>
        /// <param name="consideration"></param>
        public override void OnConsiderationCheck(Consideration consideration)
        {
            base.OnConsiderationCheck(consideration);

            if(consideration is IDataBind<RuntimeSpellAttackData> spellDependent)
                spellDependent.Bind(RuntimeData);
        }

        public override EActionStatus Perform(Agent agent)
        {
            var gameContext = agent.GetContext<TurnBaseSpellGameContext>();
            gameContext.GameManager.Choose(this);
            RuntimeData.Used++;

            return EActionStatus.Completed;
        }
    }

    /// <summary>
    /// The runtime data of the spell attack. This is mutable, meaning its max value can be changed.
    /// This pattern just save us from having to clone SpellAttackData scriptable object. 
    /// </summary>
    [Serializable]
    public class RuntimeSpellAttackData
    {
        public SpellAttackData SpellAttackData;
        public int Used;
    }
}