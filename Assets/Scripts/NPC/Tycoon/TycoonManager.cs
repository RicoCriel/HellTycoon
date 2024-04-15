using System.Collections;
using System.Collections.Generic;
using Economy;
using UnityEngine;

namespace Tycoons
{
    public class TycoonManager : MonoBehaviour
    {
        [SerializeField] private EconomyManager _economyManager;
        [SerializeField] private Tycoon _tycoonPrefab;
        [SerializeField] private List<TycoonData> _thisGameTycoons = new List<TycoonData>();
        private Dictionary<TycoonType, AIBehaviourBase> _aiBehaviours = new Dictionary<TycoonType, AIBehaviourBase>();

        // Start is called before the first frame update
        void Awake()
        {
            InitTycoonBehaviours();
            InitTycoons();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitTycoonBehaviours()
        {
            _aiBehaviours.Add(TycoonType.TycoonOne, new AIBehaviourTycoon1());
            _aiBehaviours.Add(TycoonType.TycoonTwo, new AIBehaviourTycoon2());
            _aiBehaviours.Add(TycoonType.TycoonThree, new AIBehaviourTycoon3());
        }

        private void InitTycoons()
        {
            foreach (TycoonData tycoonData in _thisGameTycoons)
            {
                Tycoon tycoon = Instantiate(_tycoonPrefab, transform);
                tycoon.gameObject.name = tycoonData.ToString();

                if (InitializeTycoon(tycoonData, tycoon)) return;

                _economyManager.SetupTycoonEconomy(tycoon);
            }
        }

        private bool InitializeTycoon(TycoonData tycoonType, Tycoon tycoon)
        {
            if (_aiBehaviours.TryGetValue(tycoonType.TycoonType, out AIBehaviourBase aiBehaviour))
            {
                tycoon.Init(tycoonType, aiBehaviour, _economyManager);
            }
            else
            {
                Debug.LogError("AI Behaviour not found");
                return true;
            }
            return false;
        }
    }
}

