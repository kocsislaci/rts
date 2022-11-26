using GameManagers;
using Unit.Character;
using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.BehaviourTree.Tasks
{
    public class TaskFollow : Node
    {
        CharacterController _controller;
        Vector3 _lastTargetPosition;

        public TaskFollow(CharacterController controller) : base()
        {
            _controller = controller;
            _lastTargetPosition = Vector3.zero;
        }

        public override NodeState Evaluate()
        {
            object currentTarget = GetData("currentTarget");
            Vector3 targetPosition = _GetTargetPosition((Transform)currentTarget);

            if (targetPosition != _lastTargetPosition)
            {
                _controller.MoveTo(targetPosition);
                _lastTargetPosition = targetPosition;
            }

            // check if the agent has reached destination
            float d = Vector3.Distance(_controller.transform.position, _controller.agent.destination);
            if (d <= _controller.agent.stoppingDistance)
            {
                ClearData("currentTarget");
                _state = NodeState.SUCCESS;
                return _state;
            }

            _state = NodeState.RUNNING;
            return _state;
        }

        private Vector3 _GetTargetPosition(Transform target)
        {
            Vector3 s = target.Find("Mesh").localScale;
            float targetSize = Mathf.Max(s.x, s.z);

            Vector3 p = _controller.transform.position;
            Vector3 t = target.position - p;
            // (add a little offset to avoid bad collisions)
            float d = targetSize + ((CharacterData)_controller.representingObject.data).attackRange - 0.2f;
            float r = d / t.magnitude;
            return p + t * (1 - r);
        }
    }
}
