using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject _portal;

    [SerializeField] private Transform _testingLayer1;
    [SerializeField] private Transform _testingLayer2;

    private GameObject _portal1;
    private GameObject _portal2;


    private void Start()
    {
        //TODO : Remove Testing Code
        //PlacePortal(new Vector3(-0.32f, 0.86f, -0.46f), TestingLayer1, TestingLayer2);
    }

    public void PlacePortal(Vector3 pos, Transform currLayer, Transform nextLayer)
    {
        _portal1 =  Instantiate(_portal, Vector3.zero, Quaternion.identity);
        _portal1.transform.SetParent(currLayer);
        _portal1.transform.localPosition = pos;
        _portal2 = Instantiate(_portal, Vector3.zero, Quaternion.identity);
        _portal2.transform.SetParent(nextLayer);
        _portal2.transform.localPosition = pos;

        _portal1.GetComponent<DemonPortal>().ExitPortal = _portal2.GetComponent<DemonPortal>();
        _portal2.GetComponent<DemonPortal>().ExitPortal = _portal1.GetComponent<DemonPortal>();

    }

}
