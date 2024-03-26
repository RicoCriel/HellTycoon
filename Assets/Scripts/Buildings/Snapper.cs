using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using FreeBuild;
using static Snapper;

public class Snapper : MonoBehaviour
{
    // Array to hold snap points. You can manually assign these in the Unity Editor.
    public Transform[] SnapPoints;

    // You might also include a layer for snap detection if needed, for more refined control
    public LayerMask SnapLayer;

    // This flag indicates whether the object has been placed (and thus should no longer try to snap to others)
    public bool IsPlaced = false;

    public bool IsColliding = false;

    private void Awake()
    {
        // Initialize snap points array if needed or perform any setup logic
        // For example, finding all child objects with a specific tag or component as snap points
    }

    // Any additional methods related to snapping logic could go here.
    // For instance, enabling or disabling visual indicators for snap points, if you have them.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Map")
        {
            IsColliding = true;
        }    
        
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Map")
        {
            IsColliding = false;
        }
    }

}








//[RequireComponent(typeof(BoxCollider))]
//public class Snapper : MonoBehaviour
//{
//    public delegate void StartSnapping();
//    public static event StartSnapping OnStartSnapping;

//    public  Transform[] snapPoints;
//    public float _snapRange = 5.5f;
//    public LayerMask snapLayer;


//    private Transform _closestSnapPoint = null;
//    [SerializeField] public bool _isPlaced = false;



//    private void Start()
//    {

//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if ((snapLayer & (1 << other.gameObject.layer)) != 0)
//        {
//            // Check if snapping is possible
//            if (CanSnapTo(other.gameObject))
//            {
//                // Perform snapping
//                SnapTo(other.gameObject);
//            }
//        }
//    }

//    bool CanSnapTo(GameObject target)
//    {
//        float closestDistance = float.MaxValue;


//        foreach (Transform snapPoint in snapPoints)
//        {
//            Vector3 snapPointWorldPos = transform.TransformPoint(snapPoint.localPosition);

//            foreach (Transform targetSnapPoint in target.GetComponent<Snapper>().snapPoints)
//            {
//                Vector3 targetSnapPointWorldPos = target.transform.TransformPoint(targetSnapPoint.localPosition);

//                float distance = Vector3.Distance(snapPointWorldPos, targetSnapPointWorldPos);

//                if (distance < _snapRange && distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    _closestSnapPoint = snapPoint;
//                }
//            }
//        }

//        if (_closestSnapPoint != null)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    void SnapTo(GameObject target)
//    {
//        if (!_isPlaced)
//        {

//            OnStartSnapping();
//            transform.position = (_closestSnapPoint.localPosition * -2) + target.transform.position;
//            //transform.rotation = Quaternion.FromToRotation(transform.forward, -_closestSnapPoint.forward);
//        }
//    }
//}
