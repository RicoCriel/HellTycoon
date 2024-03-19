using Splines;
using System.Collections.Generic;
using UnityEngine;
namespace Buildings
{
    public class BuildingBase : MonoBehaviour
    {
        List<PlaceholderConnectorHitBox> EntryBoxes = new List<PlaceholderConnectorHitBox>();
        List<PlaceholderConnectorHitBox> ExitBoxes = new List<PlaceholderConnectorHitBox>();

        private void Awake()
        {
            ConnectHitBoxesOnAwake();

        }
        private void ConnectHitBoxesOnAwake()
        {

            PlaceholderConnectorHitBox[] hitBoxes = GetComponentsInChildren<PlaceholderConnectorHitBox>();

            foreach (PlaceholderConnectorHitBox VARIABLE in hitBoxes)
            {
                if (VARIABLE.IsSplineStart)
                {
                    EntryBoxes.Add(VARIABLE);
                }
                else
                {
                    ExitBoxes.Add(VARIABLE);
                }
            }
        }
    }
}
