using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class ConstructionTester : CycleInitializerBase
{
    [Inject] private readonly IConstructionFactory _constructionFactory;

    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            int index = raycastHits.IndexOf(hit => !hit.collider.isTrigger);

            if (index > -1)
            {
                Vector3 position = FrameworkCommander.GlobalData.ConstructionsRepository.RoundPositionToGrid(raycastHits[index].point);

                if (FrameworkCommander.GlobalData.ConstructionsRepository.ConstructionExist(position.ToInt(), false))
                    return;

                BuildingProgressConstruction progressConstruction = 
                    _constructionFactory.Create<BuildingProgressConstruction>(ConstructionID.BuildingProgressConstruction);
                progressConstruction.transform.position = position;
                FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(position.ToInt(), progressConstruction);

                progressConstruction.OnTimerEnd += c => CreateDefaultConstruction(c, position.ToInt());
                //progressConstruction.StartBuilding(4, ConstructionID.Test_Construction);
            }
        }
    }

    private void CreateDefaultConstruction(BuildingProgressConstruction buildingProgressConstruction, Vector3Int position)
    {
        DefaultConstruction defaultConstruction =
            _constructionFactory.Create<DefaultConstruction>(buildingProgressConstruction.BuildingConstructionID);

        FrameworkCommander.GlobalData.ConstructionsRepository
            .GetConstruction(position, true);

        Destroy(buildingProgressConstruction.gameObject);

        FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(position, defaultConstruction);
        defaultConstruction.transform.position = position;
    }
}
