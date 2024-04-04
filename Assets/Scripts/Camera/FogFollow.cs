using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject FogPlane;
    [SerializeField]
    private GameObject Camera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FogPlane.transform.position = new Vector3(0, Camera.transform.position.y - 50,0);
    }
}
