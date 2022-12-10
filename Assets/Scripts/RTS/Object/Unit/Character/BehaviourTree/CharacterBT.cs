using System.Collections.Generic;
using RTS.Object.Unit.BehaviourTree;
using RTS.Object.Unit.BehaviourTree.Checks;
using RTS.Object.Unit.BehaviourTree.composites;
using RTS.Object.Unit.BehaviourTree.Tasks;

namespace RTS.Object.Unit.Character.BehaviourTree
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

            var _unitController = ((UnitController)_characterController);
            
            _root = new Selector(new List<Node> 
            {
                // new TaskTrySetDestination(controller: ref controller),
                new Sequence(new List<Node>
                {
                    new CheckHasCommand(controller: ref _unitController),
                    new Selector(new List<Node>()
                    {
                        new Sequence(new List<Node>() // Movement to position
                        {
                            new CheckHasTerrainTarget(controller: ref _unitController),
                            new Selector(new List<Node>()
                            {
                                new Sequence(new List<Node>()
                                {
                                    new CheckHasArrivedToDestination(controller: ref _characterController),
                                    new CheckHasPath(ref _characterController),
                                    new TaskResetPath(ref _characterController),
                                    new TaskRemoveFinishedCommand(controller: ref _unitController)
                                }),
                                new Sequence(new List<Node>()
                                {
                                    new CheckHasTerrainTarget(controller: ref _unitController),
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
                })
            });


            
            
            return _root;
        }
    }
}