using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Splines.Drawing
{
    public class SplinePointModelList
    {
        List<SplinePointModel> SplinePointModels{ get; set; }

        public SplinePointModelList()
        {
            SplinePointModels = new List<SplinePointModel>();
        }

        public void AddSplinePointModel(SplinePointModel splinePointModel)
        {
            SplinePointModels.Add(splinePointModel);
        }

        public void RemoveSplinePointModel(SplinePointModel splinePointModel)
        {
            SplinePointModels.Remove(splinePointModel);
        }

        public void RemoveSplinePointModel(int index)
        {
            SplinePointModels.RemoveAt(index);
        }

        public void ClearSplinePointModels()
        {
            SplinePointModels.Clear();
        }

        public List<SplinePointModel> GetSplinePointModels()
        {
            return SplinePointModels;
        }

        public void OrderSplinePointList()
        {
            SplinePointModels.Sort((x, y) => x.SplinePointIndex.CompareTo(y.SplinePointIndex));
        }

        public void GetAllPointsAboveHeight(int height, out List<SplinePointModel> splinePointModels)
        {
            splinePointModels = SplinePointModels.Where(splinePointModel => splinePointModel.Height > height).ToList();
        }

        public void GetAllPointsBelowHeight(int height, out List<SplinePointModel> splinePointModels)
        {
            splinePointModels = SplinePointModels.Where(splinePointModel => splinePointModel.Height < height).ToList();
        }

        public int GetCurrentHeight()
        {
            return SplinePointModels.Count > 0 ? SplinePointModels[^1].Height : 0;
        }

        public int GetStartingHeight()
        {
            return SplinePointModels.Count > 0 ? SplinePointModels[0].Height : 0;
        }

        public SplinePointModel GetSplinePointModel(int index)
        {
            return SplinePointModels[index];
        }

        public SplinePointModel GetLastSplinePointModel()
        {
            return SplinePointModels.Count > 0 ? SplinePointModels[^1] : null;
        }

        public SplinePointModel GetFirsttoLastSplinePointModel()
        {
            return SplinePointModels.Count > 0 ? SplinePointModels[^2] : null;
        }

        public int GetSplinePointModelCount()
        {
            return SplinePointModels.Count;
        }

        public void SetsplinepointForwards()
        {
            for (int i = 0; i < SplinePointModels.Count; i++)
            {
                if (i == 0)
                {
                    SplinePointModels[i].SplinePointForward = (SplinePointModels[i + 1].WorldPosition - SplinePointModels[i].WorldPosition).normalized;
                }
                else if (i == SplinePointModels.Count - 1)
                {
                    SplinePointModels[i].SplinePointForward = (SplinePointModels[i].WorldPosition - SplinePointModels[i - 1].WorldPosition).normalized;
                }
                else
                {
                    SplinePointModels[i].SplinePointForward = (SplinePointModels[i + 1].WorldPosition - SplinePointModels[i - 1].WorldPosition).normalized;
                }
            }
        }

        public List<Vector3> CreateSupportBeamPositions(float offset)
        {
            List<Vector3> _supportBeamPositions = new List<Vector3>();

            List<SplinePointModel> points = SplinePointModels;
            for (int i = 0; i < points.Count; i++)
            {
                // Get the tangent and normal vectors at the current point
                Vector3 tangent = points[i].SplinePointForward.normalized;


                // Debug.Log(tangent + " " + normal);

                // Calculate the left and right points based on the normal vector
                Vector3 left = Vector3.Cross(tangent, Vector3.up).normalized;
                Vector3 right = -left;

                // Add the left and right points to the support beam positions
                _supportBeamPositions.Add(points[i].WorldPosition + left * offset);
                _supportBeamPositions.Add(points[i].WorldPosition + right * offset);

                // Debug.DrawLine(points[i].position, points[i].position + left * LeftRightSize, Color.green, 1f);
                // Debug.DrawLine(points[i].position, points[i].position + right * LeftRightSize, Color.red, 1f);
            }

            return _supportBeamPositions;
        }

        public void RemoveRandomPointsSupportBeams(float percentageToRemove, List<Vector3> positions)
        {
            int totalPointsToRemove = Mathf.RoundToInt(positions.Count * percentageToRemove);

            for (int i = positions.Count - 1; i >= 0; i--) // Iterate in reverse order
            {
                if (Random.value < percentageToRemove) // Randomly decide whether to remove the point
                {
                    positions.RemoveAt(i);
                }
            }

            Debug.Log("Remaining points count: " + positions.Count);
        }

        public void VisualizeSplinePointModels()
        {
            foreach (SplinePointModel splinePointModel in SplinePointModels)
            {
                UnityEngine.Debug.DrawLine(splinePointModel.WorldPosition, splinePointModel.WorldPosition + splinePointModel.SplinePointForward * 1, Color.red, 5);
            }
        }

        public List<(Meshtype, List<SplinePointModel>)> MeshDivisionsBasedOnHeight()
        {
            List<SplinePointModel> SplinePointModelsList = SplinePointModels;

            List<(Meshtype, List<SplinePointModel>)> SplinePointModelLists = new List<(Meshtype, List<SplinePointModel>)>();

            Meshtype currentMeshType = Meshtype.flatground;
            List<SplinePointModel> currentList = new List<SplinePointModel>();

            int lastHeight = 0;
            SplinePointModel LastExPoint = null;
            foreach (SplinePointModel splinepoint in SplinePointModelsList)
            {
                int currentHeight = splinepoint.Height;

                if (currentHeight == 0)
                {
                    if (currentMeshType != Meshtype.flatground)
                    {
                        SplinePointModelLists.Add((currentMeshType, currentList));
                        currentList = new List<SplinePointModel>();
                        currentMeshType = Meshtype.flatground;
                        if (LastExPoint != null)
                        {
                            currentList.Add(LastExPoint);

                        }

                    }
                }
                else
                {
                    if (currentHeight != lastHeight && currentMeshType != Meshtype.slope)
                    {
                        if (currentList.Count > 0)
                        {
                            SplinePointModelLists.Add((currentMeshType, currentList));
                            currentList = new List<SplinePointModel>();
                            if (LastExPoint != null)
                            {
                                currentList.Add(LastExPoint);

                            }

                        }
                        currentMeshType = Meshtype.slope;
                    }
                    else if (currentHeight != lastHeight && currentMeshType == Meshtype.slope)
                    {
                        currentMeshType = Meshtype.slope;
                    }
                    else if (currentHeight != 0 && currentMeshType != Meshtype.flatAir)
                    {
                        if (currentList.Count > 0)
                        {
                            SplinePointModelLists.Add((currentMeshType, currentList));
                            currentList = new List<SplinePointModel>();
                            if (LastExPoint != null)
                            {
                                currentList.Add(LastExPoint);

                            }

                        }
                        currentMeshType = Meshtype.flatAir;
                    }
                }

                currentList.Add(splinepoint);
                lastHeight = currentHeight;
                LastExPoint = splinepoint;
            }

            // Add the last list
            SplinePointModelLists.Add((currentMeshType, currentList));

            return SplinePointModelLists;
        }




        public List<(Meshtype, List<SplinePointModel>)> findMeshDivisions()
        {
            List<SplinePointModel> SplinePointModelsList = SplinePointModels;

            List<(Meshtype, List<SplinePointModel>)> SplinePointModelLists = new List<(Meshtype, List<SplinePointModel>)>();

            Meshtype currentMeshType = Meshtype.flatground;
            List<SplinePointModel> currentList = new List<SplinePointModel>();

            SplinePointModel LastExPoint = null;

            //handle first point
            if (SplinePointModelsList[0].Height == 0 && SplinePointModelsList[0].SplinePointForward.y == 0)
            {
                currentMeshType = Meshtype.flatground;
            }
            else if (SplinePointModelsList[0].Height > 0 && SplinePointModelsList[0].SplinePointForward.y != 0)
            {
                currentMeshType = Meshtype.slope;
            }
            else if (SplinePointModelsList[0].Height > 0 && SplinePointModelsList[0].SplinePointForward.y == 0)
            {
                currentMeshType = Meshtype.flatAir;
            }
            LastExPoint = SplinePointModelsList[0];

            //handle rest of points
            for (int index = 0; index < SplinePointModelsList.Count; index++)
            {
                SplinePointModel splinepoint = SplinePointModelsList[index];

                if (index == 0)
                {
                    continue;
                }

                //check if true ground point
                if (splinepoint.Height == 0 && splinepoint.SplinePointForward.y < 0.1f)
                {
                    if (splinepoint.Height != LastExPoint.Height)
                    {
                        SplinePointModelLists.Add((currentMeshType, currentList));
                        currentList = new List<SplinePointModel>();
                        currentList.Add(splinepoint);
                        currentMeshType = Meshtype.flatground;
                    }
                }
                //check if slope point
                else if (splinepoint.Height > 0 && splinepoint.SplinePointForward.y is > 0.1f or < -0.1f)
                {
                    if (splinepoint.Height == LastExPoint.Height)
                    {
                        SplinePointModelLists.Add((currentMeshType, currentList));
                        currentList = new List<SplinePointModel>();
                        currentList.Add(splinepoint);
                        currentMeshType = Meshtype.slope;
                    }
                }

                //check if air point
                else if (splinepoint.Height > 0 && splinepoint.SplinePointForward.y < 0.1f)
                {
                    if (splinepoint.Height != LastExPoint.Height)
                    {
                        SplinePointModelLists.Add((currentMeshType, currentList));
                        currentList = new List<SplinePointModel>();
                        currentList.Add(splinepoint);
                        currentMeshType = Meshtype.flatAir;
                    }
                }

                currentList.Add(splinepoint);
                LastExPoint = splinepoint;
            }
            return SplinePointModelLists;
        }

        public enum Meshtype
        {
            flatground,
            slope,
            flatAir,
        }
    }
}
