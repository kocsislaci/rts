using MyRTS.Object.Unit.Capabilities.General;

namespace MyRTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckHasTarget: Node
    {
        private ITargeter _controller;
        
        public CheckHasTarget(ref ITargeter controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.Target != null ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}