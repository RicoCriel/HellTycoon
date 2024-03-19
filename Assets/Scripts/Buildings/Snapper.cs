using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Snapper : MonoBehaviour
{
    [SerializeField] private Transform[] snapPoints;
    [SerializeField] private float _snapRange = 5.5f;
    [SerializeField] private LayerMask snapLayer;


    private Transform _closestSnapPoint = null;
    [SerializeField] private bool _isPlaced = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == snapLayer)
        {
            // Check if snapping is possible
            if (CanSnapTo(other.gameObject))
            {
                // Perform snapping
                SnapTo(other.gameObject);
            }
        }
    }

    bool CanSnapTo(GameObject target)
    {
        float closestDistance = float.MaxValue;
        

        foreach (Transform snapPoint in snapPoints)
        {
            Vector3 snapPointWorldPos = transform.TransformPoint(snapPoint.localPosition);

            foreach (Transform targetSnapPoint in target.GetComponent<Snapper>().snapPoints)
            {
                Vector3 targetSnapPointWorldPos = target.transform.TransformPoint(targetSnapPoint.localPosition);

                float distance = Vector3.Distance(snapPointWorldPos, targetSnapPointWorldPos);

                if (distance < _snapRange && distance < closestDistance)
                {
                    closestDistance = distance;
                    _closestSnapPoint = snapPoint;
                }
            }
        }

        if (_closestSnapPoint != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SnapTo(GameObject target)
    {
        if(!_isPlaced)
        {
         _isPlaced = true;
        transform.parent.transform.position = _closestSnapPoint.position;

        }
    }
}

