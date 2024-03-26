using Dreamteck.Splines.Editor;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace PopupSystem
{
    public class BuildingPopupActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        //MyBuilding
        private Splines.Drawing.SplineDrawer splineDrawer;

        [SerializeField]
        protected WorldSpacePopupBase _popup;

        [SerializeField] private float _popupDelay = 0.8f;


        private Coroutine PopupSpawningRoutine;
        
        public void SetPopupper(WorldSpacePopupBase popper)
        {
            _popup = popper;
        }

        private void Awake()
        {
            if (splineDrawer == null)
            {
                splineDrawer = FindObjectOfType<Splines.Drawing.SplineDrawer>();
            }
        }

        private bool IsSplineDrawingActive()
        {
            return splineDrawer.HasStartedDrawing;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            //PopupActivatorHovered();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
           //on exit zorgt voor hoofdpijn met clickable UI
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupActivatorClicked();
        }
        
        public void PopupActivatorHovered()
        {

            if (IsSplineDrawingActive()) { return; }

            if (!_popup.IsPopupActive())
            {

                if (PopupSpawningRoutine != null)
                {
                    StopCoroutine(PopupSpawningRoutine);
                    PopupSpawningRoutine = null;
                }

                //track currently active popup and deactivate others
                PopupInstanceTracker.CurrentPopupInstance = _popup;

                if (PopupSpawningRoutine == null)
                {
                    PopupSpawningRoutine = StartCoroutine(SpawnPopupRoutine());
                }
            }
        }


        public void PopupActivatorClicked()
        {

            if (IsSplineDrawingActive()) { return; }

            if (PopupSpawningRoutine != null)
            {
                StopCoroutine(PopupSpawningRoutine);
                PopupSpawningRoutine = null;
                PopupInstanceTracker.CurrentPopupInstance = null;
            }

            if (_popup.IsPopupActive())
            {
                DeSpawnPopup();
                PopupInstanceTracker.CurrentPopupInstance = null;
            }
            else
            {
                if (!_popup.IsPopupTweeningOpen())
                {
                    PopupInstanceTracker.CurrentPopupInstance = _popup;
                    SpawnPopup();
                }
            }
        }

        private IEnumerator SpawnPopupRoutine()
        {
            yield return new WaitForSeconds(_popupDelay);

            if (!_popup.IsPopupActive())
            {
                SpawnPopup();
            }

            yield return null;
        }

        private void SpawnPopup()
        {
            _popup.SpawnPopup();
        }

        private void DeSpawnPopup()
        {
            _popup.ClosePopup();

        }
    }
}
