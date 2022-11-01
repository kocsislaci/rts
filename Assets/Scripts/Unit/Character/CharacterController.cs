using System;
using UnityEngine;
using UnityEngine.AI;

namespace Unit.Character
{
    public class CharacterController : UnitController
    {
        [SerializeField] private NavMeshAgent agent;
        
        public void MoveTo(Vector3 targetPosition)
        {
            agent.destination = targetPosition;
        }
        
        
    }
}
