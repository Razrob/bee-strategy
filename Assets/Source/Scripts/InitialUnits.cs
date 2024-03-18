using System;
using Unit.Factory;
using UnityEngine;
using Zenject;

public class InitialUnits : MonoBehaviour
{
    [SerializeField] private InitUnitPair[] initUnits;

    [Inject] private readonly UnitFactory _unitFactory;
        
    private void Start()
    {
        foreach (var initUnit in initUnits)
        {
            var unit = _unitFactory.Create(initUnit.UnitType);
            unit.Transform.position = initUnit.Transform.position;
        }
    }
        
    [Serializable]
    private struct InitUnitPair
    {
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }
    }
}