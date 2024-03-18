using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPartUI : MonoBehaviour
{
    private Button _button;
    private BuildingData _assignedData;
    private BuildingPanelUI _parentDisplay;

    public void Init(BuildingData assignedData, BuildingPanelUI parentDisplay)
    {
        _assignedData = assignedData;
        _parentDisplay = parentDisplay;
        _button = GetComponentInChildren<Button>();
        _button.GetComponent<Image>().sprite = _assignedData.Icon;
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _parentDisplay.OnClick(_assignedData);
    }
}
