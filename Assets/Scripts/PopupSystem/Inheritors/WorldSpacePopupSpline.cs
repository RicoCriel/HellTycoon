using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace PopupSystem.Inheritors
{
    public class WorldSpacePopupSpline : WorldSpacePopupBase
    {
        [Header("unprocessedSouls")]
        [SerializeField]
        private Image SoulsImage;
        [SerializeField]
        private TextMeshProUGUI SoulsOnSplineCounter;
        
        public void OverRideLocalPosition(Vector3 position)
        {
            MovePopupToPositionLocal(position);
        }
        
        public void SetSoulsOnSplineCounter(int value)
        {
            SoulsOnSplineCounter.text = value.ToString();
        }
        public void setSoulsOnSplineImage(Sprite sprite)
        {
            SoulsImage.sprite = sprite;
        }
        
        
        
    }
    
    
    
}
