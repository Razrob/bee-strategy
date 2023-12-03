using UnityEditor.Animations;
using UnityEngine;

namespace Unit.Ants
{
    public abstract class AntProfessionConfigBase : ScriptableObject
    {
        public abstract AntProfessionType Profession { get; }

        [field: SerializeField] public AnimatorController animatorController { get; private set; }
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public float Range { get; private set; }

        protected AntProfessionConfigBase(float range)
        {
            Range = range;
        }
    }
}