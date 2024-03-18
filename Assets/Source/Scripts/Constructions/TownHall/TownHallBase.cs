using Constructions.LevelSystemCore;
using Unit.Factory;
using UnitsRecruitingSystemCore;
using UnityEngine;
using Zenject;

namespace Construction.TownHalls
{
    public abstract class TownHallBase : ConstructionBase, IEvolveConstruction
    {
        [SerializeField] protected Transform workerBeesSpawnPosition;
       
        [Inject] protected readonly UnitFactory _unitFactory;
        
        public abstract IConstructionLevelSystem LevelSystem { get; protected set; }
        public IReadOnlyUnitsRecruiter Recruiter => _recruiter;

        protected UnitsRecruiter _recruiter;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            _updateEvent += OnUpdate;
        }

        private void OnUpdate()
        {
            _recruiter.Tick(Time.deltaTime);
        }
        
        public void RecruitUnit(UnitType unitID)
        {
            int freeStackIndex = _recruiter.FindFreeStack();

            if (freeStackIndex == -1)
            {
                UI_Controller._ErrorCall("All stacks are busy");
                return;
            }

            if (!_recruiter.CheckCosts(unitID))
            {
                UI_Controller._ErrorCall("Need more resources");
                return;
            }
        
            _recruiter.RecruitUnit(unitID, freeStackIndex);
        }
    }
}