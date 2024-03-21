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
        [SerializeField]
        private Image UnprocessedSoulsImage;
        [SerializeField]
        private TextMeshProUGUI UnprocessedSoulsCounter;

        [Header("ProcessedSouls")]
        [SerializeField]
        private Image ProcessedSoulsImage;
        [SerializeField]
        private TextMeshProUGUI ProcessedSoulsCounter;

        [Space]
        [Header("Processing")]
        [SerializeField]
        private Image souldProcessingBar;
        private TextMeshProUGUI ProcessedSouldText;
        
        // [SerializeField]
        // private Image MachineTypeImage;
        [SerializeField]
        private TextMeshProUGUI MachineTypeText;

        private bool _isPaused = false;
        
        protected override void Awake()
        {
            base.Awake();
            souldProcessingBar.fillAmount = 0;
        }

        //callable public methods
        
        public void SetMachineInformation(/*Sprite sprite,*/ string machineType)
        {
            // MachineTypeImage.sprite = sprite;
            MachineTypeText.text = machineType;
        }
        
        //unprocessed souls
        public void SetUnprocessedSoulsCounter(int value)
        {
            UnprocessedSoulsCounter.text = value.ToString();
        }
        public void setUnprocessedSoulsImage(Sprite sprite)
        {
            UnprocessedSoulsImage.sprite = sprite;
        }
        
        //processed souls
        public void SetProcessedSoulsCounter(int value)
        {
            ProcessedSoulsCounter.text = value.ToString();
        }
        public void setProcessedSoulsImage(Sprite sprite)
        {
            ProcessedSoulsImage.sprite = sprite;
        }
        
        //Soul processing bar
        public void ProcessSoulBarUI(float ProcessingTime)
        {
            souldProcessingBar.DOFillAmount(1, ProcessingTime).OnComplete(() =>
            {
                souldProcessingBar.fillAmount = 0;
                // OnSoulProcessingResumed(new PopupClickedEventArgs());
            });
        }

        //Buttons
        public void ButtonTogglePauseProcessing()
        {
            if (!_isPaused)
            {
                ProcessedSouldText.text = "Paused";
                OnSoulProcessingPaused(new PopupClickedEventArgs());
            }
            else
            {
                OnSoulProcessingPaused(new PopupClickedEventArgs());
                ProcessedSouldText.text = "Processing";
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
