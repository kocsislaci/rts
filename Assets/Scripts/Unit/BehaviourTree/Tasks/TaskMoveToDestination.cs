using Unit.Character;

namespace Unit.BehaviourTree.Tasks
{
    public class TaskMoveToDestination: Node
    {
        private CharacterController _controller;
        

        public TaskMoveToDestination(ref CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            _state = _controller.MoveToDestination() ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
        
    }
}