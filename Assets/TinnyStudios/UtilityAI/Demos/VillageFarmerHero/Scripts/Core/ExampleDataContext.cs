using System;
using System.Diagnostics;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// An example implementation of <see cref="IAgentDataContext"/> concretely defining the game data for the farmer hero.
    /// This is accessed by all actions and consideration.
    /// You do not need to do it this way if your game has a different way of injecting dependencies.
    /// </summary>
    public class ExampleDataContext : MonoBehaviour, IAgentDataContext
    {
        public Stats Stats;
        public Inventory Inventory;

        public StatWatcher HungerWatcher;
        public StatWatcher SleepinessWatcher;

        [Header("Dependencies")]
        public TreeObjectManager TreeObjectManager;
        public MonsterManager MonsterManager;

        private void Awake()
        {
            HungerWatcher.Start();
            SleepinessWatcher.Start();
        }

        private void Update()
        {
            HungerWatcher.Update(ref Stats.Hunger);
            SleepinessWatcher.Update(ref Stats.Sleepiness);
        }
    }

    /// <summary>
    /// Configures a stat over time. This is used for increasing Hunger and Sleepiness.
    /// </summary>
    [Serializable]
    public class StatWatcher
    {
        private readonly Stopwatch Stopwatch = new Stopwatch();

        public Action OnCompleted;
        public float IncreaseInterval = 2.0f;
        public float IncreaseAmount = 0.1f;

        public void Start()
        {
            Stopwatch.Start();
        }

        public void Update(ref float value)
        {
            if (Stopwatch.Elapsed.TotalSeconds >= IncreaseInterval)
            {
                value += IncreaseAmount;
                OnCompleted?.Invoke();
                Stopwatch.Restart();
            }
        }
    }

    /// <summary>
    /// The stats used in the farmer hero example.
    /// </summary>
    [Serializable]
    public class Stats
    {
        public float Hunger;
        public float Sleepiness;
    }

    /// <summary>
    /// A very simple implementation of an inventory for the example.
    /// </summary>
    [Serializable]
    public class Inventory
    {
        public float Money;
        public int Wood;
        
        // TODO replace with an actual equipment system
        public bool HasWeapon;
    }
}