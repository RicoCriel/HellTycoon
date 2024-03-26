using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BuildingPanelUI : MonoBehaviour
{
    [SerializeField] private BuildSideUI _sideUI;
    public static UnityAction<BuildingData> _onPartChosen;

    [SerializeField] private BuildingData[] _knownBuildingParts;
    [SerializeField] private BuildingPartUI _buildingButtonPrefab;

    [SerializeField] private GameObject _itemWindow;
    [SerializeField] private UnityEvent _onPartClick;

    public void OnHover(BuildingData chosenData)
    {
        _sideUI.UpdateSideDisplay(chosenData);
    }

    public void OnClick(BuildingData chosenData)
    {
        _onPartChosen?.Invoke(chosenData);
        _onPartClick?.Invoke();
    }

    public void OnClickAllParts()
    {
        PopulateButtons();
    }

    public void OnClickManufacturingParts()
    {
        PopulateButtons(PartType.Manufacturing);
    }

    public void OnClickPowerParts()
    {
        PopulateButtons(PartType.Power);
    }

    public void OnClickParts(int type)
    {
        PopulateButtons((PartType)type);
    }

    public void PopulateButtons()
    {
        SpawnButtons(_knownBuildingParts);
    }

    public void PopulateButtons(PartType chosenPartType)
    {
        var buildings = _knownBuildingParts.Where(p=> p.PartType == chosenPartType).ToArray();
        SpawnButtons(buildings);
    }

    public void SpawnButtons(BuildingData[] buttonData)
    {
        ClearButtons();

        foreach (var data in buttonData)
        {
            var spawnedButton = Instantiate(_buildingButtonPrefab, _itemWindow.transform);
            spawnedButton.Init(data, this);
        }
    }

    public void ClearButtons()
    {
        foreach (var button in _itemWindow.transform.Cast<Transform>())
        {
            Destroy(button.gameObject);
        }
    }
}
