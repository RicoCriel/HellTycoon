using UnityEngine;

public class DemonPortal : MonoBehaviour
{
    
    [SerializeField] private Vector3 _exitRotation;
    [SerializeField] private Vector3 _offset;
    public Transform ExitPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon"))
        {
            other.transform.position = ExitPortal.position + _offset;
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
}
