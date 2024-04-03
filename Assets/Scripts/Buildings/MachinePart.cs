using System.Collections;
using System.Collections.Generic;
using PopupSystem.Inheritors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace Buildings
{
    public class MachinePart : BuildingFactoryBase
    {
        [Header("Machine")]
        [SerializeField] private MachineType _machineType;
        private WorldSpacePopupFactory _popup;

        public MachineType MachineType => _machineType;



        [HideInInspector]
        public MachineNode Node;


        //TODO : calcualte this required fear depending on which layer machine is on
        [SerializeField] private float _requiredFearLevel;

        [SerializeField] private int _upkeepCost;

        public int UkpeepCost => _upkeepCost = 20;


        [SerializeField] private int _upkeepInterval = 5;


       private SoulManager _soulManager;
       private MachineManager _machineManager;


        void Start()
        {
            _soulManager = FindObjectOfType<SoulManager>();
            _machineManager = FindObjectOfType<MachineManager>();
            
            StartCoroutine(PayUpkeepRoutine());
        }
        IEnumerator PayUpkeepRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_upkeepInterval);
                PayUpkeep();
            }
        }


        private void PayUpkeep()
        {
            _soulManager.SubtractMoney(_upkeepCost);
            Debug.Log("Paid upkeep for machine");
        }



        protected new void Awake()
        {
            base.Awake();

            _popup = GetComponentInChildren<WorldSpacePopupFactory>();
            if (_popup)
            {
                _popup.SoulProcessingPaused += OnSoulProcessingPaused;
                _popup.SoulProcessingResumed += OnSoulProcessingResumed;
            }
            else
            {
                Debug.LogWarning("Popup is not factory type");
            }
        }
        public float GetReqFearLevel()
        {
            return _requiredFearLevel;
        }
        protected new void OnDisable()
        {
            base.OnDisable();

            if (_popup != null)
            {
                _popup.SoulProcessingPaused -= OnSoulProcessingPaused;
                _popup.SoulProcessingResumed -= OnSoulProcessingResumed;
            }

        }



        public override void AddDemon(Queue<GameObject> DemonList, GameObject demon)
        {
            base.AddDemon(DemonList, demon);

            StartUIProcessing(_unprocessedDemonContainer.Count, MachineRatePerSecond);
            _popup.SetUnprocessedSoulsCounter(_unprocessedDemonContainer.Count);
        }

        protected override void ExecuteMachineSpawningBehaviour()
        {
            base.ExecuteMachineSpawningBehaviour();

            if(_unprocessedDemonContainer.Count > 0)
                StartUIProcessing(1, MachineRatePerSecond);
        }

        protected override void ExecuteMachineProcessingBehaviour()
        {
            if (_unprocessedDemonContainer.Count > 0 && _unprocessedDemonContainer.Count < MaxDemons)
            {
                foreach (var demon in _unprocessedDemonContainer)
                {
                    if(demon == null) continue;

                    if (demon.TryGetComponent<DemonHandler>(out DemonHandler handler))
                    {
                        handler.SetStats(_machineType);
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
            StopSpawning();
            Debug.Log("Soul Processing Paused");
        }

        private void OnSoulProcessingResumed(object sender, PopupSystem.Inheritors.PopupClickedEventArgs e)
        {
            ResumeProcessing();
            Debug.Log("Soul Processing Resumed");
        }

        public void StartUIProcessing(int amount, float time)
        {
            if (_popup != null)
            {
                //_popup.SetUnprocessedSoulsCounter(amount);
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

