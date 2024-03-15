using Dreamteck.Splines;
using System;
using UnityEngine;
namespace Splines
{
    public class SplineFollowerView : MonoBehaviour
    {
       [SerializeField] private SplineFollower _mySplineFollower;


        private void Awake()
        {
            if (_mySplineFollower == null)
                _mySplineFollower = GetComponent<SplineFollower>();
            _mySplineFollower.onEndReached += OnEndReached;
        }

        // public void HookUpEndReachedEvent()
        // {
        //     _mySplineFollower.onEndReached += OnEndReached;
        // }
        //
        private void OnEndReached(double obj)
        {
            _mySplineFollower.onEndReached -= OnEndReached;
            OnFollowerArrived(new FollowerArrivedEventArgs(this.gameObject));
            // Destroy(this.gameObject);
        }

        public void SetComputer(SplineComputer splineComputer)
        {
            _mySplineFollower.spline = splineComputer;
        }

        public void SetSpeed(float speed)
        {
            _mySplineFollower.followSpeed = speed;
        }

        public void SetFollowMode(SplineFollower.FollowMode mode)
        {
            _mySplineFollower.followMode = mode;
        }

        public void SetWrapMode(SplineFollower.Wrap mode)
        {
            _mySplineFollower.wrapMode = mode;
        }

        public void SetDirection(Spline.Direction direction)
        {
            _mySplineFollower.direction = direction;
        }

        public void SetFollow(bool follow)
        {
            _mySplineFollower.follow = follow;
        }
        
        public event EventHandler<FollowerArrivedEventArgs> FollowerArrived;
        
        protected virtual void OnFollowerArrived(FollowerArrivedEventArgs eventargs)
        {
            EventHandler<FollowerArrivedEventArgs> handler = FollowerArrived;
            handler?.Invoke(this, eventargs);
        }
      
    
    }
    
    public class FollowerArrivedEventArgs : EventArgs
    {
        public GameObject GameObject{ get; }
        public FollowerArrivedEventArgs(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
