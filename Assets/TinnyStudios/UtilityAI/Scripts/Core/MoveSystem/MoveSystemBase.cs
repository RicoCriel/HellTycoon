using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The base class for the move system. This lets the agent choose which system to use.
    /// You have to implement your own, see <see cref="MoveSystemNavMeshExample"/> for an example.
    /// </summary>
    public abstract class MoveSystemBase : MonoBehaviour
    {
        public Transform DestinationTransform { get; private set; }

        public abstract bool ReachedDestination();

        public abstract void SetDestination(Vector3 destination);

        public void SetDestination(Transform destinationTransform)
        {
            DestinationTransform = destinationTransform;
            SetDestination(DestinationTransform.position);
        }

        public abstract void SetProperties(ActionMoveData moveData);
        public abstract void Stop();
    }
}