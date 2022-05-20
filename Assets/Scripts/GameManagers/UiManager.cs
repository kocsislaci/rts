using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    public List<GameObject> ResourcesPanel = new List<GameObject>();
    public GameObject PopulationPanel;

    private List<TextMeshProUGUI> resourceTexts = new List<TextMeshProUGUI>();
    private TextMeshProUGUI populationText;

    public GameObject UnitPanel;
    private GameObject _unitPanelPrefab;
    
    private void Start()
    {
        InitResourcesPanel();
        InitUnitsPanel();

    }

    private void InitResourcesPanel()
    {
        List<Resource> resources = RtsGameManager.GameManager.Resources;
        int i = 0;
        foreach (var resourcepanel in ResourcesPanel)
        {
            resourceTexts.Add(resourcepanel.GetComponentInChildren<TextMeshProUGUI>());
            resourceTexts[i].text = resources[i].Amount.ToString();
            
            resources[i].HasChanged.AddListener(UpdateUpperBar); //TODO
            
            string path = "Materials/UI/Resource/" + resources[i].name;
            Sprite sprite = Resources.Load<Sprite>(path); // can throw error
            resourcepanel.GetComponentInChildren<Image>().sprite = sprite;
            i++;
        }
        RtsGameManager.GameManager.population.PopulationHasChanged.AddListener(UpdateUpperBar);
        RtsGameManager.GameManager.population.PopulationLimitHasChanged.AddListener(UpdateUpperBar);

        populationText = PopulationPanel.GetComponentInChildren<TextMeshProUGUI>();
        populationText.text = PopulationTextFormatter(RtsGameManager.GameManager.population.ActualPopulation,
                                                        RtsGameManager.GameManager.population.PopulationLimit);
        PopulationPanel.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Materials/UI/Population/population");;
    }

    private string PopulationTextFormatter(int actual, int limit)
    {
        return actual.ToString() + "/" + limit.ToString();
    }

    private void UpdateUpperBar() 
    {
        for (int i = 0; i < resourceTexts.Count; i++)
        {
            resourceTexts[i].text = RtsGameManager.GameManager.Resources[i].ToString();
        }
        populationText.text = PopulationTextFormatter(RtsGameManager.GameManager.population.ActualPopulation,
            RtsGameManager.GameManager.population.PopulationLimit);
    }

    private void InitUnitsPanel()
    {
        this.gameObject.GetComponent<UnitsSelectionManager>().selectionEvent.AddListener(UpdateUnitPanel);
        _unitPanelPrefab = Resources.Load<GameObject>("UI/UnitPanel");
        UpdateUnitPanel();
    }
    
    
    private void UpdateUnitPanel()
    {
        var children = new List<GameObject>();
        foreach (Transform child in UnitPanel.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        
        
        int length = RtsGameManager.GameManager.SELECTED_UNITS.Count;
        for (int i = 0; i < length; i++)
        {
            GameObject unitPanel = Instantiate(_unitPanelPrefab, UnitPanel.transform);
            //unitPanel.GetComponent<TextMeshProUGUI>().text = RtsGameManager.GameManager.SELECTED_UNITS[i].gameObject
               // .GetComponent<UnitController>().;
        }
    }
    
    
    
    
    
    
    private List<Button> commandButtons = new List<Button>();
    // private Description to show TODO: desc prefab with binding, desc class with
    //                                   corresponding data and funcs to fill prefab
    
    // func to refresh cmds
    
    // func to refresh desc
}
