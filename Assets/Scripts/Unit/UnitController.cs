using GameManagers;
using UnityEngine;

namespace Unit
{
    public abstract class UnitController : MonoBehaviour
    {
        public Unit representingObject;

        protected GameObject HealthBar;
        [SerializeField] protected GameObject healthBarPrefab;
        [SerializeField] protected Transform healthBarCanvas;
        
        public bool isSelected = false;
        [SerializeField] protected GameObject selectionCircle;

        [SerializeField] protected GameObject teamIndicator;
        
        protected void Start()
        {
            healthBarCanvas = GameObject.Find("HealthBarCanvas").GetComponent<RectTransform>();
        }
        public virtual void InitialiseGameObject(Team owner, Unit caller)
        {
            representingObject = caller;
            teamIndicator.GetComponent<MeshRenderer>().material = Resources.Load<Material>(GameManager.PathToLoadTeamMaterial[owner]);
        }
        
        
        protected void UpdateHealthBar()
        {
            if (HealthBar == null) return;
            Transform fill = HealthBar.transform.Find("Fill");
            fill.GetComponent<UnityEngine.UI.Image>().fillAmount = representingObject.CurrentHealth / (float)representingObject.data.maxHealth;
        }
        
        
        public void Select()
        {
            Select(false);
        }
        public virtual void Select(bool clearSelection)
        {
            isSelected = true;
            GameManager.SELECTED_UNITS.Add(this);
        }
        public virtual void Deselect()
        {
            isSelected = false;
            GameManager.SELECTED_UNITS.Remove(this);
        }
        
        
        // Here come every gameObject manager function, right?
        
        
        public void TakeHit(int attackPoints)
        {
            representingObject.CurrentHealth -= attackPoints;
            UpdateHealthBar();
            if (representingObject.CurrentHealth <= 0) Die();

        }
        protected void Die()
        {
            Destroy(gameObject);
        }
    }
}
