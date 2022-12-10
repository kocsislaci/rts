using System.Collections.Generic;
using JetBrains.Annotations;
using RTS.Object.Resource;
using RTS.Object.Unit;
using RTS.Object.Unit.Capabilities.Movable;
using UnityEngine;

namespace RTS.Player.Commands
{
    public class CommandDto
    {
        public CommandDto(
            Vector3? pClickedPlaceOnTerrain = null,
            [CanBeNull] UnitController pClickedUnit = null,
            [CanBeNull] ResourceController pClickedResource = null
        )
        {
            clickedPlaceOnTerrain = pClickedPlaceOnTerrain;
            clickedUnit = pClickedUnit;
            clickedResource = pClickedResource;
        }
        public Vector3? clickedPlaceOnTerrain;
        [CanBeNull] public UnitController clickedUnit;
        [CanBeNull] public ResourceController clickedResource;
        [CanBeNull] public Dictionary<Vector3, IMovable> fellowMovingUnits;
    }
}
