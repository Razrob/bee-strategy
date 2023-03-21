using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TownHallLevelSystem : BuildingLevelSystemBase <TownHallLevel>
{
    private BeesRecruiting _beesRecruiting;
    
    public TownHallLevelSystem(BuildingLevelSystemBase<TownHallLevel> buildingLevelSystemBase, ResourceStorage healPoints,
        BeesRecruiting beesRecruiting) : base(buildingLevelSystemBase, healPoints)
    {
        _beesRecruiting = beesRecruiting;
    }

    public override void NextLevel()
    {
        try
        {
            base.NextLevel();
        }
        catch (Exception e)
        {
            UI_Controller._ErrorCall(e.Message);
            return;
        }

        SpendResources();
        float prevPollenCapacity = CurrentLevel.PollenCapacity;
        float prevBeesWaxCapacity = CurrentLevel.BeesWaxCapacity;
        float prevHousingCapacity = CurrentLevel.HousingCapacity;
        float prevHoneyCapacity = CurrentLevel.HoneyCapacity;
        currentLevelNum++;
            
        ResourceGlobalStorage.ChangeCapacity(ResourceID.Pollen, CurrentLevel.PollenCapacity - prevPollenCapacity);
        ResourceGlobalStorage.ChangeCapacity(ResourceID.Bees_Wax, CurrentLevel.BeesWaxCapacity - prevBeesWaxCapacity);
        ResourceGlobalStorage.ChangeCapacity(ResourceID.Housing, CurrentLevel.HousingCapacity - prevHousingCapacity);
        ResourceGlobalStorage.ChangeCapacity(ResourceID.Honey, CurrentLevel.HoneyCapacity - prevHoneyCapacity);
            
        ResourceGlobalStorage.ChangeValue(ResourceID.Housing,CurrentLevel.HousingCapacity - prevHousingCapacity);
            
        _beesRecruiting.AddStacks(CurrentLevel.RecruitingSize);
        _beesRecruiting.SetNewBeesDatas(CurrentLevel.BeesRecruitingData);

        if (HealPoints.CurrentValue >= HealPoints.Capacity)
        {
            HealPoints.SetCapacity(CurrentLevel.MaxHealPoints);
            HealPoints.SetValue(CurrentLevel.MaxHealPoints);
        }
        else
        {
            HealPoints.SetCapacity(CurrentLevel.MaxHealPoints);
        }
            
        Debug.Log("Building LVL = " + CurrentLevelNum);
    }
}
