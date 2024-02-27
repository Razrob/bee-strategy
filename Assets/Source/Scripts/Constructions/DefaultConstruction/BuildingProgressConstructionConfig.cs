﻿using UnityEngine;

[CreateAssetMenu(fileName = "BuildingProgressConstructionConfig", menuName = "Config/BuildingProgressConstructionConfig")]
public class BuildingProgressConstructionConfig : ScriptableObject, ISingleConfig
{
    [SerializeField] private ConstructionSpawnConfiguration<BuildingProgressConstruction> _configuration;

    public ConstructionSpawnConfiguration<BuildingProgressConstruction> GetConfiguration()
    {
        return _configuration;
    }
}