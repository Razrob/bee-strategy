using Constructions.LevelSystemCore;
using Unit.Factory;
using UnitsRecruitingSystemCore;
using UnityEngine;
using Zenject;

namespace Constructions
{
    public abstract class BarrackBase : ConstructionBase, IEvolveConstruction
    {
        [SerializeField] protected Transform unitsSpawnPosition;

        [Inject] protected readonly UnitFactory unitFactory;
        
        public IReadOnlyUnitsRecruiter Recruiter => recruiter;

        public IConstructionLevelSystem LevelSystem { get; protected set; }

        protected UnitsRecruiter recruiter;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            _updateEvent += RecruiterTick;
        }

        private void RecruiterTick() 
            => recruiter.Tick(Time.deltaTime);

        public void RecruitUnit(UnitType unitType) 
            => recruiter.RecruitUnit(unitType);
        
        public void RecruitUnit(UnitType unitType, int stackIndex) 
            => recruiter.RecruitUnit(unitType, stackIndex);
    }
}