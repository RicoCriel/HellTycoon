using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FreeBuild
{
	public class FreeBuildUi : MonoBehaviour 
	{
		public Transform BuildPanel;
		public delegate void Event();
		public Event Build;
		public Event Cancel;
		public Event Rotate;
        public Event RotateL;

        public void OnUI()
		{
			BuildPanel.gameObject.SetActive(true);
		}

		public void OffUi()
		{
			BuildPanel.gameObject.SetActive(false);
		}

		private void Start()
		{
			AddButtonEventListener();
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
				case "Build":
					Build();
					OffUi();
					break;
				case "Rotate":
					Rotate();
					break;
                case "RotateL":
                    RotateL();
                    break;
                case "Cancel":
					Cancel();
					OffUi();			
					break;
			}
		}
	}
}