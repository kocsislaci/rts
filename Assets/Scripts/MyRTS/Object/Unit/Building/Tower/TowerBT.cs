using System.Collections.Generic;
using MyRTS.Object.Unit.BehaviourTree;
using MyRTS.Object.Unit.BehaviourTree.Checks;
using MyRTS.Object.Unit.BehaviourTree.composites;
using MyRTS.Object.Unit.BehaviourTree.decorators;
using MyRTS.Object.Unit.BehaviourTree.Tasks;
using MyRTS.Object.Unit.Capabilities.Attacker;
using MyRTS.Object.Unit.Capabilities.General;

namespace MyRTS.Object.Unit.Building.Tower
{
    public class TowerBT: Tree
    {
        private TowerController _controller;

        private void Awake()
        {
            _controller = GetComponent<TowerController>();
        }

        protected override Node SetupTree()
        {
            Node _root;
            
            ITargeter targeter = _controller;
            IRangedAttacker rangedAttacker = _controller;

            _root = new Selector(new List<Node> 
            {
                new Sequence(new List<Node>
                {
                    new CheckHasTarget(ref targeter),
                    new Timer(_controller.AttackerData.attackSpeed, new List<Node> { new TaskRangedAttack(ref rangedAttacker)}),
                }),
                new TaskTryGetEnemyFromFOV(ref targeter),
            });
            return _root;
        }
    }
}