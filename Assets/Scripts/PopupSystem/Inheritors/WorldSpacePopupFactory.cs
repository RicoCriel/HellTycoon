using Buildings;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace PopupSystem.Inheritors
{
    public class WorldSpacePopupFactory : WorldSpacePopupBase
    {
        [Header("unprocessedSouls")]
        [SerializeField] private Image _unprocessedSoulsImage;
        [SerializeField] private TextMeshProUGUI _unprocessedSoulsCounter;

        [Header("ProcessedSouls")]
        [SerializeField] private Image _processedSoulsImage;
        [SerializeField] private TextMeshProUGUI _processedSoulsCounter;

        [Space]
        [Header("Processing")]
        [SerializeField] private Image _soulsProcessingBar;
        private TextMeshProUGUI _processedSoulsText;
        
        // [SerializeField]
        // private Image MachineTypeImage;
        [SerializeField] private TextMeshProUGUI _machineTypeText;
        
        private bool _isPaused = false;

        private MachinePart _fearNeededCount;
        [SerializeField] private TextMeshProUGUI _fearNeededText;

        protected override void Awake()
        {
            base.Awake();
            _soulsProcessingBar.fillAmount = 0;


            
            _processedSoulsText = null;
            _fearNeededCount = gameObject.GetComponentInParent<MachinePart>();
            if (_fearNeededCount != null)
            {
                _fearNeededText.SetText(_fearNeededCount.GetReqFearLevel().ToString());
            }
        }

        //callable public methods
        
        public void SetMachineInformation(/*Sprite sprite,*/ string machineType)
        {
            // MachineTypeImage.sprite = sprite;
            _machineTypeText.text = machineType;
        }
        
        //unprocessed souls
        public void SetUnprocessedSoulsCounter(int value)
        {
            _unprocessedSoulsCounter.text = value.ToString();
        }
        public void SetUnprocessedSoulsImage(Sprite sprite)
        {
            _unprocessedSoulsImage.sprite = sprite;
        }
        
        //processed souls
        public void SetProcessedSoulsCounter(int value)
        {
            _processedSoulsCounter.text = value.ToString();
        }
        public void SetProcessedSoulsImage(Sprite sprite)
        {
            _processedSoulsImage.sprite = sprite;
        }
        
        //Soul processing bar
        public void ProcessSoulBarUI(float processingTime)
        {
            _soulsProcessingBar.DOFillAmount(1, processingTime).OnComplete(() =>
            {
                _soulsProcessingBar.fillAmount = 0;
                // OnSoulProcessingResumed(new PopupClickedEventArgs());
            });
        }

        //Buttons
        public void ButtonTogglePauseProcessing()
        {
            if (!_isPaused)
            {
                _processedSoulsText.text = "Paused";
                OnSoulProcessingPaused(new PopupClickedEventArgs());
            }
            else
            {
                OnSoulProcessingPaused(new PopupClickedEventArgs());
                _processedSoulsText.text = "Processing";
            }
        }

        public event EventHandler<PopupClickedEventArgs> SoulProcessingPaused;
        public event EventHandler<PopupClickedEventArgs> SoulProcessingResumed;

        private void OnSoulProcessingPaused(PopupClickedEventArgs e)
        {
            EventHandler<PopupClickedEventArgs> handler = SoulProcessingPaused;
            handler?.Invoke(this, e);
        }
        private void OnSoulProcessingResumed(PopupClickedEventArgs e)
        {
            EventHandler<PopupClickedEventArgs> handler = SoulProcessingResumed;
            handler?.Invoke(this, e);
        }
    }

    public class PopupClickedEventArgs : EventArgs
    {
        public PopupClickedEventArgs() { }
    }
}
