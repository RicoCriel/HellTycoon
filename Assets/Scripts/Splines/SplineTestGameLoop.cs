using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Splines
{
    public class SplineTestGameLoop : MonoBehaviour
    {
        [Header("Spline Prefabs meshes and mats")]
        [SerializeField]
        private SplineView SplineViewPrefab;

        [SerializeField]
        private Mesh _meshToUse;
        [SerializeField]
        private Material _materialToUse;

        private SplineView instanciatedSpline;

        [Header("TestStuff")]
        [Range(1, 100)]
        [SerializeField]
        private float SizeTester;

        [Header("SplineFollowerTest")]
        [SerializeField]
        private SplineFollowerView _followerViewPrefab;

        [SerializeField]
        [Range(0.1f, 3)]
        private float _timeBetweenSpawns = 1f;

        [SerializeField]
        [Range(1f, 100)]
        private float _spawnAmount = 1f;

        [SerializeField]
        [Range(1f, 20)]
        private float _followSpeed = 1f;

        private void Awake()
        {
            instanciatedSpline = Instantiate(SplineViewPrefab);

            SplineMesh.Channel meshChannel = instanciatedSpline.AddMeshToGenerate(_meshToUse);
            float SplineSize = instanciatedSpline.GetSplineUniformSize();
            instanciatedSpline.SetMaterial(_materialToUse);
            instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 2);
            instanciatedSpline.SetMeshSize(10);

            StartCoroutine(TestsplineFollowers());
        }
        IEnumerator TestsplineFollowers()
        {
            yield return new WaitForSeconds(1f);
            SplineComputer splineComputer = instanciatedSpline.GetSplinecomputer();

            for (int i = 0; i < _spawnAmount; i++)
            {
                Vector3 StartPoint = instanciatedSpline.GetSplineStartingPoint();
                SplineFollowerView follower = Instantiate(_followerViewPrefab, StartPoint, Quaternion.identity, instanciatedSpline.transform);
                follower.SetComputer(splineComputer);
                follower.SetFollow(true);
                follower.SetSpeed(_followSpeed);
                follower.SetFollowMode(SplineFollower.FollowMode.Uniform);
                follower.HookUpEndReachedEvent();

                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
        }

        private void Update()
        {
            SplineMesh.Channel meshChannel = instanciatedSpline.GetMeshChannel(0);
            float SplineSize = instanciatedSpline.GetSplineUniformSize();
            // instanciatedSpline.SetMeshSize(SizeTester);
            instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 2);
            instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));
        }
    }
}
