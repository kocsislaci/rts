using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagers
{
    public class UiManager : MonoBehaviour
    {
        [Header("Resources and population panel")]
        [SerializeField] private List<GameObject> resourcesPanel = new List<GameObject>();
        [SerializeField] private GameObject populationPanel;

        [Header("Command and unit panel")]
        [SerializeField] private GameObject commandPanel;
        private List<GameObject> commandPanelButtons = new();
        [SerializeField] private GameObject commandButtonPrefab;
        [SerializeField] private GameObject unitListPanel;
        [SerializeField] private GameObject unitDescriptionPanel;

        private List<TextMeshProUGUI> resourceTexts = new List<TextMeshProUGUI>();
        private TextMeshProUGUI populationText;

        [CanBeNull] private Unit.Unit firstSelectedUnit;
        private SelectionManager selectionManager;
        
        private void Start()
        {
            InitResourcesPanel();
            selectionManager = gameObject.GetComponent<SelectionManager>();
            selectionManager.selectionEvent.AddListener(UpdateCommandPanel);
            selectionManager.deselectionEvent.AddListener(ClearCommandPanel);
        }

        private void InitResourcesPanel()
        {
            var resources = GameManagers.GameManager.Resources;
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
            GameManagers.GameManager.Population.OnPopulationHasChanged.AddListener(UpdatePopulation);
            GameManagers.GameManager.Population.OnPopulationLimitHasChanged.AddListener(UpdatePopulationLimit);

            populationText = populationPanel.GetComponentInChildren<TextMeshProUGUI>();
            populationText.text = PopulationTextFormatter(GameManagers.GameManager.Population.ActualPopulation,
                GameManagers.GameManager.Population.PopulationLimit);
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
                int i = 0;
                for (; i < unit.SkillControllers.Count; i++)
                {
                    var g = Instantiate(commandButtonPrefab, commandPanel.transform);
                    commandPanelButtons.Add(g);
                    
                    firstSelectedUnit.SkillControllers[i].SetButton(g.GetComponent<Button>());
                    var i1 = i;
                    g.GetComponent<Button>().onClick.AddListener(() => firstSelectedUnit.TriggerSkill(i1));
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
        
    }
}
