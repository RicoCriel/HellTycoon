using JetBrains.Annotations;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Economy
{
    public class Tycoon : MonoBehaviour
    {
        // private tyCoonType  _tycoonType;
        public TycoonType TycoonType{ get; private set; }

        public SoulManager SoulManager{ get; private set; }

        public TimeManager TimeManager{ get; private set; }

        public AIBehaviourBase AIBehaviour{ get; private set; }

        public TycoonData TycoonData{ get; private set; }

        private void Awake()
        {
            if (SoulManager == null)
            {
                SoulManager = GetComponentInChildren<SoulManager>();
            }
            if (TimeManager == null)
            {
                TimeManager = GetComponentInChildren<TimeManager>();
            }
        }
        public void Init(TycoonData tycoonData, AIBehaviourBase aiBehaviour)
        {
            TycoonData = tycoonData;
            TycoonType = tycoonData.TycoonType;
            AIBehaviour = aiBehaviour;
            SoulManager.Init(tycoonData);

            ResetBuyTimer(tycoonData);
            ResetSellTimer(tycoonData);
            ResetAutoCostTimer(tycoonData);
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

        private void Sell(EconomyManager economyManager, Market market)
        {
            AIBehaviour.SellBehaviour(economyManager, market, SoulManager, this);
        }

        private void Buy(EconomyManager economyManager, Market market)
        {
            AIBehaviour.BuyBehaviour(economyManager, market, SoulManager, this);
        }

        private void AutoCost(EconomyManager economyManager, Market market)
        {
            AIBehaviour.AutoCostBehaviour(economyManager, market, SoulManager, this);
        }

        private float currentSellTimer;
        private float currentBuyTimer;
        private float currentAutoCostTimer;

        private void Update()
        {
            currentSellTimer -= Time.deltaTime;
            currentBuyTimer -= Time.deltaTime;
            currentAutoCostTimer -= Time.deltaTime;
            if (currentSellTimer <= 0)
            {
                OnSellTriggered(new TycoonEventArgs(TycoonType));;
                ResetSellTimer(TycoonData);
            }
            if (currentBuyTimer <= 0)
            {
                OnBuyTriggered(new TycoonEventArgs(TycoonType));
                ResetBuyTimer(TycoonData);
            }
            if (currentAutoCostTimer <= 0)
            {
                OnAutoCostTriggered(new TycoonEventArgs(TycoonType));
                ResetAutoCostTimer(TycoonData);
            }
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
        public TycoonType TycoonType{ get; }
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
