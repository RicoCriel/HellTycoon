using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class Snapper : MonoBehaviour
{
    private SphereCollider _radiusCollider;
    private BoxCollider _snappingCollider;

    private void Awake()
    {
        _snappingCollider = GetComponent<BoxCollider>();
        _radiusCollider = GetComponent<SphereCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        var myClosestPoint = _snappingCollider.ClosestPoint(collision.collider.transform.position);
        var targetClosestPoint = collision.collider.ClosestPoint(myClosestPoint);
        Vector3 offset = targetClosestPoint - myClosestPoint;
        
        collision.gameObject.transform.position += offset;
    }
}
