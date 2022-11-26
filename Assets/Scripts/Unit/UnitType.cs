namespace Unit
{
    public enum UnitType
    {
        /*
         * White label but never used
         */
        Unit, // Root of Building and Character
        
        /*
         * Characters
         */
        Character, // Root of characters
        // CloseCombatSoldier
        // RangedSoldier
        // ThirdKindOfSoldier
        // BuilderGatherer
        
        /*
         * White label but never used
         */
        Building, // Root of buildings
        /*
         * Buildings
         */
        MainBuilding,
        House,
        Tower,
        Wall,

        /*
         * White label but never used
         */
        Resource, // Root of resources
        /*
         * Resources
         */
        Gold,
        Stone,
        Wood,
    }
}
