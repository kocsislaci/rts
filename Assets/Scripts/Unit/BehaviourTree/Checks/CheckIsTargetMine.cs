using GameManagers;
using UnityEngine;

namespace Unit.BehaviourTree.Checks
{
    public class CheckTargetIsMine: Node
    {
        private Team _myPlayerId;

        public CheckTargetIsMine(UnitController controller) : base()
        {
            _myPlayerId = GameManager.MyTeam;
        }

        public override NodeState Evaluate()
        {
            object currentTarget = Parent.GetData("currentTarget");
            UnitController um = ((Transform)currentTarget).GetComponent<UnitController>();
            if (um == null)
            {
                _state = NodeState.FAILURE;
                return _state;
            }
            _state = um.representingObject.Owner == _myPlayerId ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
    }
}
