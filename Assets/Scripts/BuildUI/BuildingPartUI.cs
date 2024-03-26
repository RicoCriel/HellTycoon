using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingPartUI : MonoBehaviour, IPointerEnterHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _parentDisplay.OnHover(_assignedData);
    }

    private void OnButtonClick()
    {
        _parentDisplay.OnClick(_assignedData);
    }
}
