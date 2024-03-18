using Poison;
using UnityEngine;

namespace Constructions
{
    public class ButterflyPoisonFlowerPoisonFogProcessor
    {
        private readonly PoisonFogFactory _poisonFogFactory;
        private readonly Transform _flowerTransform;
        private PoisonFog _staticPoisonFog;
        private float _fogExistTime;
        private float _fogRadius;

        public ButterflyPoisonFlowerPoisonFogProcessor(Transform flowerTransform, PoisonFogFactory poisonFogFactory)
        {
            _flowerTransform = flowerTransform;
            _poisonFogFactory = poisonFogFactory;
        }

        public void SetData(float fogExistTime, float fogRadius, float staticFogRadius)
        {
            _fogExistTime = fogExistTime;
            _fogRadius = fogRadius;

            if (staticFogRadius > 0)
            {
                if (_staticPoisonFog == null)
                    _staticPoisonFog = _poisonFogFactory.Create(_flowerTransform.position);
                _staticPoisonFog.SetData(staticFogRadius, float.PositiveInfinity);
            }
            else
            {
                if (_staticPoisonFog != null)
                {
                    _staticPoisonFog.RemoveFog();
                    _staticPoisonFog = null;
                }
            }
        }
        
        public void SpawnPoisonFog()
        {
            if (_staticPoisonFog == null)
                _staticPoisonFog = _poisonFogFactory.Create(_flowerTransform.position);
            _staticPoisonFog.SetData(_fogRadius, _fogExistTime);
        }
    }
}