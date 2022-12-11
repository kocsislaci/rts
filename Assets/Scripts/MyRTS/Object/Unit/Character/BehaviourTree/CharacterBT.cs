using System.Collections.Generic;
using MyRTS.Object.Unit.BehaviourTree;
using MyRTS.Object.Unit.BehaviourTree.Checks;
using MyRTS.Object.Unit.BehaviourTree.composites;
using MyRTS.Object.Unit.BehaviourTree.Tasks;
using MyRTS.Object.Unit.Capabilities.General;

namespace MyRTS.Object.Unit.Character.BehaviourTree
{
    public class CharacterBT: Tree
    {
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        protected override Node SetupTree()
        {
            Node _root;

            UnitController unitController = _characterController;
            ITargeter targeter = _characterController;

            
            _root = new Selector(new List<Node> 
            {
                new Sequence(new List<Node>
                {
                    new CheckHasCommand(controller: ref unitController),
                    new Selector(new List<Node>()
                    {
                        new Sequence(new List<Node>() // Movement to position
                        {
                            new CheckHasTerrainTarget(controller: ref unitController),
                            new Selector(new List<Node>()
                            {
                                new Sequence(new List<Node>()
                                {
                                    new CheckHasArrivedToDestination(controller: ref _characterController),
                                    new CheckHasPath(ref _characterController),
                                    new TaskResetPath(ref _characterController),
                                    new TaskRemoveFinishedCommand(controller: ref unitController)
                                }),
                                new Sequence(new List<Node>()
                                {
                                    new CheckHasTerrainTarget(controller: ref unitController),
                                    new TaskSetDestinationToTerrainTarget(controller: ref _characterController)
                                }),
                            })
                        }),
                        new Sequence(new List<Node>() { // attack build follow
                            // hasunitTarget,
                            new Selector(new List<Node>()
                            {
                                new Sequence(new List<Node>()
                                {
                                    //istargetin range
                                    new Selector(new List<Node>()
                                    {
                                        new Sequence(new List<Node>() {}),// attack
                                        new Sequence(new List<Node>() {})// build
                                    })
                                }),
                                //new SetDestinationToUnitTarget()
                            })
                        }),
                        new Sequence(new List<Node>() { // harvest
                            // hasResourceTarget
                        }),
                    })
                }),
                new TaskTryGetEnemyFromFOV(ref targeter),
            });
            return _root;
        }
    }
}