using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.Ants
{
    public class AntUnit : MovingUnit
    {
        public override AffiliationEnum Affiliation => AffiliationEnum.Ants;
        
        [SerializeField] protected AntProfessionConfigBase ProfessionConfig;

        public AntProfessionType ProfessionType => ProfessionConfig.Profession;
        public UnitType UnitType => ProfessionConfig.UnitType; 
    }
}