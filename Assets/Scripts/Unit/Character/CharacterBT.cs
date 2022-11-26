using System.Collections.Generic;
using Unit.BehaviourTree;
using Unit.BehaviourTree.Checks;
using Unit.BehaviourTree.composites;
using Unit.BehaviourTree.decorators;
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

            // _root = new Parallel(new List<Node> {
            //     new Sequence(new List<Node> {
            //         new CheckIsUnitMine(controller),
            //         new TaskTrySetDestination(controller),
            //     }),
            //     new Sequence(new List<Node> {
            //         new CheckHasDestination(),
            //         new TaskMoveToDestination(controller),
            //     })
            // });
            
            
            /*
             * 
             */
            Sequence trySetDestinationOrTargetSequence = new Sequence(new List<Node> {
                new CheckIsUnitMine(controller),
                new TaskTrySetDestinationOrTarget(controller),
            });

            Sequence moveToDestinationSequence = new Sequence(new List<Node> {
                new CheckHasDestination(),
                new TaskMoveToDestination(controller),
            });

            Sequence attackSequence = new Sequence(new List<Node> {
                new Inverter(new List<Node>
                {
                    new CheckTargetIsMine(controller),
                }),
                new CheckEnemyInAttackRange(controller),
                new Timer(
                    ((CharacterData)controller.representingObject.data).attackSpeed,
                    new List<Node>()
                    {
                        new TaskAttack(controller)
                    }
                ),
            });

            Sequence moveToTargetSequence = new Sequence(new List<Node> {
                new CheckHasTarget()
            });
            if (((CharacterData)controller.representingObject.data).attackDamage > 0)
            {
                moveToTargetSequence.Attach(new Selector(new List<Node> {
                    attackSequence,
                    new TaskFollow(controller),
                }));
            }
            else
            {
                moveToTargetSequence.Attach(new TaskFollow(controller));
            }
            
            _root = new Selector(new List<Node> {
                new Parallel(new List<Node> {
                    trySetDestinationOrTargetSequence,
                    new Selector(new List<Node>
                    {
                        moveToDestinationSequence,
                        moveToTargetSequence,
                    }),
                }),
                new CheckEnemyInFOVRange(controller),
            });
            /*
             * 
             */
            

            return _root;
        }
    }
}
