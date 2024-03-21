using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class BuildSideUI : MonoBehaviour
{
    private BuildingData _data;
    private Button _button;

    public Image BuildingImage;
    public TMP_Text BuildingText;
    public static UnityAction<BuildingData> _onBuild;

    private void Start()
    {
        BuildingImage.sprite = null;
        BuildingImage.color = Color.clear;
        BuildingText.text = string.Empty;

        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    public void UpdateSideDisplay(BuildingData data)
    {
        _data = data;
        BuildingImage.sprite = _data.Icon;
        BuildingImage.color = Color.white;
        BuildingText.text = _data.DisplayName;
    }

    private void OnButtonClick()
    {
        _onBuild ? .Invoke(_data);
    }
}
