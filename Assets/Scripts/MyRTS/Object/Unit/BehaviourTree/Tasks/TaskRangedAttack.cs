using MyRTS.Object.Unit.Capabilities.Attacker;

namespace MyRTS.Object.Unit.BehaviourTree.Tasks
{
    public class TaskRangedAttack: Node
    {
        private IRangedAttacker _controller;
        
        public TaskRangedAttack(ref IRangedAttacker controller) : base()
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            _controller.RangedAttack(_controller.Target);
            return NodeState.SUCCESS;
        }
    }
}