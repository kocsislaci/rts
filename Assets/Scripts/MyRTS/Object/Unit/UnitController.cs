using System.Collections.Generic;
using JetBrains.Annotations;
using MyRTS.GameManagers;
using MyRTS.Object.Unit.Capabilities.General;
using MyRTS.Player;
using MyRTS.Player.Commands;
using UnityEngine;

namespace MyRTS.Object.Unit
{
    public abstract class UnitController : ObjectController, ICommandable, IAttackable, ISelectable
    {
        public PlayerManager Owner { get => owner; protected set => owner = value; }
        [Space(8f)][Header("Unit stuff")][SerializeField] protected PlayerManager owner;
        public UnitData Data { get => data; protected set => data = value; }
        [SerializeField] protected UnitData data;
        
        
        public virtual void InitialiseUnit(PlayerManager pOwner, UnitType type, [CanBeNull] CommandDto initialCommandDto = null)
        {
            InitialiseObject(ObjectType.Resource);
            Data = Resources.Load<UnitData>(GameResources.PathToLoadUnitData[type]);
            
            Owner = pOwner;
            CurrentHealth = Data.maxHealth;
            IsSelected = false;

            Owner.MyUnits.Add(Uuid, this);
            SetTeamIndicatorMaterial(Resources.Load<Material>(GameResources.PathToLoadTeamMaterial[Owner.Faction]));
        }
        public override void Destroy()
        {
            Owner.MyUnits.Remove(Uuid);
            base.Destroy();
        }
        
        /*
         * ICommandable
         */
        [field: Space(8f)][field: Header("Commandable")][field: SerializeField] public List<CommandDto> ReceivedCommands { get; set; } = new();
        public void AddCommandToOverwrite(CommandDto command)
        {
            ClearEveryCommand();
            AddCommandToQueue(command);
        }
        public void AddCommandToQueue(CommandDto command)
        {
            ReceivedCommands.Add(command);
        }
        public void ClearEveryCommand()
        {
            if (ReceivedCommands.Count > 0)
                ReceivedCommands.Clear();
        }
        public void FinishCommand()
        {
            if (ReceivedCommands.Count > 0)
                ReceivedCommands.RemoveAt(0);
        }


        /*
         * IAttackable
         */
        [Space(8f)][Header("IAttackable")][SerializeField] protected float currentHealth;
        public virtual float CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                if (currentHealth <= 0) ((IAttackable)this).Die();
            }
        }
        public void TakeDamage(float attackPoints)
        {
            CurrentHealth -= attackPoints;
        }
        void IAttackable.Die()
        {
            Destroy();
        }
        
        /*
         * ISelectable
         */
        
        [SerializeField] public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value)
                    selectionCircle.SetActive(true);
                else
                    selectionCircle.SetActive(false);
                isSelected = value;
            }
        }
        protected bool isSelected;

        public GameObject SelectionCircle => selectionCircle;
        [Space(8f)][Header("IAttackable")][SerializeField] protected GameObject selectionCircle;
        public List<GameObject> TeamIndicatorList => teamIndicatorList;
        [SerializeField] protected List<GameObject> teamIndicatorList;
        public GameObject MinimapIndicator => minimapIndicator;
        [SerializeField] protected GameObject minimapIndicator;
        public void SetTeamIndicatorMaterial(Material material)
        {
            foreach (var teamIndicatorGameObject in TeamIndicatorList)
            {
                teamIndicatorGameObject.GetComponent<MeshRenderer>().material = material;
            }
            MinimapIndicator.GetComponent<MeshRenderer>().material = material;
        }
        public virtual void Select()
        {
            IsSelected = true;
        }
        public virtual void Deselect()
        {
            IsSelected = false;
        }

    }
}