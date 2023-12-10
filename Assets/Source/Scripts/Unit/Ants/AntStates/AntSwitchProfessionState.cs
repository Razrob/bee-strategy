namespace Unit.Ants.States
{
    public class AntSwitchProfessionState : EntityStateBase
    {
        public override EntityStateID EntityStateID => EntityStateID.SwitchProfession;

        private readonly AntBase _ant;
        //TODO: Create construction for switching professions, and remove this
        private readonly AntProfessionsConfigsRepository _antProfessionsConfigsRepository;
        
        public AntSwitchProfessionState(AntBase ant)
        {
            _ant = ant;
            _antProfessionsConfigsRepository =
                AntProfessionsConfigsRepositoryDemonstration.Instance.AntProfessionsConfigsRepository;
        }

        public override void OnStateEnter()
        {
            //TODO: Create construction for switching professions, and remove this
            _ant.SwitchProfession(_antProfessionsConfigsRepository.Configs[_ant.TargetProfessionType][_ant.TargetProfessionRang]);
        }

        public override void OnStateExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}