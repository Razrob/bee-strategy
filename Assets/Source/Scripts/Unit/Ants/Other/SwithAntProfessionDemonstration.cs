using Unit.Ants;
using UnityEngine;
using UnityEngine.Serialization;

public class SwithAntProfessionDemonstration : MonoBehaviour
{
    [FormerlySerializedAs("antBase")] [SerializeField] private AntBase ant;
    [SerializeField] private AntWorkerConfig workerConfig;
    [SerializeField] private AntMeleeWarriorConfig meleeConfig;
    [SerializeField] private AntRangeWarriorConfig rangeConfig;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) ant.SwitchProfession(workerConfig);
        if(Input.GetKeyDown(KeyCode.S)) ant.SwitchProfession(meleeConfig);
        if(Input.GetKeyDown(KeyCode.D)) ant.SwitchProfession(rangeConfig);
    }
}
