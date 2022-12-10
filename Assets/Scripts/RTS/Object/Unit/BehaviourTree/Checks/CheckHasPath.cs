using RTS.Object.Unit.Character;

namespace RTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckHasPath: Node
    {
        private CharacterController _controller;
        
        public CheckHasPath(ref CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.HasPath() ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}