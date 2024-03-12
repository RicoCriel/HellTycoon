using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FreeBuild
{
	public class FreeBuildUI : MonoBehaviour 
	{
		public Transform buildPanel;
		public delegate void Event();
		public Event Build;
		public Event Cancel;
		public Event Rotate;
        public Event RotateL;

        public void OnUI()
		{
			buildPanel.gameObject.SetActive(true);
		}

		public void OffUI()
		{
			buildPanel.gameObject.SetActive(false);
		}

		private void Start()
		{
			AddButtonEventListener();
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
				case "Build":
					Build();
					OffUI();
					break;
				case "Rotate":
					Rotate();
					break;
                case "RotateL":
                    RotateL();
                    break;
                case "Cancel":
					Cancel();
					OffUI();			
					break;
			}
		}
	}
}