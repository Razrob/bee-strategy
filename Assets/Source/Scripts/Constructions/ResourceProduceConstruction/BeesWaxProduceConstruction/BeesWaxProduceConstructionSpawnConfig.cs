using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeesWaxProduceConstructionSpawnConfig),
        menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeesWaxProduceConstructionSpawnConfig))]
    public class BeesWaxProduceConstructionSpawnConfig : ScriptableObject, ISingleConfig
    {
        [SerializeField] private ConstructionSpawnConfiguration<BeesWaxProduceConstruction> _configuration;

        public ConstructionSpawnConfiguration<BeesWaxProduceConstruction> GetConfiguration()
        {
            return _configuration;
        }
    }
}