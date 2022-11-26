namespace Unit.Skill
{
    public enum SkillType
    {
        /*
         * Building skills
         */
        CreateCharacter, // Later distinguish to the following
        // CreateBuilder
        
        // CreateSoldier
        // CreateRemoteSoldier
        // Create ThirdKindOfSoldier
        
        // Technology Upgrades
        // Attack, MovementSpeed, Defense, GatherSpeed
        
        /*
         * Unit skills
         */
        // Change mood
        ChangeMood, // Gatherer, Attacker, Builder
        
        // Movement
        SetDestination,
        
        // Follow
        SetTarget, // enemy, ally, neutral
        
        // Attack
        Attack,
        
        // Build
        Build, // actually build anything
        
        // Gathering
        GetResource,
        PutResource,
        
        // Building, These are the actions // manifesting on ui and can be placed then the unit is sent there with SetDestination then Build
        BuildMainBuilding, 
        BuildHouse,
        BuildTower,
        BuildWall,
    }
}
