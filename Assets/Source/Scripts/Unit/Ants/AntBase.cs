using System;
using System.Collections.Generic;
using System.Linq;
using Projectiles.Factory;
using Unit.Ants.Configs;
using Unit.Ants.Configs.Professions;
using Unit.Ants.Professions;
using Unit.Ants.States;
using Unit.ProfessionsCore;
using UnityEngine;
using Zenject;

namespace Unit.Ants
{
    public abstract class AntBase : AntUnit
    {
        [SerializeField] private AntUnitConfig config;
        [SerializeField] private GameObject resource;
        [SerializeField] private Animator animator;

        [Inject] private ProjectileFactory _projectileFactory;
        
        public ProfessionType CurProfessionType => CurrentProfession.ProfessionType;
        public int CurProfessionRang => _antProfessionRang.Rang;
        protected override ProfessionBase CurrentProfession => Profession;
        
        public ProfessionType TargetProfessionType { get; private set; }
        public ProfessionBase Profession { get; private set; }
        public int TargetProfessionRang { get; private set; }
        
        private AntProfessionRang _antProfessionRang;

        protected override void OnAwake()
        {
            base.OnAwake();
            
            _healthStorage = new ResourceStorage(config.HealthPoints, config.HealthPoints);
        }

        public override void OnElementExtract()
        {
            base.OnElementExtract();
            
            resource.SetActive(false);
            _healthStorage.SetValue(_healthStorage.Capacity);
            SetProfession(config.DefaultProfession);

            var stateBases = new List<EntityStateBase>()
            {
                new AntIdleState(this),
                new AntMoveState(this),
                new AntBuildState(this),
                new AntResourceExtractionState(this),
                new AntStorageResourceState(this),
                new AntAttackState(this),
                new AntSwitchProfessionState(this)
            };
            _stateMachine = new EntityStateMachine(stateBases, EntityStateID.Idle);
        }

        public void GiveOrderSwitchProfession(IUnitTarget unitTarget, AntProfessionConfigBase config) 
            => GiveOrderSwitchProfession(unitTarget, config.ProfessionType, config.AntProfessionRang);
        
        public void GiveOrderSwitchProfession(IUnitTarget unitTarget,
            ProfessionType newProfessionType, AntProfessionRang newProfessionRang)
        {
            if(newProfessionType == CurProfessionType && newProfessionRang.Rang == CurProfessionRang || 
               unitTarget.IsAnyNull())
            {
                Debug.LogWarning($"You try give order to set profession that already set ({newProfessionType == CurProfessionType}, {newProfessionRang.Rang == CurProfessionRang}) or Target is null ({unitTarget})");
                AutoGiveOrder(unitTarget);
                return;
            }

            TargetProfessionType = newProfessionType;
            TargetProfessionRang = newProfessionRang.Rang;
            
            HandleGiveOrder(unitTarget, UnitPathType.Switch_Profession);
        }

        public void SwitchProfession(AntProfessionConfigBase newProfession)
        {
            TrySetProfession(newProfession);
            AutoGiveOrder(null);
        }
        
        /// <summary>
        /// Set new profession with checks, for set without any check use SetProfession(...)
        /// </summary>
        private void TrySetProfession(AntProfessionConfigBase newProfession)
        {
            if (!newProfession.Access.Contains(UnitType))
            {
                Debug.LogWarning($"You try set profession ({newProfession}) that un accessed to this unit ({UnitType})");
                return; 
            }
            if(newProfession.ProfessionType == CurProfessionType && newProfession.ProfessionRang == CurProfessionRang)
            {
                Debug.LogWarning($"You try set profession that already set ({CurProfessionType}:{CurProfessionRang})({newProfession})");
                return;
            }

            SetProfession(newProfession);
        }
        
        /// <summary>
        /// Set new profession without any checks, for set with check use TrySetProfession(...)
        /// </summary>
        private void SetProfession(AntProfessionConfigBase newProfession)
        {
            switch (newProfession.ProfessionType)
            {
                case (ProfessionType.Worker):
                    var resourceRepository = ResourceGlobalStorage.ResourceRepository;
                    var workerLogic = new AntWorkerProfession(this, newProfession as AntWorkerConfig,
                        resourceRepository, resource);
                    Profession = workerLogic;
                    break;
                case (ProfessionType.MeleeWarrior):
                    Profession = new AntMeleeWarriorProfession(this, newProfession as AntMeleeWarriorConfig);
                    break;
                case (ProfessionType.RangeWarrior):
                    Profession  = new AntRangeWarriorProfession(this, newProfession as AntRangeWarriorConfig, _projectileFactory);
                    break;
                default: throw new NotImplementedException();
            }

            /*need because worker can have resource in the hands,
             so it is need for hide resource skin with profession changing*/
            resource.SetActive(false);
            _antProfessionRang = newProfession.AntProfessionRang;
            animator.runtimeAnimatorController = newProfession.AnimatorController;
        }
    }
}