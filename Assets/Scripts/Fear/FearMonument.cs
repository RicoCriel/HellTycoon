using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearMonument : MonoBehaviour
{
    [SerializeField] private float _searchRadius = 10f;
    [SerializeField] private float _fearApplyInterval = 3f;
    [SerializeField] private float _fearPerInterval = 5f;

    void Start()
    {
        InvokeRepeating("ApplyFear", _fearApplyInterval, _fearApplyInterval);
    }

    private void Update()
    {
        List<DemonBase> demons = FindGameObjectsInRangeWithScript();
        demons.ForEach(demon => demon.DemonFear.IncreaseFear(_fearPerInterval));
    }

    private List<DemonBase> FindGameObjectsInRangeWithScript()
    {
        List<DemonBase> foundObjects = new List<DemonBase>();

        
            // Get all game objects within the search radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius);

            // Check each collider for the desired script
            foreach (Collider col in colliders)
            {
                // Check if the collider has a GameObject attached
                GameObject obj = col.gameObject;
                if (obj != null)
                {
                    // Check if the GameObject has the desired script attached
                    DemonBase script = obj.GetComponent<DemonBase>();
                    if (script != null)
                    {
                        // If the script is found, add the GameObject to the list
                        foundObjects.Add(script);
                    }
                }
            
        }

        return foundObjects;
    }
}