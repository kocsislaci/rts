using System.Collections.Generic;
using UnityEngine;

namespace RTS.Object.Unit.Capabilities.General
{
    public interface ISelectable
    {
        public bool IsSelected
        {
            get;
            set;
        }
        [SerializeField]public GameObject SelectionCircle { get; }
        [SerializeField] public List<GameObject> TeamIndicatorList { get; }
        [SerializeField]public GameObject MinimapIndicator { get; }

        public void SetTeamIndicatorMaterial(Material material);

        public void Select();
        public void Deselect();
    }
}
