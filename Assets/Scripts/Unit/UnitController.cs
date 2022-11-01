using System;
using UnityEngine;

namespace Unit
{
    public abstract class UnitController : MonoBehaviour
    {
        [SerializeField] private GameObject teamIndicator;

        public Unit selfClass;
        
        private Material blueMaterial;
        private Material redMaterial;

        private void Awake()
        {
            blueMaterial = Resources.Load<Material>("Materials/Unit/BlueTeamMaterial");
            redMaterial = Resources.Load<Material>("Materials/Unit/RedTeamMaterial");
        }

        public virtual void InitialiseGameObject(TeamEnum owner, Unit selfClass)
        {
            this.selfClass = selfClass;
            switch (owner)
            {
                case TeamEnum.Blue:
                    teamIndicator.GetComponent<MeshRenderer>().material = blueMaterial;
                    break;
                case TeamEnum.Red:
                    teamIndicator.GetComponent<MeshRenderer>().material = redMaterial;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(owner), owner, null);
            }
        }
    }
}