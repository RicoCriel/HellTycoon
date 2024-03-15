using UnityEngine;
using System.Collections.Generic;

public class LandLayerManager : MonoBehaviour
{

    public List<GameObject> AllPlots = new List<GameObject>();

    [SerializeField] private GameObject _prefab;
    [SerializeField] private EconManager _econManager;
    [SerializeField] private int _LandPrice = 50;

    private int _step = 1;
    private int _landOffsetStep = 500;

    void Start()
    {
        // Add the initial plot of land
        GameObject initialPlot = GameObject.FindGameObjectWithTag("Map");
        if (initialPlot != null)
        {
            AllPlots.Add(initialPlot);
        }
    }

    public GameObject GetCurrPlot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.CompareTag("Map"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public GameObject NextPlot(GameObject obj)
    {
        
        int index = AllPlots.IndexOf(obj);
        if (AllPlots.Count <= index +1)
        {
            return null;
        }
            return AllPlots[index + 1];
    }
    
    public void BuyNew()
    {
        if (  _econManager.GetMoney() >= _LandPrice)
        {
            _econManager.SubtractMoney(_LandPrice);
            GameObject newPlot = Instantiate(_prefab, new Vector3(0 + _landOffsetStep * _step, 0, 0), Quaternion.identity);
            AllPlots.Add(newPlot);
            ++_step;
        }
    }

    private void Update()
    {
       
       
    }

   
}