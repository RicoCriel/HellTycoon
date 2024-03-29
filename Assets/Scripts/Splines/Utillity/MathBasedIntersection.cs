using System.Collections.Generic;
using UnityEngine;
namespace Splines.Utillity
{
    public static class MathBasedIntersection
    {
        
        public static bool LineIntersectsItself3D(List<Vector3> points, out int pointIndex, bool drawDebug)
        {
            if (points.Count > 3)
            {
                Vector3 pointA1 = points[points.Count - 1];
                Vector3 pointA2 = points[points.Count - 2];

                if (drawDebug)
                {
                    Debug.DrawLine(pointA1 + Vector3.up *3, pointA2+ Vector3.up *3, Color.blue, 0);
                }

                for (int index = 0; index < points.Count - 2; index++)
                {
                    Vector3 pointB1 = points[index];
                    Vector3 pointB2 = points[index + 1];

                    if (drawDebug)
                    {
                        Debug.DrawLine(pointB1+ Vector3.up *3, pointB2+ Vector3.up *3, Color.green, 0);
                    }

                    float minDistance = 0.2f; // Adjust the threshold as needed
                    if (Vector3.Distance(pointA1, pointB1) < minDistance || Vector3.Distance(pointA2, pointB2) < minDistance)
                    {
                        continue; // Skip intersection check if the distance is below the threshold
                    }

                    if (DoLinesIntersect3D(pointA1, pointA2, pointB1, pointB2))
                    {
                        pointIndex = index;
                        return true;
                    }
                }
            }
            pointIndex = -1;
            return false;
        }
        
        public static bool LineIntersectsItself3D(List<Vector3> points, Vector3 MostRecentPoint, Vector3 PredictionPoint, out int pointIndex, bool drawDebug)
        {
            if (points.Count > 4)
            {
                Vector3 pointA1 = MostRecentPoint;
                Vector3 pointA2 = PredictionPoint;

                if (drawDebug)
                {
                    Debug.DrawLine(pointA1+ Vector3.up *3, pointA2+ Vector3.up *3, Color.blue, 0);
                }

                for (int index = 0; index < points.Count - 1; index++)
                {
                    Vector3 pointB1 = points[index];
                    Vector3 pointB2 = points[index + 1];

                    if (drawDebug)
                    {
                        Debug.DrawLine(pointB1+ Vector3.up *3, pointB2+ Vector3.up *3, Color.green, 0);
                    }

                    float minDistance = 0.2f; // Adjust the threshold as needed
                    if (Vector3.Distance(pointA1, pointB1) < minDistance || Vector3.Distance(pointA2, pointB2) < minDistance)
                    {
                        continue; // Skip intersection check if the distance is below the threshold
                    }

                    if (DoLinesIntersect3D(pointA1, pointA2, pointB1, pointB2))
                    {
                        pointIndex = index;
                        return true;
                    }
                }
            }
            pointIndex = -1;
            return false;
        }

        private static bool DoLinesIntersect3D(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
        {
            Vector3 crossProductA = Vector3.Cross(a2 - a1, b1 - a1);
            Vector3 crossProductB = Vector3.Cross(a2 - a1, b2 - a1);

            // Check if the lines are parallel (cross products are colinear)
            if (Vector3.Dot(crossProductA, crossProductB) >= 0)
            {
                return false;
            }

            Vector3 crossProductC = Vector3.Cross(b2 - b1, a1 - b1);
            Vector3 crossProductD = Vector3.Cross(b2 - b1, a2 - b1);

            // Check if the lines are parallel (cross products are colinear)
            if (Vector3.Dot(crossProductC, crossProductD) >= 0)
            {
                return false;
            }

            return true;
        }
    }
}
