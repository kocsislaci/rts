using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RtsGameManager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class UiManager : MonoBehaviour
{
    public List<GameObject> ResourcesPanel = new List<GameObject>();
    public GameObject PopulationPanel;

    private List<TextMeshProUGUI> resourceTexts = new List<TextMeshProUGUI>();
    private TextMeshProUGUI populationText;

    public GameObject UnitPanel;
    private GameObject _unitPanelPrefab;

    public GameObject CommandPanel;
    public GameObject commandPrefab;

    private void Start()
    {
        InitResourcesPanel();
        InitUnitsPanel();
        this.gameObject.GetComponent<SelectionManager>().selectionEvent.AddListener(UpdateCommandPanel);
    }

    private void InitResourcesPanel()
    {
        List<Resource> resources = RtsGameManager.GameManager.Resources;
        int i = 0;
        foreach (var resourcepanel in ResourcesPanel)
        {
            resourceTexts.Add(resourcepanel.GetComponentInChildren<TextMeshProUGUI>());
            resourceTexts[i].text = resources[i].Amount.ToString();
            
            resources[i].HasChanged.AddListener(UpdateResource);
            
            string path = "Materials/UI/Resource/" + resources[i].name;
            Sprite sprite = Resources.Load<Sprite>(path); // can throw error
            resourcepanel.GetComponentInChildren<Image>().sprite = sprite;
            i++;
        }
        RtsGameManager.GameManager.population.PopulationHasChanged.AddListener(UpdatePopulation);
        RtsGameManager.GameManager.population.PopulationLimitHasChanged.AddListener(UpdatePopulationLimit);

        populationText = PopulationPanel.GetComponentInChildren<TextMeshProUGUI>();
        populationText.text = PopulationTextFormatter(RtsGameManager.GameManager.population.ActualPopulation,
                                                        RtsGameManager.GameManager.population.PopulationLimit);
        PopulationPanel.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Materials/UI/Population/population");;
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
        populationText.text = PopulationTextFormatter(population, RtsGameManager.GameManager.population.PopulationLimit);

    }
    private void UpdatePopulationLimit(int limit)
    {
        populationText.text = PopulationTextFormatter(RtsGameManager.GameManager.population.ActualPopulation, limit);
    }
    private void InitUnitsPanel()
    {
        this.gameObject.GetComponent<SelectionManager>().selectionEvent.AddListener(UpdateUnitPanel);
    }
    private void UpdateUnitPanel(SelectionController sc)
    {
        Debug.Log("UpdateUnitPanel");
    }
    private void UpdateCommandPanel(SelectionController sc)
    {
        Debug.Log("UpdateCommandPanel");
    }
    
    private List<Button> commandButtons = new List<Button>();
    // private Description to show TODO: desc prefab with binding, desc class with
    //                                   corresponding data and funcs to fill prefab
    
    // func to refresh cmds
    
    // func to refresh desc
}
