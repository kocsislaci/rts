using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.BehaviourTree.Tasks
{
    public class TaskAttack: Node
    {
        CharacterController _controller;

        public TaskAttack(CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            object currentTarget = GetData("currentTarget");
            _controller.Attack((Transform) currentTarget);
            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}
