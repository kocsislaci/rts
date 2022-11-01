using UnityEngine;

namespace Unit
{
    public abstract class UnitSelectionController : MonoBehaviour
    {
        /*
         * Under Object
         */
        [SerializeField] protected GameObject selectionCircle;
        
        /*
         * Over Object
         */
        [SerializeField] protected Transform healthBarCanvas;
        [SerializeField] protected GameObject healthBarPrefab;
        protected GameObject HealthBar;
        
        /*
         * On the UI
         */
        [SerializeField] protected Transform unitListPanel;
        [SerializeField] protected GameObject iconPrefab;
        protected GameObject Icon;

        private void Start()
        {
            healthBarCanvas = GameObject.Find("HealthBarCanvas").GetComponent<RectTransform>();
            unitListPanel = GameObject.Find("UnitListPanel").GetComponent<RectTransform>();
        }

        public void Select()
        {
            
            Select(false);
        }

        public virtual void Select(bool clearSelection)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Deselect()
        {
            throw new System.NotImplementedException();
        }
    }
}
