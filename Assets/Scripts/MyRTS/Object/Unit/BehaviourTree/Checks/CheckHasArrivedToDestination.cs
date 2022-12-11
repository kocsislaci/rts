
using MyRTS.Object.Unit.Character;

namespace MyRTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckHasArrivedToDestination : Node
    {
        private CharacterController _controller;

        public CheckHasArrivedToDestination(ref CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.HasReachedDestination() ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}