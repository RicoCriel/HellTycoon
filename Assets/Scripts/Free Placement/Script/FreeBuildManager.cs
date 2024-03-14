using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeBuild
{
    public class FreeBuildManager : MonoBehaviour
    {
        // Add Object to be built to list
        public List<GameObject> constructionItems = new List<GameObject>();

        // can select outline Color.
        public Color ableAreaColor = new Color(0, 255, 0);
        public Color notAbleAreaColor = new Color(255, 0, 0);

        public Material transParentMaterial;
        public FreeBuildUI uiManager;
        public GameObject rootObject;
        public static bool constructionMode = false;
        [SerializeField] PortalManager portalManager;
        [SerializeField] LandLayerManager landLayerManager;
        [SerializeField] Transform Layer1;
        [SerializeField] Transform Layer2;
        //
        private GameObject ghostObject;
        private GameObject realObject;

        // Movement speed
        public float moveSpeed = 5.0f;
        // Rotation speed
        public float rotateSpeed = 100.0f;
        // Height change speed
        public float heightChangeSpeed = 1.0f;


        //
        public void CreateGhostObject(string objName)
        {
            realObject = constructionItems.Find(x => x.name == objName);
            //if (null == realObject)
            //{
            //    Debug.LogError("You have to list the objects you're trying to build on.");
            //    return;
            //}
            if (null == realObject.GetComponent<FreeBuildObject>())
            {
                Debug.LogError("The object you are trying to build must have a ConstructionObject Component.");
                return;
            }

            if (ghostObject)
                Destroy(ghostObject);

            //
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit);

            //
            if (isHit)
            {
                uiManager.OnUI();
                CreateGhostObject(hit);
                constructionMode = true;
            }
        }

       
            void Update()
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // Movement
                    Vector3 moveDirection = Vector3.zero;
                    if (Input.GetKey(KeyCode.W))
                    {
                        moveDirection += Camera.main.transform.up;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        moveDirection -= Camera.main.transform.up;
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        moveDirection -= Camera.main.transform.right;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        moveDirection += Camera.main.transform.right;
                    }

                    // Height change
                    float heightChange = 0.0f;
                    if (Input.GetKey(KeyCode.Space))
                    {
                        heightChange += 1.0f;
                    }
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        heightChange -= 1.0f;
                    }

                    if (ghostObject)
                    {
                        // Move
                        ghostObject.transform.position += moveDirection * Time.deltaTime * moveSpeed;

                        // Change height
                        Vector3 newPosition = ghostObject.transform.position;
                        newPosition.y += heightChange * Time.deltaTime * heightChangeSpeed;
                        ghostObject.transform.position = newPosition;
                    }
                }

                // Rotation
                if (Input.GetKey(KeyCode.Q))
                {
                    RotateGhostObject(rotateSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    RotateGhostObject(-rotateSpeed * Time.deltaTime);
                }
            }

            // Other methods remain unchanged...

        private void CreateGhostObject(RaycastHit hit)
        {
            ghostObject = Instantiate(realObject, new Vector3(hit.point.x, hit.point.y + GetObjectHeight(hit.transform), hit.point.z), Quaternion.identity);

            SetGhostOutline(hit.transform.gameObject);
            SetUIEvent(hit.transform.gameObject);
        }

        private void RotateGhostObject(float angle)
        {
            if (ghostObject)
            {
                ghostObject.transform.Rotate(0, angle, 0);
            }
        }

        private void SetUIEvent(GameObject areaToBeBuilt)
        {
            uiManager.Build = delegate
            {
                if (CanBuild(areaToBeBuilt, ghostObject))
                {
                    if(realObject.GetComponent<DemonPortal>() != null)
                    {
                        Transform curr = landLayerManager.GetCurrPlot().transform;
                        Transform next = landLayerManager.NextPlot(curr.gameObject).transform;
                        ghostObject.transform.SetParent(curr);
                        portalManager.PlacePortal(ghostObject.transform.localPosition, curr, next);
                    }
                    else
                    {
                        
                  
                    GameObject go = Instantiate(realObject, ghostObject.transform.position, ghostObject.transform.rotation);
                    if (rootObject)
                        go.transform.SetParent(rootObject.transform);
                    }
                }
                else
                {
                    Debug.LogWarning("you can't build in impossible area");
                }
                DestroyGhostObject();
                constructionMode = false;
            };

            uiManager.Cancel = delegate
            {
                DestroyGhostObject();
                constructionMode = false;
            };
        }

        // Show Object to be built
        private void SetGhostOutline(GameObject areaToBeBuilt)
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
            if (tf.GetComponent<Collider>())
            {
                return tf.GetComponent<Collider>().bounds.size.y / 2;
            }
            else
            {
                return tf.transform.position.y;
            }
        }

        private void DestroyGhostObject()
        {
            if (ghostObject)
            {
                Destroy(ghostObject);
            }
        }
    } // class ConstructionManager
}
