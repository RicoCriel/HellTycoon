using UnityEngine;
using System.Collections;

public class BasicMachine : MonoBehaviour
{
    [SerializeField] float ProcessTime = 2.0f; // Delay in seconds
    [SerializeField] GameObject inputLoc;
    [SerializeField] GameObject outputLoc;
    private float _timer = 0.0f;
    private bool inputReceived = false;
    private bool _processing = false;
    private GameObject _inputObject;
    private GameObject _outputObject;

    // Update is called once per frame
    void Update()
    {
        // Check for input
        // Detect input object here and save it to _inputObject
        // inputobject to determine what the output object should be and save it in _outputObject
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputReceived = true;
            Debug.Log("Input received!");
        }

        // Process input after delay
        if (inputReceived && !_processing)
        {
            _timer += Time.deltaTime;

            if (_timer >= ProcessTime)
            {
                ProcessInput();
                inputReceived = false;
                _processing = false;
                _timer = 0.0f;
            }
        }
    }

    void ProcessInput()
    {
        //output the _outputObject
        _processing = true;
        Debug.Log("fork knife");
    }
    
    
}
