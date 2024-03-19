using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreeBuild
{
	public class FreeBuildObject : MonoBehaviour 
	{
		public void SetObjectTransparent(Color color, Material transParentMaterial)
		{
			MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				color.a = 0.5f;
				transParentMaterial.color = color;				
				meshRenderer.material = transParentMaterial;
			}
		}
	}
}
