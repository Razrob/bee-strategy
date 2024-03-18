using UnityEngine;
using Zenject;

namespace Poison
{
    public class PoisonFogFactory : MonoBehaviour
    {
        [Inject] private PoisonFogFactoryConfig _poisonFogFactoryConfig;

        private Pool<PoisonFog> _pool;

        private void Awake()
        {
            _pool = new Pool<PoisonFog>(InstantiatePoisonFog);
        }

        public PoisonFog Create()
            => Create(Vector3.zero);

        public PoisonFog Create(Vector3 position)
        {
            var fog = _pool.ExtractElement();
            fog.transform.position = position;
            FrameworkCommander.GlobalData.PoisonFogsRepository.Add(fog);
            
            return fog;
        }

        private PoisonFog InstantiatePoisonFog()
            => Instantiate(_poisonFogFactoryConfig.PoisonFogPrefab, transform);
    }
}