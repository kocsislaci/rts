using UnityEngine;
using UnityEngine.AI;

namespace MyRTS.Object.Unit.Capabilities.Movable
{
    public interface IMovable
    {
        public NavMeshAgent Agent { get; }
        public bool SetDestination(Vector3 targetPosition);
        public bool HasPath();
        public bool HasReachedDestination();
        public bool ResetPath();
    }
}
