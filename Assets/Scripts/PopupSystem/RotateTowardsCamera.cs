using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    // Reference to the main camera
    private Camera mainCamera;

    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Ensure we have a reference to the camera
        if (mainCamera == null)
            return;

        // Calculate the direction from the child GameObject to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Ensure the direction vector is not zero
        if (directionToCamera != Vector3.zero)
        {
            // Calculate the rotation needed to look at the camera
            Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);

            // Apply the rotation to the child GameObject
            transform.rotation = rotationToCamera;
        }
    }
}
