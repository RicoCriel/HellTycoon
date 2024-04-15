using System;
using UnityEngine;

namespace Tycoons
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TycoonData", order = 1)]
    public class TycoonData : ScriptableObject
    {
        public TycoonType TycoonType;

        public float StartMoney;
        
        [Header("buying")]
        [Range(0.1f, 60)]
        public float MinBuyTime;
        [Range(0.1f, 60)]
        public float MaxBuyTime;
        [Range(0.1f, 50)]
        public float MinBuyAmount;
        [Range(0.1f, 500)]
        public float MaxBuyAmount;

        [Header("Selling")]
        [Range(0.1f, 60)]
        public float MinSellTime;
        [Range(0.1f, 60)]
        public float MaxSellTime;
        [Range(1f, 50)]
        public int MinSellAmount;
        [Range(1f, 50)]
        public int MaxSellAmount;

        [Header("preferences")]
        [Range(0f, 100)]
        private float _bodyPreference;
        [Range(0f, 100)]
        private float _hornPreference;
        [Range(0f, 100)]
        private float _wingsPreference;
        [Range(0f, 100)]
        private float _armorPreference;
        [Range(0f, 100)]
        private float _facePreference;
        
        public float normalizedBodyPreference => CalculateBodyPreference();
        public float normalizedHornPreference => CalculateHornPreference();
        public float normalizedWingsPreference => CalculateWingsPreference();
        public float normalizedArmorPreference => CalculateArmorPreference();
        public float normalizedFacePreference => CalculateFacePreference();
        
        public float CalculateBodyPreference()
        {
            return _bodyPreference / (_bodyPreference + _hornPreference + _wingsPreference + _armorPreference + _facePreference);
        }
        
        public float CalculateHornPreference()
        {
            return _hornPreference / (_bodyPreference + _hornPreference + _wingsPreference + _armorPreference + _facePreference);
        }
        
        public float CalculateWingsPreference()
        {
            return _wingsPreference / (_bodyPreference + _hornPreference + _wingsPreference + _armorPreference + _facePreference);
        }
        
        public float CalculateArmorPreference()
        {
            return _armorPreference / (_bodyPreference + _hornPreference + _wingsPreference + _armorPreference + _facePreference);
        }
        
        public float CalculateFacePreference()
        {
            return _facePreference / (_bodyPreference + _hornPreference + _wingsPreference + _armorPreference + _facePreference);
        }
        
        [Header("Auto")]
        [Range(0.1f, 60)]
        public float MinAutoCostTime;
        [Range(0.1f, 60)]
        public float MaxAutoCostTime;
        [Range(0.1f, 50)]
        public float MinAutoCostAmount;
        [Range(0.1f, 500)]
        public float MaxAutoCostAmount;
        
    }
}
