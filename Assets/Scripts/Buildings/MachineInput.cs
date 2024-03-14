using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MachineInput : MonoBehaviour
{
    private BoxCollider _collider;
    private UnityEvent _activated;
    [SerializeField] private int _demonLayer;

    public bool Open;

    private void Awake()
    {
        if( _collider == null )
            _collider = GetComponent<BoxCollider>();
    }

    public void Initialize(MachineManager machine)
    {
        _activated.AddListener(machine.StartProduction);
    }

    void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.layer == _demonLayer)
           _activated.Invoke();
    }
}
