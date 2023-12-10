using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Unit.Ants
{
    public abstract class AntProfessionConfigBase : ScriptableObject
    {
        public abstract AntProfessionType ProfessionType { get; }

        [field: SerializeField] public AntProfessionRang AntProfessionRang { get; private set; }
        [field: SerializeField] public AnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public float InteractionRange { get; private set; }
        [field: SerializeField] private List<AntType> antsAccess { get; set; }
        
        public IReadOnlyList<AntType> AntsAccess => antsAccess;
    }
}