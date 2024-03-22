using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Buildings
{
    public class MachinePart : BuildingFactoryBase
    {
        [SerializeField] private MachineType _machineType;

        private int _partIndex = -1;
        public int PartIndex => _partIndex;

        public MachineType MachineType => _machineType;
        public MachineNode Node;

        public delegate void DemonStatsChanged();
        public static event DemonStatsChanged onDemonStatsChanged;

        public void Initialize(MachineNode node, int machineIdx, int partIdx)
        {
            Node = node;
            _partIndex = partIdx;
            _machineIdx = machineIdx;
        }

        

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

