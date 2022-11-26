using System.Collections.Generic;
using GameManagers;
using UI;
using Unit.Character;
using UnityEngine;

namespace Unit.Building
{
    public class BuildingController : UnitController
    {
        [SerializeField] public GameObject spawnPoint;
        
        private int collosions = 0;
        [SerializeField] private BoxCollider placerCollider;
        [SerializeField] private List<GameObject> buildingParts;
        
        public override void InitialiseGameObject(Team owner, Unit caller)
        {
            base.InitialiseGameObject(owner, caller);
            placerCollider.isTrigger = true;

            if (((Building)caller).BuildingStatus == BuildingStatus.PENDING)
            {
                ChangeMaterial(((Building)caller).BuildingPlacementStatus);
            }
            else
            {
                foreach (var buildingPart in buildingParts)
                {
                    buildingPart.GetComponent<MeshRenderer>().material =
                        Resources.Load<Material>(GameManager.PathToLoadBuildingMaterial);
                }
                teamIndicator.GetComponent<MeshRenderer>().material = Resources.Load<Material>(GameManager.PathToLoadTeamMaterial[((Building)representingObject).Owner]);
            }
            transform.localScale = new Vector3(1,
                (float)((Building)representingObject).CurrentHealth / ((Building)representingObject).data.maxHealth, 1);
        }

        

        public override void Select(bool clearSelection)
        {
            if (GameManager.SELECTED_BUILDINGS.Contains(this)) return;
            if (clearSelection)
            {
                var selectedUnits = new List<Character.CharacterController>(GameManager.SELECTED_CHARACTERS);
                foreach (var selectedUnit in selectedUnits)
                    selectedUnit.Deselect();
            }
            GameManager.SELECTED_BUILDINGS.Add(this);
            
            /*
         * Set healthBar
         */
            HealthBar = Instantiate(healthBarPrefab, healthBarCanvas, true);
            HealthBar healthBarComponent = HealthBar.GetComponent<HealthBar>();
            healthBarComponent.Initialize(transform);
            healthBarComponent.SetPosition();
            
            /*
             * Set circle
             */
            selectionCircle.SetActive(true);
        }
        public override void Deselect()
        {
            if (!GameManager.SELECTED_BUILDINGS.Contains(this)) return;
            GameManager.SELECTED_BUILDINGS.Remove(this);
                
            /*
        * Off healthBar
        */
            Destroy(HealthBar);
            HealthBar = null;

            /*
         * Off selection circle
         */
            selectionCircle.SetActive(false);
        }
        
        
        
        
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Terrain")) return;
            collosions++;
            if (collosions == 1)
            {
                ((Building)representingObject).BuildingPlacementStatus = BuildingPlacementStatus.INVALID;
                ChangeMaterial(BuildingPlacementStatus.INVALID);
            }
        }
        protected void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Terrain")) return;
            collosions--;
            if (collosions == 0)
            {
                ((Building)representingObject).BuildingPlacementStatus = BuildingPlacementStatus.VALID;
                ChangeMaterial(BuildingPlacementStatus.VALID);
            }
        }
        protected void ChangeMaterial(BuildingPlacementStatus status)
        {
            foreach (var buildingPart in buildingParts)
            {
                buildingPart.GetComponent<MeshRenderer>().material =
                    Resources.Load<Material>(
                        GameManager.PathToLoadBuildingPlacementStatusMaterial[status]);
            }
        }
        
        public void Placement()
        {
            placerCollider.isTrigger = false;
            ((Building)representingObject).BuildingStatus = BuildingStatus.PLACED;
            
            foreach (var buildingPart in buildingParts)
            {
                buildingPart.GetComponent<MeshRenderer>().material =
                    Resources.Load<Material>(GameManager.PathToLoadBuildingMaterial);
            }
            teamIndicator.GetComponent<MeshRenderer>().material = Resources.Load<Material>(GameManager.PathToLoadTeamMaterial[((Building)representingObject).Owner]);
        }
        public void GetBuilt(Character.Character builder)
        {
            if (((Building)representingObject).BuildingStatus == BuildingStatus.PLACED)
            {
                ((Building)representingObject).CurrentHealth += ((CharacterData)builder.data).buildingUnit;
                transform.localScale = new Vector3(1, (float)((Building)representingObject).CurrentHealth / ((Building)representingObject).data.maxHealth, 1);
                if (((Building)representingObject).CurrentHealth == ((Building)representingObject).data.maxHealth)
                {
                    ((Building)representingObject).BuildingStatus = BuildingStatus.BUILT;
                }
            }
            else
            {
                Debug.Log("not placed or already built");
            }
        }
    }
}
