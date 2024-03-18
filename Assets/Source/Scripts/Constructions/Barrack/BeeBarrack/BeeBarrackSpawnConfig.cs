using UnityEngine;

namespace Constructions
{
    [CreateAssetMenu(fileName = nameof(BeeBarrackSpawnConfig), menuName = "Configs/Constructions/SpawnConfigs/" + nameof(BeeBarrackSpawnConfig))]
    public class BeeBarrackSpawnConfig : ScriptableObject, ISingleConfig
    {
        [SerializeField] private ConstructionSpawnConfiguration<BeeBarrack> _configuration;

        public ConstructionSpawnConfiguration<BeeBarrack> GetConfiguration()
        {
            return _configuration;
        }
    }
}