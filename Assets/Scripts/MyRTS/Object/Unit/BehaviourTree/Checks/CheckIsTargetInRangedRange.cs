namespace MyRTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckIsTargetInRangedRange: Node
    {
        private UnitController _controller;
        
        public CheckIsTargetInRangedRange(ref UnitController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.ReceivedCommands[0].clickedPlaceOnTerrain is not null ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}