using Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MonoBehaviour
{
    [SerializeField] private Transform _input1;
    [SerializeField] private Transform _input2;
    [SerializeField] private Transform _output;
    [SerializeField] private PlaceholderConnectorHitBox _inputHitbox1;
    [SerializeField] private PlaceholderConnectorHitBox _inputHitbox2;
    [SerializeField] private PlaceholderConnectorHitBox _outputHitbox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            other.transform.position = _output.position;
            _outputHitbox.SpawnObject(other.gameObject);
        }
    }
}
