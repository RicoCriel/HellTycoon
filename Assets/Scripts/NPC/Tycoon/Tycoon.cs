using JetBrains.Annotations;
using System;
using System.Collections;
using Economy;
using TinnyStudios.AIUtility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tycoons
{
    public class Tycoon : MonoBehaviour
    {
        [SerializeField] private float _productionChangeInterval = 10f;

        // private tyCoonType  _tycoonType;
        public TycoonType TycoonType { get; private set; }

        public EconomyManager EconomyManager { get; protected set; }

        public TimeManager TimeManager { get; private set; }

        //public AIBehaviourBase AIBehaviour{ get; private set; }

        public TycoonData TycoonData { get; private set; }

        [SerializeField] private Agent _agent;

        private void Awake()
        {

            if (TimeManager == null)
            {
                TimeManager = GetComponentInChildren<TimeManager>();
            }
        }

        public void Init(TycoonData tycoonData, AIBehaviourBase aiBehaviour, EconomyManager economyManager)
        {
            TycoonData = tycoonData;

            var context = _agent.GetContext<TycoonDataContext>();
            if (context != null)
            {
                context.TycoonType = TycoonData.TycoonType;
                context.EconomyManager = economyManager;
                //context.Preference = tycoonData.

                context.OnChangeProduction += () => StartCoroutine(ChangeProductionCooldown());
            }

            TycoonType = TycoonData.TycoonType;
            //AIBehaviour = aiBehaviour;
            EconomyManager = economyManager;

            ResetBuyTimer(tycoonData);
            ResetSellTimer(tycoonData);
            ResetAutoCostTimer(tycoonData);
        }

        private IEnumerator ChangeProductionCooldown()
        {
            yield return new WaitForSeconds(_productionChangeInterval);
            var context = _agent.GetContext<TycoonDataContext>();

            if (context != null)
            {
                context.CanChangeProduction = true;
            }
        }

        public void LoseLogic()
        {
            gameObject.SetActive(false);
        }

        private void ResetAutoCostTimer(TycoonData tycoonData)
        {
            currentAutoCostTimer = Random.Range(tycoonData.MinAutoCostTime, tycoonData.MaxAutoCostTime);
        }
        private void ResetSellTimer(TycoonData tycoonData)
        {

            currentSellTimer = Random.Range(tycoonData.MinSellTime, tycoonData.MaxSellTime);
        }
        private void ResetBuyTimer(TycoonData tycoonData)
        {

            currentBuyTimer = Random.Range(tycoonData.MinBuyTime, tycoonData.MaxBuyTime);
        }

        private void Sell()
        {
            //AIBehaviour.SellBehaviour(EconomyManager, this);
        }

        private void Buy()
        {
            //AIBehaviour.BuyBehaviour(EconomyManager, this);
        }

        private void AutoCost()
        {
            //AIBehaviour.AutoCostBehaviour(EconomyManager, this);
        }

        private float currentSellTimer;
        private float currentBuyTimer;
        private float currentAutoCostTimer;

        private void Update()
        {
            //currentSellTimer -= Time.deltaTime;
            //currentBuyTimer -= Time.deltaTime;
            //currentAutoCostTimer -= Time.deltaTime;
            //if (currentSellTimer <= 0)
            //{
            //    OnSellTriggered(new TycoonEventArgs(TycoonType));
            //    ResetSellTimer(TycoonData);
            //}
            //if (currentBuyTimer <= 0)
            //{
            //    OnBuyTriggered(new TycoonEventArgs(TycoonType));
            //    ResetBuyTimer(TycoonData);
            //}
            //if (currentAutoCostTimer <= 0)
            //{
            //    OnAutoCostTriggered(new TycoonEventArgs(TycoonType));
            //    ResetAutoCostTimer(TycoonData);
            //}
        }

        public event EventHandler<TycoonEventArgs> SellTriggered;
        public event EventHandler<TycoonEventArgs> BuyTriggered;
        public event EventHandler<TycoonEventArgs> AutoCostTriggered;

        private void OnSellTriggered(TycoonEventArgs e)
        {
            EventHandler<TycoonEventArgs> handler = SellTriggered;
            handler?.Invoke(this, e);
        }
        private void OnBuyTriggered(TycoonEventArgs e)
        {
            EventHandler<TycoonEventArgs> handler = BuyTriggered;
            handler?.Invoke(this, e);
        }
        private void OnAutoCostTriggered(TycoonEventArgs e)
        {
            EventHandler<TycoonEventArgs> handler = AutoCostTriggered;
            handler?.Invoke(this, e);
        }
    }
    public class TycoonEventArgs : EventArgs
    {
        public TycoonType TycoonType { get; }
        public TycoonEventArgs(TycoonType tycoonType)
        {
            TycoonType = tycoonType;
        }
    }


    public enum TycoonType
    {
        TycoonOne,
        TycoonTwo,
        TycoonThree
    }
}
