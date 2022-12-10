using System;
using RTS.Object.Unit.BehaviourTree;

namespace RTS.Object.Unit.Building.MainBuilding
{
    public class MainBuildingBT : BehaviourTree.Tree
    {
        private MainBuildingController controller;

        private void Awake()
        {
            controller = GetComponent<MainBuildingController>();
        }

        protected override Node SetupTree()
        {
            Node _root;
            
            // Setup here
            throw new NotImplementedException();
        }
    }
}
