using DG.Tweening;
using Splines.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace UI
{
    public class MessagePopupHUD : MonoBehaviour
    {
          [SerializeField]
        private TextMeshProUGUI MessageText;
        [SerializeField]
        private CanvasGroup MessageCanvasGroup;
        [SerializeField]
        private Transform MessageTextScalar;

        Sequence TurnMessageSequence;
        
 

        // private void Awake()
        // {
        //     TurnMessageSequence = DOTween.Sequence();
        // }
        
        public static event Action<string> OnPopupTriggered;

        public static void TriggerPopup(string message)
        {
            OnPopupTriggered?.Invoke(message);
        }

        private void OnEnable()
        {
            TurnMessageSequence = DOTween.Sequence();
            OnPopupTriggered += DisplayPopupText;
        }

        private void OnDisable()
        {
            OnPopupTriggered += DisplayPopupText;
        }

        public void closePopup()
        {
            TurnMessageSequence.Append(MessageTextScalar.DOScale(Vector3.zero, 0.3f));
            TurnMessageSequence.Insert(0, MessageCanvasGroup.DOFade(0, 0.2f));
        }

        public void DisplayPopupText(string Text/*, bool untillclosed = false*/)
        {
            Debug.Log(Text);
            TurnMessageSequence.Kill();
            StopAllCoroutines();
            /*if (untillclosed)
            {

                StartCoroutine(DisplayMessageTextRoutineUntillClosed(Text));
            }
            else
            {*/
                StartCoroutine(DisplayMessageTextRoutine(Text));
            // }
        }


        IEnumerator DisplayMessageTextRoutine(string TextToDisplay)
        {
            MessageText.text = TextToDisplay;
            MessageTextScalar.localScale = Vector3.zero;
            MessageCanvasGroup.alpha = 0;


            TurnMessageSequence.Append(MessageTextScalar.DOScale(Vector3.one, 0.3f));
            TurnMessageSequence.Insert(0, MessageCanvasGroup.DOFade(1, 0.2f));

            yield return new WaitForSeconds(1f + (TextToDisplay.Length * 0.05f));

            TurnMessageSequence.Append(MessageTextScalar.DOScale(Vector3.zero, 0.3f));
            TurnMessageSequence.Insert(0, MessageCanvasGroup.DOFade(0, 0.2f));

        }
        //
        // IEnumerator DisplayMessageTextRoutineUntillClosed(string TextToDisplay)
        // {
        //     MessageText.text = TextToDisplay;
        //     MessageTextScalar.localScale = Vector3.zero;
        //     MessageCanvasGroup.alpha = 0;
        //
        //
        //     TurnMessageSequence.Append(MessageTextScalar.DOScale(Vector3.one, 0.3f));
        //     TurnMessageSequence.Insert(0, MessageCanvasGroup.DOFade(1, 0.2f));
        //
        //     yield return null;
        //
        // }
    }
    
    // public class PopupEventArgs : EventArgs
    // {
    //     public string DisplayText{ get; }
    //
    //     public PopupEventArgs(string dislayText)
    //     {
    //         DisplayText = dislayText;
    //     }
    // }
}
