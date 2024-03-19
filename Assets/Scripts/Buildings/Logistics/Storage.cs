using Splines;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private Transform _output;
    [SerializeField] private Transform _input;
    [SerializeField] private PlaceholderConnectorHitBox _outputHitbox;
    [SerializeField] private PlaceholderConnectorHitBox _inputHitbox;
    [SerializeField] private bool _shouldStore = true;
    
    private List<GameObject> _items;

    private void Start()
    {
        _items = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            if (_shouldStore)
            {
                _items.Add(other.gameObject);
            }
            else
            {
                other.transform.position = _output.position;
                _outputHitbox.SpawnObject(other.gameObject);
            }
        }
    }
}


