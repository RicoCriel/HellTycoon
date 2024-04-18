using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LandLayerManager : MonoBehaviour
{

    public List<GameObject> AllLayers = new List<GameObject>();

    [SerializeField] private LayerMask _ignoreLayer;


    private int _step = 1;
    private int _landOffsetStep = 500;

    private GameObject _currPlot;

    void Start()
    {
        //// Add the initial plot of land
        //GameObject initialPlot = GameObject.FindGameObjectWithTag("Map");
        //if (initialPlot != null)
        //{
        //    AllLayers.Add(initialPlot);
        //}
        _currPlot = AllLayers[0];
    }

    public GameObject GetCurrPlot()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit,1000f,~_ignoreLayer))
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //    if (hit.collider.transform.CompareTag("Map"))
        //    {
        //        return hit.collider.gameObject;
        //    }
        //}
        //return null;
        return _currPlot;
    }

    public GameObject NextPlot(GameObject obj)
    {
        
        int index = AllLayers.IndexOf(obj);
        if (AllLayers.Count <= index +1)
        {
            return null;
        }
            return AllLayers[index + 1];
    }


    public void SetPlot(GameObject obj)
    {
        _currPlot = obj;
    }

    private void Update()
    {
       
       
    }

   
}
