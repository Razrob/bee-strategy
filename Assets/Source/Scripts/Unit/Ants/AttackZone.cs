using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : TriggerZone
{
    [SerializeField] private UnitBase _parent;

    private List<UnitBase> _enemies;
    public IReadOnlyList<UnitBase> Enemies => _enemies;
    
    protected override Func<ITriggerable, bool> EnteredComponentIsSuitable => 
        comp => comp is UnitBase unitBase && unitBase.Affiliation != _parent.Affiliation;

    protected override void OnEnter(ITriggerable component)
    {
        _enemies.Add((UnitBase)component);
    }

    protected override void OnExit(ITriggerable component)
    {
        _enemies.Remove((UnitBase)component);
    }
}
