using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace PopupSystem
{
    public class BuildingPopupActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        //MyBuilding

        [SerializeField]
        private WorldSpacePopupBase _popup;

        [SerializeField] private float _popupDelay = 0.8f;


        private Coroutine PopupSpawningRoutine;

        private void Awake()
        {
            _popup.DestroyButtonClicked += DestroyBuilding;
        }
        private void DestroyBuilding(object sender, PopupClickedEventArgs e)
        {
            Destroy(this.gameObject);
        }

        private void OnDisable()
        {
            _popup.DestroyButtonClicked -= DestroyBuilding;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
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

        public void OnPointerExit(PointerEventData eventData)
        {
            // Debug.Log("Pointer Exit");
            // if (PopupSpawningRoutine != null)
            // {
            //     StopCoroutine(PopupSpawningRoutine);
            //     PopupSpawningRoutine = null;
            // }
            // DeSpawnPopup();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (PopupSpawningRoutine != null)
            {
                StopCoroutine(PopupSpawningRoutine);
                PopupSpawningRoutine = null;
            }

            if (_popup.IsPopupActive())
            {
                DeSpawnPopup();
            }
            else
            {
                if (!_popup.IsPopupTweeningOpen())
                {
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
