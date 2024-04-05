using Splines.Drawing;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Splines.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MeshDataList", order = 1)]
    public class MeshDataList : ScriptableObject
    {
        public List<MeshDataPart> MeshDataParts = new List<MeshDataPart>();

        public Dictionary<SplineType, MeshDataPart> MeshDataDictionary = new Dictionary<SplineType, MeshDataPart>();

        private void OnValidate()
        {
            MeshDataParts ??= new List<MeshDataPart>();

            MeshDataDictionary.Clear();
            foreach (MeshDataPart meshDataPart in MeshDataParts)
            {
                MeshDataDictionary.TryAdd(meshDataPart.MeshType, meshDataPart);
            }
        }

    }
}
