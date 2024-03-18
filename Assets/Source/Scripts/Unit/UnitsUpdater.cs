using System.Linq;
using UnityEngine;

namespace Unit
{
    public class UnitsUpdater : CycleInitializerBase
    {
        protected override void OnUpdate()
        {
            var time = Time.deltaTime;
            foreach (var unitsPair in FrameworkCommander.GlobalData.UnitRepository.Units)
            {
                var units = unitsPair.Value.ToList();
                foreach (var unit in units)
                    unit.HandleUpdate(time);
            }
        }
    }
}