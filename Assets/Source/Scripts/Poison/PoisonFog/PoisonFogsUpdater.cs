using System.Linq;
using UnityEngine;

namespace Poison
{
    public class PoisonFogsUpdater : CycleInitializerBase
    {
        protected override void OnUpdate()
        {
            var time = Time.deltaTime;
            var fogs = FrameworkCommander.GlobalData.PoisonFogsRepository.Fogs.ToList();
            foreach (var fog in fogs)
                fog.HandleUpdate(time);
        }
    }
}