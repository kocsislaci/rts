using System;
using Unit.BehaviourTree;
using Tree = Unit.BehaviourTree.Tree;

namespace Unit.Building
{
    public class BuildingBT : Tree
    {
        private BuildingController controller;

        private void Awake()
        {
            controller = GetComponent<BuildingController>();
        }

        protected override Node SetupTree()
        {
            Node _root;
            
            // Setup here
            throw new NotImplementedException();
        
            return _root;
        }
        
    }
}