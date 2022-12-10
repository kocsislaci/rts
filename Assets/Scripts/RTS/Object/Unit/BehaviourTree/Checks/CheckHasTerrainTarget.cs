using RTS.Object.Unit.Character;
using UnityEngine;

namespace RTS.Object.Unit.BehaviourTree.Checks
{
    public class CheckHasTerrainTarget: Node
    {
        private UnitController _controller;
        
        public CheckHasTerrainTarget(ref UnitController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            return _controller.ReceivedCommands[0].clickedPlaceOnTerrain is not null ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
