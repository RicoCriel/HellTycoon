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
        [SerializeField] private SplineView _splineViewPrefab;

        [SerializeField] private Mesh _meshToUse;
        [SerializeField] private Material _materialToUse;

        private SplineView _instanciatedSpline;

        [Header("TestStuff")] 
        [Range(0.1f, 100)]
        [SerializeField] private float _sizeTester;

        [Header("SplineFollowerTest")]
        [SerializeField]  private SplineFollowerView _followerViewPrefab;
        
        [Range(0.1f, 3)]
        [SerializeField] private float _timeBetweenSpawns = 1f;

        [Range(1f, 100)]
        [SerializeField] private float _spawnAmount = 1f;
        
        [Range(1f, 20)]
        [SerializeField] private float _followSpeed = 1f;
        
        [Header("SplineConnectors")]
        [SerializeField] private PlaceholderConnectorHitBox _boxIn;

        [SerializeField] private PlaceholderConnectorHitBox _boxOut;
        
        [SerializeField] private Transform _middlePoint;

        private void Awake()
        {
            List<Vector3> points = new List<Vector3>();
            points.Add(_boxIn.GetConnectorPointSpline());
            points.Add(_boxIn.GetConnectorAnglePointSpline());
            points.Add(_middlePoint.position);
            points.Add(_boxOut.GetConnectorAnglePointSpline());
            points.Add(_boxOut.GetConnectorPointSpline());


            _instanciatedSpline = Instantiate(_splineViewPrefab);

            _instanciatedSpline.AddPoints(points, 1, Vector3.up);
            // instanciatedSpline.SetPointOrientation(BoxIn.GetConnectorPointDirection(), 0);
            // instanciatedSpline.SetPointOrientation(-BoxOut.GetConnectorPointDirection(), 2);

            // instanciatedSpline.SetSplineType(Spline.Type.Bezier);

            SplineMesh.Channel meshChannel = _instanciatedSpline.AddMeshToGenerate(_meshToUse);
            float splineSize = _instanciatedSpline.GetSplineUniformSize();
            _instanciatedSpline.SetMaterial(_materialToUse);
            _instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)splineSize * 3);
            // instanciatedSpline.SetMeshSize(10);
            _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));

            StartCoroutine(TestsplineFollowers());
        }
        IEnumerator TestsplineFollowers()
        {
            yield return new WaitForSeconds(1f);
            SplineComputer splineComputer = _instanciatedSpline.GetSplinecomputer();

            for (int i = 0; i < _spawnAmount; i++)
            {
                Vector3 startPoint = _instanciatedSpline.GetSplineStartingPoint();
                SplineFollowerView follower = Instantiate(_followerViewPrefab, startPoint, Quaternion.identity, _instanciatedSpline.transform);
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
            SplineComputer splineComputer = _instanciatedSpline.GetSplinecomputer();
            Vector3 startPoint = _instanciatedSpline.GetSplineStartingPoint();
            
            //instantiate and parent demon to follower
            SplineFollowerView follower = Instantiate(_followerViewPrefab, startPoint, Quaternion.identity, _instanciatedSpline.transform);
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
            SplineMesh.Channel meshChannel = _instanciatedSpline.GetMeshChannel(0);
            float splineSize = _instanciatedSpline.GetSplineUniformSize();
            // instanciatedSpline.SetMeshSize(SizeTester);
            _instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)splineSize * 2);
            _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));
        }
    }
}
