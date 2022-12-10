namespace RTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckIsUnitTargetMine: Node
    {
        private UnitController _controller;
        
        public CheckIsUnitTargetMine(ref UnitController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.ReceivedCommands[0].clickedUnit.Owner.Faction == _controller.Owner.Faction ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}