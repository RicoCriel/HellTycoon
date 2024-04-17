using System.Collections;
using System.Collections.Generic;
using Economy;
using TinnyStudios.AIUtility;
using UnityEngine;

namespace Tycoons
{
    public class TycoonDataContext : MonoBehaviour, IAgentDataContext
    {
        public EconomyManager EconomyManager;
        public StatType Preference;
        public StatType CurrentProduction;
        public TycoonType TycoonType;
    }
}

