using System.Collections.Generic;
using Unit.BehaviourTree;
using Unit.BehaviourTree.Checks;
using Unit.BehaviourTree.composites;
using Unit.BehaviourTree.Tasks;

namespace Unit.Character
{
    public class CharacterBT: Tree
    {
        private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        protected override Node SetupTree()
        {
            Node _root;
            
            _root = new Selector(new List<Node> {
                new TaskTrySetDestination(controller: ref controller),
                new Sequence(new List<Node>
                {
                    new CheckHasDestination(controller: ref controller),
                    new TaskMoveToDestination(controller: ref controller)
                })
            });
            
            return _root;
        }
    }
}
