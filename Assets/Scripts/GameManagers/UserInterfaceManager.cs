using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagers
{
    public class UserInterfaceManager : MonoBehaviour
    {
        [Header("Resources and population panel")]
        [SerializeField] private List<GameObject> resourcesPanel = new List<GameObject>();
        [SerializeField] private GameObject populationPanel;

        [Header("Command and unit panel")]
        [SerializeField] private GameObject commandPanel;
        private List<GameObject> commandPanelButtons = new();
        private List<GameObject> unitListPanelIcons = new();
        [SerializeField] private GameObject unitListPanel;
        [SerializeField] private GameObject unitDescriptionPanel;

        private List<TextMeshProUGUI> resourceTexts = new List<TextMeshProUGUI>();
        private TextMeshProUGUI populationText;

        [CanBeNull] private Unit.Unit firstSelectedUnit;
        private List<Unit.Unit> selectedUnits = new();
        private UserInputManager.UserInputManager _userInputManager;
        
        private void Start()
        {
            InitResourcesPanel();
            _userInputManager = gameObject.GetComponent<UserInputManager.UserInputManager>();
            
            _userInputManager.selectionEvent.AddListener(UpdateCommandPanel);
            _userInputManager.deselectionEvent.AddListener(ClearCommandPanel);
            
            _userInputManager.selectionEvent.AddListener(UpdateUnitListPanel);
            _userInputManager.deselectionEvent.AddListener(ClearUnitListPanel);
        }

        private void InitResourcesPanel()
        {
            var resources = GameManager.Resources;
            int i = 0;
            foreach (var resourcePanel in resourcesPanel)
            {
                resourceTexts.Add(resourcePanel.GetComponentInChildren<TextMeshProUGUI>());
                resourceTexts[i].text = resources[i].Amount.ToString();
            
                resources[i].HasChanged.AddListener(UpdateResource);
            
                string path = "Materials/UI/Resource/" + resources[i].type;
                Sprite sprite = UnityEngine.Resources.Load<Sprite>(path);
                resourcePanel.GetComponentInChildren<Image>().sprite = sprite;
                i++;
            }
            GameManager.Population.OnPopulationHasChanged.AddListener(UpdatePopulation);
            GameManager.Population.OnPopulationLimitHasChanged.AddListener(UpdatePopulationLimit);

            populationText = populationPanel.GetComponentInChildren<TextMeshProUGUI>();
            populationText.text = PopulationTextFormatter(GameManager.Population.ActualPopulation,
                GameManager.Population.PopulationLimit);
            populationPanel.GetComponentInChildren<Image>().sprite = UnityEngine.Resources.Load<Sprite>("Materials/UI/Population/population");;
        }

        private string PopulationTextFormatter(int actual, int limit)
        {
            return actual.ToString() + "/" + limit.ToString();
        }

        private void UpdateResource(int amount) 
        {
            for (int i = 0; i < resourceTexts.Count; i++)
            {
                resourceTexts[i].text = amount.ToString();
            }

        }
        private void UpdatePopulation(int population)
        {
            populationText.text = PopulationTextFormatter(population, GameManager.Population.PopulationLimit);

        }
        private void UpdatePopulationLimit(int limit)
        {
            populationText.text = PopulationTextFormatter(GameManager.Population.ActualPopulation, limit);
        }
        
        
        private void UpdateCommandPanel(Unit.Unit unit)
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
        }

        private void UpdateUnitListPanel(Unit.Unit unit)
        {
            selectedUnits.Add(unit);

            foreach (var selectedUnit in selectedUnits)
            {
                var igo = UnityEngine.Resources.Load(GameManager.PathToLoadIconPrefab[selectedUnit.unitType]) as GameObject;
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
