using Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Buildings
{
    public class BuildingFactoryBase : BuildingBase
    {
        internal Queue<GameObject> UnprocessedDemonContainer = new Queue<GameObject>();
        internal Queue<GameObject> _processedDemonContainer = new Queue<GameObject>();

        [SerializeField]
        internal int MaxDemons = 10;

        internal int MachineSpawnRatePerSecond = 1;

        private Coroutine _myspanwingRoutine;

        protected void Awake()
        {
            base.Awake();

            _myspanwingRoutine = StartCoroutine(DemonSpawningRoutine());
        }

        protected void ResumeSpawning()
        {
            if (_myspanwingRoutine == null)
            {
                StartCoroutine(DemonSpawningRoutine());
            }
        }

        protected void StopSpawning()
        {
            StopCoroutine(DemonSpawningRoutine());
        }

        private IEnumerator DemonSpawningRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(MachineSpawnRatePerSecond);
                ExecuteMachineSpawningBehaviour();
            }
        }


        public bool ContainerHasSpace(Queue<GameObject>DemonList)
        {
            return DemonList.Count < MaxDemons;
        }

        public void AddDemon(Queue<GameObject>DemonList, GameObject demon)
        {
            DemonList.Enqueue(demon);
        }

        public GameObject GetDemon(Queue<GameObject>DemonList)
        {
            return DemonList.Dequeue();
        }

        protected virtual void ExecuteMachineProcessingBehaviour()
        {
            if (UnprocessedDemonContainer.Count > 0 && ContainerHasSpace()
            {
                AddDemon(UnprocessedDemonContainer.Dequeue());
            }
            {
                //Process demon sprites?
            }
        }

        protected virtual void ExecuteMachineSpawningBehaviour()
        {
            SpawnDemon(_exitBoxes[0]);
        }

        public void SpawnDemon(PlaceholderConnectorHitBox OutNode)
        {
            if (_processedDemonContainer.Count > 0)
            {
                if (OutNode.Spline.EndConnector.myBuildingNode.TryGetComponent(out BuildingFactoryBase nextMachine))
                {
                    if (nextMachine.ContainerHasSpace())
                    {
                        //if this machine still has demons
                        if (OutNode.SpawnObject(_processedDemonContainer.Peek()))
                        {
                            _processedDemonContainer.Dequeue();
                        }
                    }
                }
            }
        }



    }

}
