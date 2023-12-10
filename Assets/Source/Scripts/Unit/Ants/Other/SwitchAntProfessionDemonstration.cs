using Unit.Ants;
using UnityEngine;

public class SwitchAntProfessionDemonstration : MonoBehaviour, IUnitTarget
{
    //TODO: remove this script and create construction for switch professions (also look AntProfessionsConfigsRepositoryDemonstration.cs)
    [SerializeField] private AntBase ant;
    [SerializeField] private AntProfessionRang professionRang;
    
    public Transform Transform => transform;
    public UnitTargetType TargetType => UnitTargetType.Construction;
    public AffiliationEnum Affiliation => AffiliationEnum.Ants;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SwitchProfession(AntProfessionType.Worker);
        if(Input.GetKeyDown(KeyCode.S)) 
            SwitchProfession(AntProfessionType.MeleeWarrior);
        if(Input.GetKeyDown(KeyCode.D)) 
            SwitchProfession(AntProfessionType.RangeWarrior);
    }

    private void SwitchProfession(AntProfessionType newProfessionType)
    {
        ant.GiveOrderSwitchProfession(this, ant.transform.position, newProfessionType, professionRang);
    }
}
