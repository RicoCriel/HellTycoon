using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MachineInput : MonoBehaviour
{
    [SerializeField] private string _demonTag;
    [SerializeField] private MachineManager _machineManagerPrefab;
    
    private BoxCollider _collider;
    private UnityEvent _activated;
    

    public bool Open = true;

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        _activated = new UnityEvent();

        var machineManager = Instantiate(_machineManagerPrefab);
        machineManager.Initialize(this);
        _activated.AddListener(machineManager.StartProduction);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == _demonTag)
        {
            _activated.Invoke();
            Open = false;
        }

    }
}
