using UnityEngine;

namespace Unit.Ants
{
    public class AntProfessionsConfigsRepositoryDemonstration : MonoBehaviour
    {
        //TODO: remove this script and create construction for switch professions (also look SwitchAntProfessionDemonstration.cs)
        public static AntProfessionsConfigsRepositoryDemonstration Instance;

        [field: SerializeField] 
        public AntProfessionsConfigsRepository AntProfessionsConfigsRepository { get; private set; }
        
        private void Awake()
        {
            if (!Instance.IsNullOrUnityNull())
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
    }
}