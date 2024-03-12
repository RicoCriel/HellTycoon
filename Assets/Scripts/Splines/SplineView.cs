using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineView : MonoBehaviour
{
    private SplineComputer _mySplineComputer;
    private SplineMesh _mySplineMesh;

    private void Awake()
    {
        _mySplineComputer = GetComponent<SplineComputer>();
        _mySplineMesh = GetComponent<SplineMesh>();
    }

}
