using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unit;
using Unit.ResourceObject;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagers
{
    public class UserInterfaceManager : MonoBehaviour
    {
        [Header("Resources and population panel")] [SerializeField]
        private Dictionary<ResourceType, GameObject> resourcesPanel = new();
        [SerializeField] private GameObject populationPanel;

        [Header("Command and unit panel")]
        [SerializeField] private GameObject commandPanel;
        private List<GameObject> commandPanelButtons = new();
        private List<GameObject> unitListPanelIcons = new();
        [SerializeField] private GameObject unitListPanel;
        [SerializeField] private GameObject unitDescriptionPanel;

        private Dictionary<ResourceType, TextMeshProUGUI> resourceTexts = new();
        private TextMeshProUGUI populationText;

        [CanBeNull] private UnitController firstSelectedUnit;
        private List<UnitController> selectedUnits = new();
        private UserInputManager.UserInputManager _userInputManager;
        
        private void Start()
        {
            InitResourcesPanel();
            _userInputManager = gameObject.GetComponent<UserInputManager.UserInputManager>();
            
            // _userInputManager.selectionEvent.AddListener(UpdateCommandPanel);
            // _userInputManager.deselectionEvent.AddListener(ClearCommandPanel);
            
            _userInputManager.selectionEvent.AddListener(UpdateUnitListPanel);
            _userInputManager.deselectionEvent.AddListener(ClearUnitListPanel);
        }

        private void InitResourcesPanel()
        {
            var resources = GameManager.MyResources;
            resourcesPanel.Add(ResourceType.Gold ,GameObject.Find("GoldResourcePanel"));
            resourcesPanel.Add(ResourceType.Stone ,GameObject.Find("StoneResourcePanel"));
            resourcesPanel.Add(ResourceType.Wood ,GameObject.Find("WoodResourcePanel"));

            foreach (var resourcePanel in resourcesPanel)
            {
                resourceTexts.Add(resourcePanel.Key, resourcePanel.Value.GetComponentInChildren<TextMeshProUGUI>());
                resourceTexts[resourcePanel.Key].text = resources[resourcePanel.Key].Amount.ToString();
            
                resources[resourcePanel.Key].HasChanged.AddListener(UpdateResource);
            
                Sprite sprite = UnityEngine.Resources.Load<Sprite>(GameManager.PathToLoadResourceIcon[resourcePanel.Key]);
                resourcePanel.Value.GetComponentInChildren<Image>().sprite = sprite;
            }
            GameManager.MyPopulation.OnPopulationHasChanged.AddListener(UpdatePopulation);
            GameManager.MyPopulation.OnPopulationLimitHasChanged.AddListener(UpdatePopulationLimit);

            populationText = populationPanel.GetComponentInChildren<TextMeshProUGUI>();
            populationText.text = PopulationTextFormatter(GameManager.MyPopulation.ActualPopulation,
                GameManager.MyPopulation.PopulationLimit);
            populationPanel.GetComponentInChildren<Image>().sprite = UnityEngine.Resources.Load<Sprite>(GameManager.PathToLoadPopulationIcon); 
        }

        private string PopulationTextFormatter(int actual, int limit)
        {
            return actual.ToString() + "/" + limit.ToString();
        }

        private void UpdateResource(ResourceType type, int amount) 
        {
            resourceTexts[type].text = amount.ToString();
        }
        private void UpdatePopulation(int population)
        {
            populationText.text = PopulationTextFormatter(population, GameManager.MyPopulation.PopulationLimit);

        }
        private void UpdatePopulationLimit(int limit)
        {
            populationText.text = PopulationTextFormatter(GameManager.MyPopulation.ActualPopulation, limit);
        }
        
        
        /*private void UpdateCommandPanel(Unit.Unit unit)
        {
            if (firstSelectedUnit == null)
            {
                firstSelectedUnit = unit;
                for (int i = 0; i < firstSelectedUnit.SkillControllers.Count; i++)
                {
                    var skillType = firstSelectedUnit.SkillControllers[i].skill.skillType;
                    var bgo = UnityEngine.Resources.Load(GameManager.PathToLoadButton[skillType]) as GameObject;
                    var g = Instantiate(bgo, commandPanel.transform);
                    commandPanelButtons.Add(g);
                    
                    firstSelectedUnit.SkillControllers[i].SetButton(g.GetComponent<Button>());
                    var indexOfSkill = i;
                    g.GetComponent<Button>().onClick.AddListener(() => firstSelectedUnit.TriggerSkill(indexOfSkill));
                }
            }
        }
        private void ClearCommandPanel()
        {
            firstSelectedUnit = null;
            foreach (var button in commandPanelButtons)
            {
                Destroy(button);
            }
        }*/

        private void UpdateUnitListPanel(UnitController unit)
        {
            selectedUnits.Add(unit);

            foreach (var selectedUnit in selectedUnits)
            {
                var igo = UnityEngine.Resources.Load(GameManager.PathToLoadIconPrefab[selectedUnit.data.type]) as GameObject;
                unitListPanelIcons.Add(Instantiate(igo, unitListPanel.transform));
            }
        }

        private void ClearUnitListPanel()
        {
            selectedUnits.Clear();
            foreach (var unitListPanelIcon in unitListPanelIcons)
            {
                Destroy(unitListPanelIcon);
            }
        }
    }
}
