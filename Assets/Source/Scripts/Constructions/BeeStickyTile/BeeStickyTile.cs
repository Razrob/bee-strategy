﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Constructions
{
    public class BeeStickyTile : ConstructionBase
    {
        [SerializeField] private SerializableDictionary<ResourceID, int> _costValues;
        [SerializeField] private TriggerBehaviour _triggerBehaviour;
        public override ConstructionID ConstructionID => ConstructionID.Bee_Sticky_Tile_Construction;

        protected override void OnAwake()
        {
            _healthStorage = new ResourceStorage(100, 100);
        }

        protected override void OnStart()
        {
            _triggerBehaviour.EnterEvent += OnUnitEnter;
            _triggerBehaviour.ExitEvent += OnUnitExit;
        }

        private void OnUnitEnter(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out MovingUnit movingUnit))
            {
                movingUnit.ChangeContainsStickyTiles(1);
            }
        }

        private void OnUnitExit(ITriggerable triggerable)
        {
            if (triggerable.TryCast(out MovingUnit movingUnit))
            {
                movingUnit.ChangeContainsStickyTiles(-1);
            }
        }
    }
}