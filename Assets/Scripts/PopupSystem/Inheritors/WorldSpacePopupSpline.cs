using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace PopupSystem.Inheritors
{
    public class WorldSpacePopupSpline : WorldSpacePopupBase
    {
        [Header("unprocessedSouls")]
        [SerializeField] private Image _soulsImage;
        [SerializeField] private TextMeshProUGUI _soulsOnSplineCounter;

        public void OverRideLocalPosition(Vector3 position)
        {
            MovePopupToPositionLocal(position);
        }

        public void SetSoulsOnSplineCounter(int value)
        {
            if (_soulsOnSplineCounter != null)
                _soulsOnSplineCounter.text = value.ToString();
        }
        public void setSoulsOnSplineImage(Sprite sprite)
        {
            if (_soulsImage != null)
                _soulsImage.sprite = sprite;
        }



    }



}
