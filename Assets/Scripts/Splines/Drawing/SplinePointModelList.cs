using System.Collections.Generic;
using System.Linq;
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
    }
}
