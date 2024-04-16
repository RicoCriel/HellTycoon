using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.TurnBasedSpell
{
    /// <summary>
    /// The Game Context data for each agent in the game.
    /// Here GameManager is manually injected by Unity's interface. You could have your game manager pass all the dependencies down instead.
    /// Up to you. 
    /// </summary>
    public class TurnBaseSpellGameContext : MonoBehaviour, IAgentDataContext
    {
        public TurnBasedGameManager GameManager;
        public TurnBaseCharacter Self;
        public TurnBaseCharacter Foe;
    }
}