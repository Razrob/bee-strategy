using System.Collections.Generic;
using Unit.Bees.Configs;
using Unit.ProfessionsCore;
using Unit.States;
using UnityEngine;

namespace Unit.Bees
{
    public class WorkerBee : BeeUnit
    {
        [SerializeField] private BeeWorkerConfig beeWorkerConfig;
        [SerializeField] private GameObject resourceSkin;

        public override UnitType UnitType => UnitType.WorkerBee;
        protected override ProfessionBase CurrentProfession => _workerProfession;

        private WorkerProfession _workerProfession;

        protected override void OnAwake()
        {
            base.OnAwake();

            _healthStorage = new ResourceStorage(beeWorkerConfig.HealthPoints, beeWorkerConfig.HealthPoints);
        }

        public override void OnElementExtract()
        {
            base.OnElementExtract();
            
            _healthStorage.SetValue(_healthStorage.Capacity);
            var resourceRepository = ResourceGlobalStorage.ResourceRepository;
            _workerProfession = new WorkerProfession(this, beeWorkerConfig.InteractionRange,
                beeWorkerConfig.GatheringCapacity, beeWorkerConfig.GatheringTime, resourceRepository, resourceSkin);
            resourceSkin.SetActive(false);

            var stateBases = new List<EntityStateBase>()
            {
                new IdleState(),
                new MoveState(this, _workerProfession),
                new BuildState(this),
                new ResourceExtractionState(this, _workerProfession),
                new StorageResourceState(this, _workerProfession),
            };
            _stateMachine = new EntityStateMachine(stateBases, EntityStateID.Idle);
        }
    }
}
    