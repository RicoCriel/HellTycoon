using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class FearMonument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _searchRadius = 10f;
    [SerializeField] private float _fearApplyInterval = 3f;
    [SerializeField] private float _fearPerInterval = 5f;
    [SerializeField] private Material _indicatorMaterial;
    private GameObject _visualIndicator;

    [SerializeField] private GameObject _mesh;

    private float _fearApplyCounter;

    void Start()
    {
        
        
        // Create a sphere to represent the radius
        _visualIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _visualIndicator.transform.parent = transform;
        _visualIndicator.transform.localScale = new Vector3(_searchRadius * 2, _searchRadius * 2, _searchRadius * 2);
        _visualIndicator.transform.localPosition = Vector3.zero;

        // Adjust color and transparency
        _visualIndicator.GetComponent<Renderer>().material = _indicatorMaterial;
        _visualIndicator.gameObject.SetActive(false);


        CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.radius = GetComponentInChildren<CapsuleCollider>().radius * _mesh.transform.localScale.x * 2;
        capsuleCollider.height = GetComponentInChildren<CapsuleCollider>().height * _mesh.transform.localScale.x;
        _visualIndicator.layer = 2;

    }

    private void Update()
    {
        _visualIndicator.transform.localScale = new Vector3(_searchRadius * 2, _searchRadius * 2, _searchRadius * 2);

        _fearApplyCounter += Time.deltaTime;
        if (_fearApplyCounter >= _fearApplyInterval)
        {
            List<DemonBase> demons = FindGameObjectsInRangeWithScript();
            demons.ForEach(demon => demon.DemonFear.IncreaseFear(_fearPerInterval));
            _fearApplyCounter = 0;
        }
       

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _visualIndicator.gameObject.SetActive(true);
    }

    // Called when the mouse pointer exits the GameObject's collider
    public void OnPointerExit(PointerEventData eventData)
    {
        _visualIndicator.gameObject.SetActive(false);
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