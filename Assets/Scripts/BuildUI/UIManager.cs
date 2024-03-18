using System.Collections;
using System.Collections.Generic;
using FreeBuild;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BuildingPanelUI _buildingPanel;
    public static UnityAction<bool> Enable;

    private void Start()
    {
        _buildingPanel.gameObject.SetActive(false);
        //SetMouseCursorState(BuildingPanel.gameObject.activeInHierarchy);
    }

    private void Update()
    {
    }

    public void ShowUI()
    {
        var desiredPanelState = !_buildingPanel.gameObject.activeInHierarchy;
        _buildingPanel.gameObject.SetActive(desiredPanelState);
        if(desiredPanelState) _buildingPanel.PopulateButtons();
        //SetMouseCursorState(desiredPanelState);
        Enable ? .Invoke(!desiredPanelState);
    }

    private void SetMouseCursorState(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
