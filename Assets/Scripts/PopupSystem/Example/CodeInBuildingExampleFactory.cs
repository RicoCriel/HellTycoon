using PopupSystem.Inheritors;
using System;
using UnityEngine;
namespace PopupSystem
{
    public class CodeInBuildingExampleFactory : MonoBehaviour
    {
        //Basically Make a Variable of the type of popup you want in this class
        //Lets say its a default factory

        [Header("popup")]
        [SerializeField]
        private WorldSpacePopupFactory _popupFactory;
        [SerializeField]
        private BuildingPopupActivator _popupActivator;

        private void Awake()
        {
            //nullcheck and find object
            
            _popupActivator.SetPopupper(_popupFactory);
            _popupFactory.DestroyButtonClicked += OnDestroyButtonClicked;
            _popupFactory.SoulProcessingPaused += OnSoulProcessingPaused;
            _popupFactory.SoulProcessingResumed += OnSoulProcessingResumed;
        }
        
            
        //onenable is also an option instead of awake, might be better for poolng
        // private void OnEnable()
        // {
        //     _popupActivator.SetPopupper(_popupFactory);
        //     _popupFactory.DestroyButtonClicked += OnDestroyButtonClicked;
        //     _popupFactory.SoulProcessingPaused += OnSoulProcessingPaused;
        //     _popupFactory.SoulProcessingResumed += OnSoulProcessingResumed;
        // }

        private void OnDisable()
        {
            _popupFactory.DestroyButtonClicked -= OnDestroyButtonClicked;
            _popupFactory.SoulProcessingPaused -= OnSoulProcessingPaused;
            _popupFactory.SoulProcessingResumed -= OnSoulProcessingResumed;
        }

        public void StartUIProcessing(int amount, float time)
        {
            _popupFactory.SetUnprocessedSoulsCounter(amount);
            _popupFactory.ProcessSoulBarUI(time);
        }
        
        //alot of other methods...

        public void DoneUIProcessing()
        {
            _popupFactory.SetUnprocessedSoulsCounter(0);
        }

        public void PopupForceClose()
        {
            //manually call closing
            _popupFactory.ClosePopup();
            //or (this might open if closed)
            _popupActivator.PopupActivatorClicked();
        }

        private void OnSoulProcessingPaused(object sender, Inheritors.PopupClickedEventArgs e)
        {
            Debug.Log("Soul Processing Paused");
        }
        private void OnSoulProcessingResumed(object sender, Inheritors.PopupClickedEventArgs e)
        {
            Debug.Log("Soul Processing Resumed");
        }
        private void OnDestroyButtonClicked(object sender, PopupClickedEventArgs e)
        {
            Destroy(gameObject);
            Debug.Log("Destroy Button Clicked");
        }

    }

    public class CodeInBuildingExampleBase : MonoBehaviour
    {
        //Basically Make a Variable of the type of popup you want in this class
        //Lets say this is the abse implementation
        [Header("popup")]
        [SerializeField]
        private WorldSpacePopupBase _popupBase;
        [SerializeField]
        private BuildingPopupActivator _popupActivator;

        private void Awake()
        {
            _popupActivator.SetPopupper(_popupBase);
            _popupBase.DestroyButtonClicked += OnDestroyButtonClicked;
        }
        
        //onenable is also an option instead of awake, might be better for poolng
        // private void OnEnable()
        // {
        //     _popupActivator.SetPopupper(_popupBase);
        //     _popupBase.DestroyButtonClicked += OnDestroyButtonClicked;
        // }

        private void OnDisable()
        {
            _popupBase.DestroyButtonClicked -= OnDestroyButtonClicked;
        }


        public void PopupForceClose()
        {
            //manually call closing
            _popupBase.ClosePopup();
            //or (this might open if closed)
            _popupActivator.PopupActivatorClicked();
        }

        private void OnDestroyButtonClicked(object sender, PopupClickedEventArgs e)
        {
            Destroy(gameObject);
            Debug.Log("Destroy Button Clicked");
        }

    }
}
