
using RTS.Object.Unit.Character;

namespace RTS.Object.Unit.BehaviourTree.Checks
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