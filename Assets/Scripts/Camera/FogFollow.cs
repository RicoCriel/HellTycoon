using System.Collections.Generic;
using UnityEngine;

public class FogFollow : MonoBehaviour
{
    [SerializeField] private GameObject FogPlane;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private List<Material> layerColors;

    private int currentColorIndex = 0;
    private Color targetColor;

    private void Start()
    {
        ChangeColor(0);
    }
    void Update()
    {
        // Update position of FogPlane to follow the camera
        FogPlane.transform.position = new Vector3(0, MainCamera.transform.position.y - 50, 0);

        // Ensure currentColorIndex is within bounds of layerColors list
        currentColorIndex = Mathf.Clamp(currentColorIndex, 0, layerColors.Count - 1);

        // Calculate lerped color
        Color currentColor = FogPlane.GetComponent<Renderer>().material.color;
        Color lerpedColor = Color.Lerp(currentColor, targetColor, Time.deltaTime*3);

        // Set the fog color property of the material
        FogPlane.GetComponent<Renderer>().material.color = lerpedColor;
    }

    public void ChangeColor(int index)
    {
        currentColorIndex = index;
        targetColor = layerColors[currentColorIndex].color;
    }
}
