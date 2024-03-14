using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] GameObject Portal;

    [SerializeField] Transform TestingLayer1;
    [SerializeField] Transform TestingLayer2;

    private GameObject portal1;
    private GameObject portal2;


    private void Start()
    {
        //TODO : Remove Testing Code
        //PlacePortal(new Vector3(-0.32f, 0.86f, -0.46f), TestingLayer1, TestingLayer2);
    }

    public void PlacePortal(Vector3 pos, Transform currLayer, Transform nextLayer)
    {
        
        portal1 =  Instantiate(Portal, Vector3.zero, Quaternion.identity);
        portal1.transform.SetParent(currLayer);
        portal1.transform.localPosition = pos;
        portal2 = Instantiate(Portal, Vector3.zero, Quaternion.identity);
        portal2.transform.SetParent(nextLayer);
        portal2.transform.localPosition = pos;
        
        portal1.GetComponent<DemonPortal>().ExitPortal = portal2.transform;
        portal2.GetComponent<DemonPortal>().ExitPortal = portal1.transform;

    }

}
