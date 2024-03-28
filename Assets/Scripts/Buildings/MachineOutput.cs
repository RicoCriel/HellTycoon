using System.Collections;
using System.Collections.Generic;
using Splines;
using UnityEngine;

namespace Buildings
{
    public class MachineOutput : BuildingFactoryBase
    {
        [SerializeField] private GameObject _demonPrefab;
        [SerializeField] private MachineType _machineType;

        public void SpawnDemon(MachineNode node)
        {
            if (_demonPrefab == null) return;

            var demon = Instantiate(_demonPrefab, transform.position, Quaternion.identity);
            var handler = demon.GetComponent<DemonHandler>();

            if (handler == null) return;



        }

        protected override void ExecuteMachineProcessingBehaviour()
        {
            if (_unprocessedDemonContainer.Count > 0)
            {
                foreach (var demon in _unprocessedDemonContainer)
                {
                    if (demon.TryGetComponent<DemonHandler>(out DemonHandler handler))
                    {
                        handler.SetStats(_machineType);
                    }
                }
                base.ExecuteMachineProcessingBehaviour();
            }
        }
    }
}

