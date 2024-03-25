using System.Collections;
using System.Collections.Generic;
using PopupSystem.Inheritors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Buildings
{
    public class MachinePart : BuildingFactoryBase
    {
        [Header("Machine")]
        [SerializeField] private MachineType _machineType;
        private WorldSpacePopupFactory _popup;

        private int _partIndex = -1;
        public int PartIndex => _partIndex;

        public MachineType MachineType => _machineType;

        [HideInInspector]
        public MachineNode Node;

       

        protected new void Awake()
        {
            base.Awake();

            if (TryGetComponent(out _popup))
            {
                _popup.SoulProcessingPaused += OnSoulProcessingPaused;
                _popup.SoulProcessingResumed += OnSoulProcessingResumed;
            }
            else
            {
                Debug.LogWarning("Popup is not factory type");
            }
        }

        protected void OnDisable()
        {
            base.OnDisable();

            if (_popup != null)
            {
                _popup.SoulProcessingPaused -= OnSoulProcessingPaused;
                _popup.SoulProcessingResumed -= OnSoulProcessingResumed;
            }

        }

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

            if (_popup)
            {
                _popup.SetProcessedSoulsCounter(_processedDemonContainer.Count);
                _popup.SetUnprocessedSoulsCounter(_unprocessedDemonContainer.Count);
            }
        }

        // Popup Logic-----------------------------------------------------
        private void OnSoulProcessingPaused(object sender, PopupSystem.Inheritors.PopupClickedEventArgs e)
        {
            Debug.Log("Soul Processing Paused");
        }

        private void OnSoulProcessingResumed(object sender, PopupSystem.Inheritors.PopupClickedEventArgs e)
        {
            Debug.Log("Soul Processing Resumed");
        }

        public void StartUIProcessing(int amount, float time)
        {
            if (_popup != null)
            {
                _popup.SetUnprocessedSoulsCounter(amount);
                _popup.ProcessSoulBarUI(time);
            }

        }

        public void DoneUIProcessing()
        {
            if (_popup != null)
                _popup.SetUnprocessedSoulsCounter(0);
        }
    }
}

