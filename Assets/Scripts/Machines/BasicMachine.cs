using UnityEngine;
using System.Collections;

public class BasicMachine : MonoBehaviour
{
    [SerializeField] private float _processTime = 2.0f; // Delay in seconds
    [SerializeField] private GameObject _inputLoc;
    [SerializeField] private GameObject _outputLoc;
    private float _timer = 0.0f;
    private bool _inputReceived = false;
    private bool _processing = false;
    private GameObject _inputObject;
    private GameObject _outputObject;

    // Update is called once per frame
    private void Update()
    {
        // Check for input
        // Detect input object here and save it to _inputObject
        // inputobject to determine what the output object should be and save it in _outputObject
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _inputReceived = true;
            Debug.Log("Input received!");
        }

        // Process input after delay
        if (_inputReceived && !_processing)
        {
            _timer += Time.deltaTime;

            if (_timer >= _processTime)
            {
                ProcessInput();
                _inputReceived = false;
                _processing = false;
                _timer = 0.0f;
            }
        }
    }

    private void ProcessInput()
    {
        //output the _outputObject
        _processing = true;
        Debug.Log("fork knife");
    }
    
    
}
