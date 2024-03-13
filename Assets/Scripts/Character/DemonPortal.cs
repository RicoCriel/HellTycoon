using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform ExitPortal;
    [SerializeField] Vector3 ExitRotation;
    [SerializeField] Vector3 Offset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon"))
        {
            other.transform.position = ExitPortal.position + Offset;
        }


        else if (!other.CompareTag("Player"))
        { return; }
        else
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = ExitPortal.position + Offset;
            other.GetComponent<CharacterController>().enabled = true;

        }
    }
}
