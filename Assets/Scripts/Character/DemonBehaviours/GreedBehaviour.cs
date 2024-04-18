using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class GreedBehaviour : DemonBehaviourBase
{
    [SerializeField] private Material _testMat;
    // Start is called before the first frame update
    protected override void Start()
    {
        Material newMaterial = new Material(CoreUtils.CreateEngineMaterial("Universal Render Pipeline/Lit"));
        newMaterial.color = Color.white;
        List<Renderer> renderers = gameObject.GetComponentsInChildren<Renderer>().ToList<Renderer>();
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sharedMaterial = newMaterial;
        }
    }
}

