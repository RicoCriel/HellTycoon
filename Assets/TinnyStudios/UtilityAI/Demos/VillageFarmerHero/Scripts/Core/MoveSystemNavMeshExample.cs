using UnityEngine;
using UnityEngine.AI;

namespace TinnyStudios.AIUtility
{
    public class MoveSystemNavMeshExample : MoveSystemBase
    {
        public NavMeshAgent NavAgent;
        
        public override bool ReachedDestination()
        {
            if (NavAgent.pathPending) return false;
            if (!(NavAgent.remainingDistance <= NavAgent.stoppingDistance)) return false;

            return !NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f;
        }

        public override void SetDestination(Vector3 destination)
        {
            NavAgent.SetDestination(destination);
        }

        public override void SetProperties(ActionMoveData moveData)
        {
            NavAgent.speed = moveData.Properties.Speed;
            NavAgent.angularSpeed = moveData.Properties.AngularSpeed;
            NavAgent.stoppingDistance = moveData.Properties.StopDistance;
            NavAgent.acceleration = moveData.Properties.Acceleration;
        }

        public override void Stop()
        {
            NavAgent.ResetPath();
        }
    }
}