using Dreamteck.Splines.Editor;
using Splines;
using System.Collections.Generic;
using UnityEngine;
using Splines.Drawing;
using SplineDrawer = Splines.Drawing.SplineDrawer;
namespace Buildings
{
    public class BuildingBase : MonoBehaviour
    {
        internal List<PlaceholderConnectorHitBox> _entryBoxes = new List<PlaceholderConnectorHitBox>();
        internal List<PlaceholderConnectorHitBox> _exitBoxes = new List<PlaceholderConnectorHitBox>();

        internal SplineDrawer splineDrawer;

        protected void Awake()
        {
            if (splineDrawer == null)
                splineDrawer = FindObjectOfType<SplineDrawer>();

            ConnectHitBoxesOnAwake();

        }
        private void ConnectHitBoxesOnAwake()
        {

            PlaceholderConnectorHitBox[] hitBoxes = GetComponentsInChildren<PlaceholderConnectorHitBox>();

            foreach (PlaceholderConnectorHitBox Connector in hitBoxes)
            {
                if (Connector.IsSplineStart)
                {
                    _entryBoxes.Add(Connector);
                }
                else
                {
                    _exitBoxes.Add(Connector);
                }
                Connector.myBuildingNode = this;
            }
        }
    }
}
