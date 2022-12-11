using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace MyRTS.Object.Resource.Dto
{
    public record PlayerInterfaceDto
    {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="resourceValues"></param>
    /// <param name="population"></param>
    /// <param name="availableActionButtons"></param>
    /// <param name="selectedUnitIcons"></param>
    public PlayerInterfaceDto(
        [CanBeNull] Dictionary<ResourceType, int> resourceValues = null,
        [CanBeNull] Tuple<int, int> population = null,
        [CanBeNull] List<RectTransform> availableActionButtons = null,
        [CanBeNull] List<RectTransform> selectedUnitIcons = null
    )
    {
        this.resourceValues = resourceValues;
        this.population = population;
        this.availableActionButtons = availableActionButtons;
        this.selectedUnitIcons = selectedUnitIcons;
    }
    [CanBeNull] public Dictionary<ResourceType, int> resourceValues;
    [CanBeNull] public Tuple<int, int> population;
    [CanBeNull] public List<RectTransform> availableActionButtons;
    [CanBeNull] public List<RectTransform> selectedUnitIcons;
    }
}