using System;
using MyRTS.Object.Unit.BehaviourTree;

namespace MyRTS.Object.Unit.Building.MainBuilding
{
    public class MainBuildingBT : BehaviourTree.Tree
    {
        private MainBuildingController _controller;

        private void Awake()
        {
            _controller = GetComponent<MainBuildingController>();
        }

        protected override Node SetupTree()
        {
            Node _root;
            
            // Setup here
            throw new NotImplementedException();
        }
    }
}
