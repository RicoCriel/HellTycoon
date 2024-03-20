using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class MachinePart : BuildingFactoryBase
    {
        [SerializeField] private MachineType _machineType;
        public MachineType MachineType => _machineType;
        public MachineNode Node;

        protected override void ExecuteMachineProcessingBehaviour()
        {
            if (_unprocessedDemonContainer.Count > 0)
            {
                foreach (var demon in _unprocessedDemonContainer)
                {
                    if (demon.TryGetComponent<DemonHandler>(out DemonHandler handler))
                    {
                        handler.HornLevel = Node.Stat0;
                        handler.FaceLevel = Node.Stat1;
                        handler.WingsLevel = Node.Stat2;
                        handler.ArmorLevel = Node.Stat3;
                    }
                }
                base.ExecuteMachineProcessingBehaviour();
            }
        }
    }
}

