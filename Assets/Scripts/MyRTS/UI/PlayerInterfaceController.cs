using System.Collections.Generic;
using MyRTS.Object.Resource;
using MyRTS.Object.Resource.Dto;
using MyRTS.Player;
using TMPro;
using UnityEngine;

namespace MyRTS.UI
{
    public class PlayerInterfaceController : MonoBehaviour
    {
        [Header("Resources and population panel")]
        [SerializeField] private Dictionary<ResourceType, TextMeshProUGUI> resourceTexts = new();
        [SerializeField] private TextMeshProUGUI populationText;
        [Header("Actions, units and description panel")]
        [Header("Available actions are in:")]
        [SerializeField] private RectTransform commandPanel;
        [Header("Unit icons are in:")]
        [SerializeField] private RectTransform unitListPanel;
        [Header("Unit description data are in:")]
        [SerializeField] private RectTransform unitDescriptionPanel;
        [SerializeField] private RectTransform descriptionPanelIconPanel;
        [SerializeField] private List<TextMeshProUGUI> descriptionFields = new();

        /// /////////////////
        private void Awake()
        {
            resourceTexts.Add(ResourceType.Gold, GameObject.Find("GoldResourcePanel").GetComponentInChildren<TextMeshProUGUI>());
            resourceTexts.Add(ResourceType.Stone, GameObject.Find("StoneResourcePanel").GetComponentInChildren<TextMeshProUGUI>());
            resourceTexts.Add(ResourceType.Wood, GameObject.Find("WoodResourcePanel").GetComponentInChildren<TextMeshProUGUI>());
        }


        public void InitialisePlayerInterfaceController(PlayerManager playerInterfaceManager)
        {
            playerInterfaceManager.PlayerStateChanged.AddListener(OnUpdate);
        }

        public void OnUpdate(PlayerInterfaceDto incomingData)
        {
            if (incomingData.resourceValues != null)
                foreach (var incomingResource in incomingData.resourceValues)
                {
                    resourceTexts[incomingResource.Key].text = incomingResource.Value.ToString();
                }
            if (incomingData.population != null)
                populationText.text = incomingData.population.Item1 + "/" + incomingData.population.Item2;
            for (var i = 0; i < commandPanel.childCount; i++)
            {
                Destroy(commandPanel.GetChild(i).gameObject);
            }
            
            if (incomingData.availableActionButtons != null)
                foreach (var availableActionButton in incomingData.availableActionButtons)
                {
                    availableActionButton.parent = commandPanel;
                }
            for (var i = 0; i < unitListPanel.childCount; i++)
            {
                Destroy(unitListPanel.GetChild(i).gameObject);
            }
            if (incomingData.selectedUnitIcons != null)
                foreach (var selectedUnitIcon in incomingData.selectedUnitIcons)
                {
                    selectedUnitIcon.parent = unitListPanel;
                }
        }
    }
}