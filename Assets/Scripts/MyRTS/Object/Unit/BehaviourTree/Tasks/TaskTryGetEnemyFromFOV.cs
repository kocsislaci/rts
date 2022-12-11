using MyRTS.Object.Unit.Capabilities.General;

namespace MyRTS.Object.Unit.BehaviourTree.Tasks
{
    public class TaskTryGetEnemyFromFOV: Node
    {
        private ITargeter _controller;
        
        public TaskTryGetEnemyFromFOV(ref ITargeter controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            var enemy = _controller.TryPickEnemyFromFOV();
            if (enemy == null)
            {
                return NodeState.FAILURE;
            }
            else
            {
                _controller.Target = enemy;
                return NodeState.SUCCESS;
            }
        }
    }
}