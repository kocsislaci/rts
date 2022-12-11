namespace MyRTS.Object.Unit.Capabilities
{
    public enum ActionType
    {
        TrainerAction,
        BuilderAction,
        
        /*
         * Building skills
         */
        // creator actions
        // TrainCharacter, // Later distinguish to the following
        // TrainPeasant,
        // TrainSoldiers,
        // TrainRangedSoldier,
        // TrainThirdKindOfSoldier,

        // technology tree actions
        // Technology Upgrades
        // UpgradeMovementSpeed
        // UpgradeAttackPower
        // UpgradeDefense
        // UpgradeGatherSpeed
        
        /*
         * Character actions
         */
        // creator actions
        // BuildMainBuilding, 
        // BuildHouse,
        // BuildTower,
        // BuildWall,
        // // Behaviour action
        // ToggleStance, // Aggressive, Neutral, Peaceful
    }
}
