using RTS.Object.Unit.Character;
using UnityEngine;

namespace RTS.Object.Unit.BehaviourTree.Tasks
{
    public class TaskRemoveFinishedCommand : Node
    {
        private UnitController _controller;
        
        public TaskRemoveFinishedCommand(ref UnitController controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            _controller.FinishCommand();
            Debug.Log("hello finished");
            return NodeState.SUCCESS;
        }
    }
}