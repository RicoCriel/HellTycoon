using System.Collections;
using System.Collections.Generic;
using Splines;
using UnityEngine;

namespace Buildings
{
    public class MachineOutput : BuildingFactoryBase
    {
        [SerializeField] private GameObject _demonPrefab;
        private MachineNode _node;

        public MachineNode Node
        {
            set => _node = value;
        }

        public void SpawnDemon(MachineNode node)
        {
            if (_demonPrefab == null) return;

            var demon = Instantiate(_demonPrefab, transform.position, Quaternion.identity);
            var handler = demon.GetComponent<DemonHandler>();

            if (handler == null) return;

            handler.HornLevel = node.Stat0;
            handler.FaceLevel = node.Stat1;
            handler.WingsLevel = node.Stat2;
            handler.ArmorLevel = node.Stat3;


        }

        protected override void ExecuteMachineProcessingBehaviour()
        {
            if (_unprocessedDemonContainer.Count > 0)
            {
                foreach (var demon in _unprocessedDemonContainer)
                {
                    if (demon.TryGetComponent<DemonHandler>(out DemonHandler handler))
                    {
                        handler.HornLevel = _node.Stat0;
                        handler.FaceLevel = _node.Stat1;
                        handler.WingsLevel = _node.Stat2;
                        handler.ArmorLevel = _node.Stat3;
                    }
                }
                base.ExecuteMachineProcessingBehaviour();
            }
        }
    }
}

