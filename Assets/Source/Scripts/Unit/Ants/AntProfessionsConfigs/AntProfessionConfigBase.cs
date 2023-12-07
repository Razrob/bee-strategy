using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Unit.Ants
{
    public abstract class AntProfessionConfigBase : ScriptableObject
    {
        public abstract AntProfessionType Profession { get; }
        

        [field: SerializeField] public AnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] protected List<AntType> antsAccess { get; private set; }
        
        public IReadOnlyList<AntType> AntsAccess => antsAccess;
    }
}