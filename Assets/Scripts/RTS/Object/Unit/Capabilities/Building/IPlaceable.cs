using System.Collections.Generic;
using RTS.Object.Unit.Building;
using UnityEngine;

namespace RTS.Object.Unit.Capabilities.Building
{
    public interface IPlaceable
    {
    public BuildingPlacementStatus BuildingPlacementStatus { get; set; }
    public int Collosions { get; set; }
    protected internal BoxCollider Collider { get; }
    protected internal List<GameObject> BuildingParts { get; }
    public void OnTriggerEnter(Collider other);
    public void OnTriggerExit(Collider other);
    public void Placement();
    }
}
