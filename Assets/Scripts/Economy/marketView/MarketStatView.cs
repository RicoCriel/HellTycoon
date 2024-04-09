using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Economy
{
    public class MarketStatView : MonoBehaviour
    {
        [SerializeField]
        private Image _partImage;
        [SerializeField]
        private TextMeshProUGUI PartName;
        [SerializeField]
        private TextMeshProUGUI _partPriceText;
        [SerializeField]
        private TextMeshProUGUI _partAmountText;

        public void Init(string partName, float partPriceText, float partAmountText)
        {
            PartName.text = partName;
            // _partImage.sprite = partImage;
            UpdateText(partPriceText, partAmountText);
        }

        public void UpdateText(float price, float amount)
        {
            SetPartPrice(price);
            SetPartAmount(amount);
        }
        private void SetPartPrice(float price)
        {
            _partPriceText.text = price.ToString();
        }

        private void SetPartAmount(float amount)
        {
            _partAmountText.text = amount.ToString();
        }

    }
}
