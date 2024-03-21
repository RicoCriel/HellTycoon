using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using FreeBuild;
using static Snapper;

[RequireComponent(typeof(BoxCollider))]
public class Snapper : MonoBehaviour
{
    public delegate void StartSnapping();
    public static event StartSnapping OnStartSnapping;

    public  Transform[] snapPoints;
    public float _snapRange = 5.5f;
    public LayerMask snapLayer;


    private Transform _closestSnapPoint = null;
    [SerializeField] public bool _isPlaced = false;



    private void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if ((snapLayer & (1 << other.gameObject.layer)) != 0)
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
        if (!_isPlaced)
        {

            OnStartSnapping();
            transform.position = (_closestSnapPoint.localPosition * -2) + target.transform.position;
            //transform.rotation = Quaternion.FromToRotation(transform.forward, -_closestSnapPoint.forward);
        }
    }
}
