using UnityEngine;
using CharacterController = RTS.Object.Unit.Character.CharacterController;

namespace RTS.Object.Unit.BehaviourTree.Tasks
{
    public class TaskSetDestinationToTerrainTarget: Node
    {
        private CharacterController _controller;
        

        public TaskSetDestinationToTerrainTarget(ref CharacterController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            _state = _controller.SetDestination((Vector3)_controller.ReceivedCommands[0].clickedPlaceOnTerrain) ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
        
    }
}