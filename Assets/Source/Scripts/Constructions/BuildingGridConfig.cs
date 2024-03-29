﻿using UnityEngine;

[CreateAssetMenu(fileName = "BuildingGridConfig", menuName = "Config/BuildingGridConfig")]
public class BuildingGridConfig : ScriptableObject, ISingleConfig
{
    [SerializeField] private Vector2 _hexagonsOffcets;

    public Vector2 HexagonsOffcets => _hexagonsOffcets;
}

