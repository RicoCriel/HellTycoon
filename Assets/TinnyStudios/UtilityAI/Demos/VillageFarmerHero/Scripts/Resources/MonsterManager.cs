using System;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// In this example, the monster manager instantiate 1 every 10s
    /// </summary>
    public class MonsterManager : ObjectManager<Monster, MonsterManager>
    {
        public float SpawnInterval = 10;
        public int Max = 3;

        public Stopwatch Stopwatch = new Stopwatch();

        private void Start()
        {
            Stopwatch.Start();
        }

        private void Update()
        {
            if (Count >= Max)
                return;

            if (Stopwatch.Elapsed.TotalSeconds >= SpawnInterval)
            {
                var spawnPosition = transform.position;
                spawnPosition.x += Random.Range(-2, 2);
                spawnPosition.z += Random.Range(-2, 2);

                Spawn(spawnPosition, Quaternion.identity);
                Stopwatch.Restart();
            }
        }
    }
}