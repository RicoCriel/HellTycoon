using PopupSystem.Inheritors;
using PopupSystem;
using Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Buildings
{
    public class BuildingFactoryBase : BuildingBase
    {
        internal Queue<GameObject> _unprocessedDemonContainer = new Queue<GameObject>();
        internal Queue<GameObject> _processedDemonContainer = new Queue<GameObject>();


        [SerializeField] internal int MaxDemons = 10;

        [Header("Popup")]
        [SerializeField] internal WorldSpacePopupBase _popupFactory;
        [SerializeField] internal BuildingPopupActivator _popupActivator;

        internal int MachineRatePerSecond = 1;
        internal int _machineIdx = -1;

        private Coroutine _mySpawningRoutine;

        [HideInInspector]
        public UnityEvent<BuildingFactoryBase> OnDestruct;

        protected new void Awake()
        {
            base.Awake();

            OnDestruct = new UnityEvent<BuildingFactoryBase>();

            _mySpawningRoutine = StartCoroutine(MachineRoutine());

            //Popup setup
            if (_popupFactory != null)
            {
                if (_popupActivator != null)
                {
                    _popupActivator.SetPopupper(_popupFactory);
                }
                else
                {
                    Debug.LogWarning("Popup activator not assigned");
                }

                _popupFactory.DestroyButtonClicked += OnDestroyButtonClicked;
            }
            else
            {
                Debug.Log("Popup not assigned");
            }
        }

        private void OnDestroy()
        {
            OnDestruct?.Invoke(this);
        }

        protected void OnDisable()
        {
            if(_popupFactory == null) return;
            _popupFactory.DestroyButtonClicked -= OnDestroyButtonClicked;
        }

        public void Initialize(int machineIdx)
        {
            _machineIdx = machineIdx;
        }

        protected void ResumeProcessing()
        {
            if (_mySpawningRoutine == null)
            {
                StartCoroutine(MachineRoutine());
            }
        }

        protected void StopSpawning()
        {
            StopCoroutine(MachineRoutine());
        }

        private IEnumerator MachineRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(MachineRatePerSecond / 2f);
                ExecuteMachineProcessingBehaviour();
                yield return new WaitForSeconds(MachineRatePerSecond / 2f);
                ExecuteMachineSpawningBehaviour();
            }
        }


        public bool ContainerHasSpace(Queue<GameObject> DemonList)
        {
            return DemonList.Count < MaxDemons;
        }

        public void AddDemon(Queue<GameObject> DemonList, GameObject demon)
        {
            demon.SetActive(false);
            DemonList.Enqueue(demon);
        }

        public GameObject GetDemon(Queue<GameObject> DemonList)
        {
            return DemonList.Dequeue();
        }

        protected virtual void ExecuteMachineProcessingBehaviour()
        {
            if (_unprocessedDemonContainer.Count > 0 && ContainerHasSpace(_processedDemonContainer))
            {
                AddDemon(_processedDemonContainer, _unprocessedDemonContainer.Dequeue());
                PlayProcessingAnimation();
            }

        }

        protected virtual void PlayProcessingAnimation()
        {
            //todo play animation
            //hook up aninmator etc preferably make a new class for each factory
        }

        protected virtual void ExecuteMachineSpawningBehaviour()
        {
            SpawnDemon(_exitBoxes[0]);
        }

        public void SpawnDemon(PlaceholderConnectorHitBox OutNode)
        {
            if (_processedDemonContainer.Count > 0 && OutNode.Spline != null)
            {
                if (OutNode.Spline.EndConnector.myBuildingNode.TryGetComponent(out BuildingFactoryBase nextMachine))
                {
                    if (nextMachine.ContainerHasSpace(nextMachine._unprocessedDemonContainer))
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

        // Popup Logic-----------------------------------------------------
        private void OnDestroyButtonClicked(object sender, PopupSystem.PopupClickedEventArgs e)
        {
            Destroy(gameObject);
            Debug.Log("Destroy Button Clicked");
        }

        public void PopupForceClose()
        {
            //manually call closing
            _popupFactory.ClosePopup();
            //or (this might open if closed)
            _popupActivator.PopupActivatorClicked();
        }
    }

}
