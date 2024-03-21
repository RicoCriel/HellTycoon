using DG.Tweening;
using System;
using UnityEngine;
namespace PopupSystem
{
    public class WorldSpacePopupBase : MonoBehaviour
    {
        [SerializeField]
        protected Canvas _popupCanvas;
        [SerializeField]
        protected Transform _popupTransform;

        private Camera _mainCamera;

        private Tween _currentTween;

        protected virtual void Awake()
        {
            if (_popupTransform == null)
                _popupTransform = transform.GetChild(0);

            if (_popupCanvas == null)
                _popupCanvas = transform.GetChild(0).GetComponent<Canvas>();
                                
            //find maincam
            _mainCamera = Camera.main;
        
            //hook up canves to camera
            _popupCanvas.worldCamera = _mainCamera;
        
            AnglePopupToCamera();
        
            _popupTransform.localScale = Vector3.zero;
            _popupTransform.gameObject.SetActive(false);
        }
    
        public bool IsPopupActive()
        {
            return _popupTransform.gameObject.activeSelf;
        }
        
        public bool IsPopupTweeningOpen()
        {
            return _currentTween != null && _currentTween.IsActive();
        }

        //only angled once since it doesnt move?
        private void AnglePopupToCamera()
        {
            if (_mainCamera != null)
            {
                Quaternion parentRotation = transform.parent.rotation;
                Quaternion cameraRotation = _mainCamera.transform.rotation;

                Quaternion combinedRotation = parentRotation * cameraRotation;
                Vector3 eulerRotation = combinedRotation.eulerAngles;

                _popupTransform.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
            }
            else
            {
                Debug.LogWarning("Main camera not found. Make sure there is an active camera in the scene.");
            }
        }

        //Methods to Call in other scripts
        [HideInInspector]
        public void SpawnPopup()
        {
            killCurrentTween();
            _popupTransform.localScale = Vector3.zero;
            _popupTransform.gameObject.SetActive(true);

            //tween using doTween
            _currentTween = _popupTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        [HideInInspector]
        public void ClosePopup()
        { 
            killCurrentTween();
            _popupTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => _popupTransform.gameObject.SetActive(false));
        }
    
        private void killCurrentTween()
        {
            if (_currentTween != null)
            {
                _currentTween.Kill();
            }
        }

        //Methods for buttons

        public void ButtonClose()
        {
            ClosePopup();
        }

        public void ButtonDestroy()
        {
            OnDestroyButtonClicked(new PopupClickedEventArgs());
        }

        public event EventHandler<PopupClickedEventArgs> DestroyButtonClicked;

        private void OnDestroyButtonClicked(PopupClickedEventArgs e)
        {
            EventHandler<PopupClickedEventArgs> handler = DestroyButtonClicked;
            handler?.Invoke(this, e);
        }
    }

    public class PopupClickedEventArgs : EventArgs
    {
        public PopupClickedEventArgs() { }
    }
}