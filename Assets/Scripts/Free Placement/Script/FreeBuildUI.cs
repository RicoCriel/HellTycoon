using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FreeBuild
{
	public class FreeBuildUI : MonoBehaviour 
	{
		[SerializeField] private Transform _buildPanel;
		public delegate void Event();
		public Event Build;
		public Event Cancel;
		public Event RotateR;
        public Event RotateL;

        public void EnableUI()
		{
			_buildPanel.gameObject.SetActive(true);
		}

		public void DisableUI()
		{
			_buildPanel.gameObject.SetActive(false);
		}

		//private void Start()
		//{
		//	AddButtonEventListener();
		//}

		//private void AddButtonEventListener()
		//{
		//	for (int i = 0; i < _buildPanel.childCount; ++i)
		//	{
		//		Button btn = _buildPanel.GetChild(i).GetComponent<Button>();

		//		btn.onClick.AddListener(delegate { SetListener(btn.name); });
		//	}
		//}

		//private void SetListener(string objName)
		//{
		//	switch (objName)
		//	{
		//		case "Build":
		//			Build();
		//			DisableUI();
		//			break;
		//		case "Rotate":
		//			RotateR();
		//			break;
		//		case "RotateL":
		//			RotateL();
		//			break;
		//		case "Cancel":
		//			Cancel();
		//			DisableUI();
		//			break;
		//	}
		//}
	}
}