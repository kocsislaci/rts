using Unit.Character;

namespace Unit.BehaviourTree.Checks
{
    public class CheckHasDestination: Node
    {
        private CharacterController _controller;
        
        public CheckHasDestination(ref CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            if (_controller.HasDestination())
                return NodeState.SUCCESS;
            else
                return NodeState.FAILURE;
        }
    }
}