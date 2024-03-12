using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleSceneUI : MonoBehaviour {

	public Text money_text;
	public Transform buildPanel;
	public FreeBuild.FreeBuildManager cm;

	void Start () 
	{
		AddButtonEventListener();
	}

    public void OnOffPanel()
	{
		buildPanel.gameObject.SetActive(!buildPanel.gameObject.activeSelf);
	}

	private void AddButtonEventListener()
	{
		for(int i = 0; i < buildPanel.childCount; ++i)
		{
			Button btn = buildPanel.GetChild(i).GetComponent<Button>();

			btn.onClick.AddListener(delegate {SetListener(btn.name);});
		}
	}

	private void SetListener(string objName)
	{
		switch(objName)
		{
			case "Table1":
				cm.CreateGhostObject("table1");
				break;
			case "Table2":
				cm.CreateGhostObject("table2");
				break;
			case "Crown":
				cm.CreateGhostObject("crown");
				break;
			case "Apple":
				cm.CreateGhostObject("apple");
				break;
		}

		OnOffPanel();
	}
}
