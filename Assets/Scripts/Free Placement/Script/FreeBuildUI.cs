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
	}
}