using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleSceneUI : MonoBehaviour {

	public Text MoneyText;
	public Transform BuildPanel;
	public FreeBuild.FreeBuildManager CM;

	void Start () 
	{
		AddButtonEventListener();
	}

    public void OnOffPanel()
	{
		BuildPanel.gameObject.SetActive(!BuildPanel.gameObject.activeSelf);
	}

	private void AddButtonEventListener()
	{
		for(int i = 0; i < BuildPanel.childCount; ++i)
		{
			Button btn = BuildPanel.GetChild(i).GetComponent<Button>();

			btn.onClick.AddListener(delegate {SetListener(btn.name);});
		}
	}

	private void SetListener(string objName)
	{
		switch(objName)
		{
			case "Table1":
				CM.CreateGhostObject("table1");
				break;
			case "Portal":
				CM.CreateGhostObject("Portal");
				break;
			case "Crown":
				CM.CreateGhostObject("crown");
				break;
			case "Apple":
				CM.CreateGhostObject("apple");
				break;
		}

		OnOffPanel();
	}
}
