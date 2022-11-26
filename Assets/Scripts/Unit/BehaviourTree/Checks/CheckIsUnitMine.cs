using GameManagers;

namespace Unit.BehaviourTree.Checks
{
    public class CheckIsUnitMine: Node
    {
        private bool _isUnitMine;

        public CheckIsUnitMine(UnitController controller) : base()
        {
            _isUnitMine = controller.representingObject.Owner == GameManager.MyTeam;
        }

        public override NodeState Evaluate()
        {
            _state = _isUnitMine ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
    }
}
