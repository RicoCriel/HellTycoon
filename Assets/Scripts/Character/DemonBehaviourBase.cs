using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class DemonBehaviourBase : MonoBehaviour 
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Material newMaterial = new Material(CoreUtils.CreateEngineMaterial("Universal Render Pipeline/Lit"));
        newMaterial.color = Color.black;
        List<Renderer> renderers = gameObject.GetComponentsInChildren<Renderer>().ToList<Renderer>();
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].sharedMaterial = newMaterial;
        }
    }
}
