using System;
using UnityEngine;
namespace Economy
{
    
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TycoonData", order = 1)]
    public class TycoonData : ScriptableObject
    {
        public tycoonType _tycoonType;

        public float _startMoney;
        
        [Header("buying")]
        [Range(0.1f, 60)]
        public float minBuyTime;
        [Range(0.1f, 60)]
        public float maxBuyTime;
        [Range(0.1f, 50)]
        public float minBuyAmount;
        [Range(0.1f, 500)]
        public float maxBuyAmount;

        [Header("Selling")]
        [Range(0.1f, 60)]
        public float minSellTime;
        [Range(0.1f, 60)]
        public float maxSellTime;
        [Range(1f, 50)]
        public int minSellAmount;
        [Range(1f, 50)]
        public int maxSellAmount;

        [Header("preferences")]
        [Range(0f, 100)]
        private float BodyPreference;
        [Range(0f, 100)]
        private float HornPreference;
        [Range(0f, 100)]
        private float WingsPreference;
        [Range(0f, 100)]
        private float ArmorPreference;
        [Range(0f, 100)]
        private float FacePreference;
        
        public float NormalizedBodyPreference => CalculateBodyPreference();
        public float NormalizedHornPreference => CalculateHornPreference();
        public float NormalizedWingsPreference => CalculateWingsPreference();
        public float NormalizedArmorPreference => CalculateArmorPreference();
        public float NormalizedFacePreference => CalculateFacePreference();
        
        public float CalculateBodyPreference()
        {
            return BodyPreference / (BodyPreference + HornPreference + WingsPreference + ArmorPreference + FacePreference);
        }
        
        public float CalculateHornPreference()
        {
            return HornPreference / (BodyPreference + HornPreference + WingsPreference + ArmorPreference + FacePreference);
        }
        
        public float CalculateWingsPreference()
        {
            return WingsPreference / (BodyPreference + HornPreference + WingsPreference + ArmorPreference + FacePreference);
        }
        
        public float CalculateArmorPreference()
        {
            return ArmorPreference / (BodyPreference + HornPreference + WingsPreference + ArmorPreference + FacePreference);
        }
        
        public float CalculateFacePreference()
        {
            return FacePreference / (BodyPreference + HornPreference + WingsPreference + ArmorPreference + FacePreference);
        }
        
        [Header("Auto")]
        [Range(0.1f, 60)]
        public float minAutoCostTime;
        [Range(0.1f, 60)]
        public float maxAutoCostTime;
        [Range(0.1f, 50)]
        public float minAutoCostAmount;
        [Range(0.1f, 500)]
        public float maxAutoCostAmount;
        
    }
}
