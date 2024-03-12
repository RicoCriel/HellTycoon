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

        [Header("SplineConnectors")]
        [SerializeField]
        private PlaceholderConnectorHitBox BoxIn;
        [SerializeField]
        private PlaceholderConnectorHitBox BoxOut;

        [SerializeField]
        private Transform middlePoint;

        private void Awake()
        {
            List<Vector3> points = new List<Vector3>();
            points.Add(BoxIn.GetConnectorPointSpline());
            points.Add(BoxIn.GetConnectorAnglePointSpline());
            points.Add(middlePoint.position);
            points.Add(BoxOut.GetConnectorAnglePointSpline());
            points.Add(BoxOut.GetConnectorPointSpline());


            instanciatedSpline = Instantiate(SplineViewPrefab);

            instanciatedSpline.AddPoints(points, 1, Vector3.zero);
            // instanciatedSpline.SetPointOrientation(BoxIn.GetConnectorPointDirection(), 0);
            // instanciatedSpline.SetPointOrientation(-BoxOut.GetConnectorPointDirection(), 2);

            // instanciatedSpline.SetSplineType(Spline.Type.Bezier);

            SplineMesh.Channel meshChannel = instanciatedSpline.AddMeshToGenerate(_meshToUse);
            float SplineSize = instanciatedSpline.GetSplineUniformSize();
            instanciatedSpline.SetMaterial(_materialToUse);
            instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 3);
            // instanciatedSpline.SetMeshSize(10);
            instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));

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

                // follower.HookUpEndReachedEvent();
                // follower.FollowerArrived += (sender, args) => {
                //     //Call machine code where the object just arrived.
                //     Destroy(args.GameObject);
                // };
                //

                EventHandler<FollowerArrivedEventArgs> followerArrivedHandler = null;
                followerArrivedHandler = (sender, args) => {
                    Debug.Log("Follower Arrived");
                    //Call machine code where the object just arrived.
                    follower.FollowerArrived -= followerArrivedHandler; // Unsubscribe after arrival
                    Destroy(args.GameObject);
                };
                follower.FollowerArrived += followerArrivedHandler;

                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
        }

        public void SpawnSplineFollower(GameObject gameObject, SplineView computer)
        {
            //get relevant data
            SplineComputer splineComputer = instanciatedSpline.GetSplinecomputer();
            Vector3 StartPoint = instanciatedSpline.GetSplineStartingPoint();
            
            //instantiate and parent demon to follower
            SplineFollowerView follower = Instantiate(_followerViewPrefab, StartPoint, Quaternion.identity, instanciatedSpline.transform);
            gameObject.transform.parent = follower.transform;
            
            //set up follower logic
            follower.SetComputer(splineComputer);
            follower.SetFollow(true);
            follower.SetSpeed(_followSpeed);
            follower.SetFollowMode(SplineFollower.FollowMode.Uniform);

            //hook up events
            EventHandler<FollowerArrivedEventArgs> followerArrivedHandler = null;
            followerArrivedHandler = (sender, args) => {
                
                Debug.Log("Follower Arrived");
                //todo Call machine code where the object just arrived.
                
                follower.FollowerArrived -= followerArrivedHandler; // Unsubscribe after arrival
                
                //unparent get data etc?
                Destroy(args.GameObject);
            };
            follower.FollowerArrived += followerArrivedHandler;
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
