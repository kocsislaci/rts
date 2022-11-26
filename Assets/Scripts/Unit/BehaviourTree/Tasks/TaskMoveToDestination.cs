using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.BehaviourTree.Tasks
{
    public class TaskMoveToDestination: Node
    {
        CharacterController _controller;

        public TaskMoveToDestination(CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            object destinationPoint = GetData("destinationPoint");
            Vector3 destination = (Vector3) destinationPoint;
            // check to see if the destination point was changed
            // and we need to re-update the agent's destination
            if (Vector3.Distance(destination, _controller.agent.destination) > 0.2f)
            {
                bool canMove = _controller.MoveTo(destination);
                _state = canMove ? NodeState.RUNNING : NodeState.FAILURE;
                return _state;
            }

            // check to see if the agent has reached the destination
            float d = Vector3.Distance(_controller.transform.position, _controller.agent.destination);
            if (d <= _controller.agent.stoppingDistance)
            {
                ClearData("destinationPoint");
                _state = NodeState.SUCCESS;
                return _state;
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}