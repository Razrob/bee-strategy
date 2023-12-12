using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_GameplayMain : UIScreen
{
    [Serializable]
    private struct SomeResourcePrint
    {
        public Image Icon;
        public TextMeshProUGUI value;
    }
    
    [SerializeField] private SomeResourcePrint pollen;
    [SerializeField] private SomeResourcePrint wax;
    [SerializeField] private SomeResourcePrint housing;
    [SerializeField] private SomeResourcePrint honey;

    private void Start()
    {
        pollen.Icon.sprite = ResourceGlobalStorage.GetResource(ResourceID.Pollen).Icon;
        
        wax.Icon.sprite = ResourceGlobalStorage.GetResource(ResourceID.Bees_Wax).Icon;
        
        housing.Icon.sprite = ResourceGlobalStorage.GetResource(ResourceID.Housing).Icon;

        honey.Icon.sprite = ResourceGlobalStorage.GetResource(ResourceID.Honey).Icon;
    }

    private void Awake()
    {
        ResourceGlobalStorage.ResourceChanged += UpdateResourceInformation;
    }
    
    private void UpdateResourceInformation()
    {
        pollen.value.text = ResourceGlobalStorage.GetResource(ResourceID.Pollen).CurrentValue.ToString() + "/" + ResourceGlobalStorage.GetResource(ResourceID.Pollen).Capacity.ToString();
        wax.value.text = ResourceGlobalStorage.GetResource(ResourceID.Bees_Wax).CurrentValue.ToString() + "/" + ResourceGlobalStorage.GetResource(ResourceID.Bees_Wax).Capacity.ToString();
        housing.value.text = ResourceGlobalStorage.GetResource(ResourceID.Housing).CurrentValue.ToString() + "/" + ResourceGlobalStorage.GetResource(ResourceID.Housing).Capacity.ToString();
        honey.value.text = ResourceGlobalStorage.GetResource(ResourceID.Honey).CurrentValue.ToString() + "/" + ResourceGlobalStorage.GetResource(ResourceID.Honey).Capacity.ToString();
    }
}
