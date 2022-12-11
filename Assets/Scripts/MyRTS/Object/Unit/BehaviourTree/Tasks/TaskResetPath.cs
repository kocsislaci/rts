using MyRTS.Object.Unit.Character;

namespace MyRTS.Object.Unit.BehaviourTree.Tasks
{
    public class TaskResetPath: Node
    {
        private CharacterController _controller;
        
        public TaskResetPath(ref CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.ResetPath() ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}