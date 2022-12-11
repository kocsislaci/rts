namespace MyRTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckHasCommand : Node
    {
        private UnitController _controller;

        public CheckHasCommand(ref UnitController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.ReceivedCommands.Count > 0 ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
