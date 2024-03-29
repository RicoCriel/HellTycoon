using UnityEngine;

public class DemonPortal : MonoBehaviour
{
    
    [SerializeField] private Vector3 _exitRotation;
    [SerializeField] private Vector3 _offset;
    public Transform ExitPortal;
    private bool _isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        _isColliding = true;
        if (other.CompareTag("Demon"))
        {
            other.transform.position = ExitPortal.position + _offset;
            other.GetComponent<DemonHandler>().SetLayer();
        }


        else if (!other.CompareTag("Player"))
        { return; }
        else
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = ExitPortal.position + _offset;
            other.GetComponent<CharacterController>().enabled = true;
            

        }
         

  
    }
    private void OnTriggerExit(Collider other)
    {
        _isColliding = false;
    }

    public bool IsColliding()
    {
        return _isColliding;
    }
}
