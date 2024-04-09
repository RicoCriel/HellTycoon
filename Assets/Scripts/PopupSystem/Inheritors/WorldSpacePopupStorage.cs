using System;
using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PopupSystem.Inheritors
{
    public class WorldSpacePopupStorage : WorldSpacePopupBase
    {
        [SerializeField] private TextMeshProUGUI _soulsCounter;
        [SerializeField] private TextMeshProUGUI _statusText;
        private bool _storing = true;

        protected override void Awake()
        {
            base.Awake();


        }

        public void SetSouls(int amount)
        {
            _soulsCounter.text = amount.ToString();
        }

        //Buttons
        public void ButtonTogglePauseProcessing()
        {
            if (!_storing)
            {
                _statusText.text = "Stockpiling";
                OnSoulProcessingPaused(new PopupClickedEventArgs());
            }
            else
            {
                OnSoulProcessingResumed(new PopupClickedEventArgs());
                _statusText.text = "Emptying";
            }

            _storing = !_storing;
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
}