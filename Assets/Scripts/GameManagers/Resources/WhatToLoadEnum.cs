namespace GameManagers.Resources
{
    public enum WhatToLoadEnum
    
    {
        /*
         * White label but never used
         */
        Unit, // Root of Building and Character
        
        /*
         * Characters
         */
        Character, // Root of characters
        
        /*
         * Buildings
         */
        Building, // Root of buildings
        MainBuilding,
        House,
        Tower,
        Wall,

        /*
         * Resources
         */
        Resource, // Root of resources
        Gold,
        Stone,
        Wood,
    }
}
