using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeBuild
{
	public class FreeBuildManager : MonoBehaviour 
	{
		// Add Obejct to be built to list
		public List<GameObject> constructionItems = new List<GameObject>();

		// can select outline Color.
		public Color ableAreaColor = new Color(0,255,0);
		public Color notAbleAreaColor = new Color(255,0,0);

		public Material transParentMaterial;
		public FreeBuildUI uiManager;
		public GameObject rootObject;
		public static bool constructionMode = false;

		//
		private GameObject ghostObject;
		private GameObject realObject;


		//
		public void CreateGhostObject(string objName)
		{
			realObject = constructionItems.Find( x => x.name == objName);
			if(null == realObject)
			{
				Debug.LogError("You have to list the objects you're trying to build on.");
				return;
			}
			if(null == realObject.GetComponent<FreeBuildObject>())
			{
				Debug.LogError("The object you are trying to build must have a ConstructionObject Component.");
				return;
			}

			if(ghostObject)
				Destroy(ghostObject);

			//
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
			RaycastHit hit;
			bool isHit = Physics.Raycast(ray, out hit);

			// 
			if(isHit)
			{
				uiManager.OnUI();
				CreateGhostObject(hit);
				constructionMode = true;
			}
		}

		void Update()
		{
		#if UNITY_ANDROID	
			if(Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == null)
			{
				Touch touch = Input.GetTouch(0);
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;
				bool isHit = Physics.Raycast(ray, out hit);
				
				if (touch.phase == TouchPhase.Began && isHit && ghostObject)
				{				
					MoveGhostObject(hit);					
				}
			}
		#endif

		#if UNITY_EDITOR || UNITY_STANDALONE
			if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				bool isHit = Physics.Raycast(ray, out hit);

				if (isHit && ghostObject)
				{
					MoveGhostObject(hit);
				}
			}
            if (Input.GetKey(KeyCode.Q))
            {
                //rotate left
                ghostObject.transform.Rotate(0, 2.0f, 0);
            }
            if (Input.GetKey(KeyCode.E))
            {
                //rotate right
                ghostObject.transform.Rotate(0, -2.0f, 0);
            }
#endif
        }

        private void CreateGhostObject(RaycastHit hit)
		{
			ghostObject = Instantiate(realObject, new Vector3(hit.point.x, hit.point.y + GetObjectHeight(hit.transform), hit.point.z), Quaternion.identity);

			SetGhostOutLine(hit.transform.gameObject);
			SetUIEvent(hit.transform.gameObject);
		}

		private void MoveGhostObject(RaycastHit hit)
		{
			ghostObject.transform.position = new Vector3(hit.point.x, hit.point.y + GetObjectHeight(hit.transform), hit.point.z);

			SetGhostOutLine(hit.transform.gameObject);
			SetUIEvent(hit.transform.gameObject);
		}

		private void SetUIEvent(GameObject areaToBeBuilt)
		{
			uiManager.Build = delegate 
			{
				if(CanBuild(areaToBeBuilt, ghostObject))
				{
					GameObject go = Instantiate(realObject, ghostObject.transform.position, ghostObject.transform.rotation);
					if(rootObject)
						go.transform.SetParent(rootObject.transform);
				}
				else
				{
					Debug.LogWarning("you can't build in impossible area");
				}
				GameObject.Destroy(ghostObject);
				constructionMode = false;
			};

			uiManager.Rotate = delegate 
			{
				ghostObject.transform.Rotate(0, .0f, 0);
			};

            uiManager.RotateL = delegate
            {
                ghostObject.transform.Rotate(0, -5.0f, 0);
            };

            uiManager.Cancel = delegate 
			{ 
				GameObject.Destroy(ghostObject);
				constructionMode = false;
			};
		}

		// Show Obejct to be built
		private void SetGhostOutLine(GameObject areaToBeBuilt)
		{
			Color color = CanBuild(areaToBeBuilt, ghostObject) ? ableAreaColor : notAbleAreaColor;
			ghostObject.GetComponent<FreeBuildObject>().SetObjectTransparent(color, transParentMaterial);
		}

		private bool CanBuild(GameObject areaToBeBuilt, GameObject go)
		{
			return (areaToBeBuilt.transform.gameObject.tag == go.GetComponent<FreeBuildObject>().ConstructionAreaTagName);
		}

		private float GetObjectHeight(Transform tf)
		{
			if(tf.GetComponent<Collider>())
			{
				return tf.GetComponent<Collider>().bounds.size.y / 2;
			}
			else
			{
				return tf.transform.position.y;
			}
		}

	} // class ConstructionManager
}
