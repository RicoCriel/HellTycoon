using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MachineInput : MonoBehaviour
{
    private BoxCollider _collider;
    private UnityEvent _activated;
    [SerializeField] private string _demonTag;

    public bool Open = true;

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<BoxCollider>();

        _activated = new UnityEvent();
    }

    public void Initialize(MachineManager machine)
    {
        _activated.AddListener(machine.StartProduction);
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
