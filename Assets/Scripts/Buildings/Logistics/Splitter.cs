using Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spliiter : MonoBehaviour
{
    [SerializeField] private Transform _output1;
    [SerializeField] private Transform _output2;
    [SerializeField] private PlaceholderConnectorHitBox _outputHitbox1;
    [SerializeField] private PlaceholderConnectorHitBox _outputHitbox2;
    [SerializeField] private PlaceholderConnectorHitBox _inputHitbox;

    private bool _goRight = false;


    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Resource"))
            {
                if (_goRight)
                {
                    other.transform.position = _output1.position;
                    _outputHitbox1.SpawnObject(other.gameObject);
                }
                else
                {
                    other.transform.position = _output2.position;
                    _outputHitbox2.SpawnObject(other.gameObject);
                }
                _goRight = !_goRight;
            }
        }
    }

}
