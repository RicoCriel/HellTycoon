using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private DemonHandler _demonHandler;

    [SerializeField] private DemonFear _demonFear;

    [SerializeField] private DemonSettings _demonSettings;

    [SerializeField] private TextMeshPro _fearCounter;

    public DemonBehaviourBase _demonBehaviourBase;

    private bool _isHovered;

    private Camera _mainCamera;
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public DemonHandler DemonHandler
    {
        get
        {
            return _demonHandler;
        }
    }

    private void Update()
    {
        if(_isHovered)
        {
            _fearCounter.SetText(_demonFear.FearLevel.ToString());
            AngleFearCounterToCam();
        }
    }

    public DemonFear DemonFear
    {
        get
        {
            return _demonFear;
        }
    }

    public DemonSettings DemonSettings
    {
        get
        {
            return _demonSettings;
        }
    }


    public void InitEffect(HellLayer hellLayer)
    {
        switch (hellLayer)
        {
            case HellLayer.Limbo:
                _demonBehaviourBase = new LimboBehaviour();
                break;
            case HellLayer.Lust:
                _demonBehaviourBase = new LustBehaviour();
                break;
            case HellLayer.Gluttony:
                _demonBehaviourBase = new GluttonyBehaviour();
                break;
            case HellLayer.Greed:
                _demonBehaviourBase = new GreedBehaviour();
                break;
            case HellLayer.Wrath:
                _demonBehaviourBase = new WrathBehaviour();
                break;
            case HellLayer.Heresy:
                _demonBehaviourBase = new HeresyBehaviour();
                break;
            case HellLayer.Violence:
                _demonBehaviourBase = new ViolenceBehaviour();
                break;
            case HellLayer.Fraud:
                _demonBehaviourBase = new FraudBehaviour();
                break;
            case HellLayer.Treachery:
                _demonBehaviourBase = new TreacheryBehaviour();
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
        _fearCounter.gameObject.SetActive(true);
    }

    // Called when the mouse pointer exits the GameObject's collider
    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        _fearCounter.gameObject.SetActive(false);
    }

    private void AngleFearCounterToCam()
    {
    //    if (_mainCamera != null)
    //    {
    //        //Quaternion parentRotation = transform.rotation;
    //        //Quaternion cameraRotation = _mainCamera.transform.rotation;

    //        //Quaternion combinedRotation = parentRotation * cameraRotation;
    //        //Vector3 eulerRotation = combinedRotation.eulerAngles;

    //        //_fearCounter.gameObject.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

    //        Vector3 lookAtDirection = _mainCamera.transform.position - transform.position;

    //        transform.rotation = Quaternion.LookRotation(lookAtDirection);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Main camera not found. Make sure there is an active camera in the scene.");
    //    }
    }
}
