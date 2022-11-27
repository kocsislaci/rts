using GameManagers;
using UnityEngine;
using UnityEngine.Events;

namespace Unit
{
    public abstract class UnitController : MonoBehaviour
    {
        public UnitData data;
        protected Team owner;

        public Team Owner
        {
            get => owner;
            set { owner = value; }
        }

        protected float currentHealth;
        public virtual float CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                UpdateHealthBar();
                if (currentHealth <= 0) Die();
            }
        }

        public bool isSelected = false;
        [SerializeField] protected GameObject selectionCircle;
        [SerializeField] protected GameObject teamIndicator;
        [SerializeField] protected GameObject minimapIndicator;

        protected GameObject healthBar;
        [SerializeField] protected GameObject healthBarPrefab;
        [SerializeField] protected Transform healthBarCanvas;
        public UnityEvent OnDying = new();

        
        public virtual void InitialiseGameObject(Team owner)
        {
            Owner = owner;
            CurrentHealth = data.maxHealth;

            healthBarCanvas = GameObject.Find("HealthBarCanvas").GetComponent<RectTransform>();
            SetTeamIndicatorMaterial(Resources.Load<Material>(GameManager.PathToLoadTeamMaterial[owner]));
        }
        
        
        public void Select()
        {
            Select(false);
        }
        public virtual void Select(bool clearSelection)
        {
            isSelected = true;
            GameManager.MY_SELECTED_UNITS.Add(this);
        }
        public virtual void Deselect()
        {
            isSelected = false;
            GameManager.MY_SELECTED_UNITS.Remove(this);
        }
        public virtual void SetTeamIndicatorMaterial(Material material)
        {
            teamIndicator.GetComponent<MeshRenderer>().material = material;
            minimapIndicator.GetComponent<MeshRenderer>().material = material;
        }
        
        
        
        protected void UpdateHealthBar()
        {
            if (healthBar == null) return;
            Transform fill = healthBar.transform.Find("Fill");
            fill.GetComponent<UnityEngine.UI.Image>().fillAmount = CurrentHealth / data.maxHealth;
        }
        public void TakeDamage(float attackPoints)
        {
            CurrentHealth -= attackPoints;
        }
        protected void Die()
        {
            OnDying.Invoke();
        }
        
        
        
        /*
         protected List<SkillController> skillControllers;
        public List<SkillController> SkillControllers { get => skillControllers; }
         
         protected void InitializeSkillControllers()
        {
            skillControllers = new List<SkillController>();
            SkillController skillController;
            foreach (SkillData skill in data.skills)
            {
                var thisGameObject = gameObject;
                skillController = thisGameObject.AddComponent<SkillController>();
                skillController.Initialize(skill, thisGameObject);
                skillControllers.Add(skillController);
            }
        }
        public void TriggerSkill(int index, GameObject target = null)
        {
            skillControllers[index].Trigger(target);
        }*/
    }
}
