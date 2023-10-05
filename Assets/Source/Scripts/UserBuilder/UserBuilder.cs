using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using UnityEngine.EventSystems;
using System.Linq;

[Serializable]
struct BuildingDictionaryData
{
    public ConstructionID constructionID;
    [Tooltip("Movable prefab(/Prefabs/Constructions/MovableModel), NOT MAIN PREFAB")]
    public GameObject constructionModel;
}
public class UserBuilder : CycleInitializerBase
{
    [Inject] private readonly IConstructionFactory _constructionFactory;
    [SerializeField] LayerMask layerMask;
    
    [SerializeField] private bool useCheckMouseOverUI;

    [SerializeField] private List<BuildingDictionaryData> buildingsDictionaryData;
    [SerializeField] private Dictionary<ConstructionID, GameObject> _movableBuildingsWithID;
    [SerializeField] private ConstructionBase[] _buildings;
    private Dictionary<ConstructionID, ConstructionBase> _buildingsWithID;

    public Dictionary<ConstructionID, ConstructionBase> BuildingsWithID => _buildingsWithID;

    GameObject currentBuilding;
    ConstructionID currentConstructionID;
    
    bool spawnBuilding = false;
    private float _numberTownHall = 0;
    private UnitPool pool;
    private GameObject currentWorker;
    protected override void OnInit()
    {
        _movableBuildingsWithID = new Dictionary<ConstructionID, GameObject>();

        foreach (var element in _buildings)
            element.CalculateCost();

        _buildingsWithID = _buildings.ToDictionary(x => x.ConstructionID, x => x);



        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        pool = controller.GetComponent<UnitPool>();

        for (int n = 0; n < buildingsDictionaryData.Count; n++)
            _movableBuildingsWithID.Add(buildingsDictionaryData[n].constructionID, buildingsDictionaryData[n].constructionModel);
    }

    protected override void OnUpdate()
    {
        if (spawnBuilding)
        {
            _MoveBuilding(currentBuilding);
        }
        else
        {
            _Main();
        }
    }

    private void _Main()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if(FrameworkCommander.GlobalData.ConstructionSelector.TrySelect(ray))
            {
                ConstructionBase selectedConstruction = FrameworkCommander.GlobalData.ConstructionSelector.SelectedConstruction;
                selectedConstruction.Select();
                UI_Controller._SetBuilding(selectedConstruction);
            }
            else if (!MouseOverUI())
            {
                UI_Controller._SetWindow("UI_GameplayMain");
            }
        }
    }

    private bool MouseOverUI()//проверка что курсор игрока не наведен на UI/UX
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void _MoveBuilding(GameObject _currentBuilding)//перемещение здания по карте
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if ((!MouseOverUI() || !useCheckMouseOverUI) && Physics.Raycast(ray, out hit, 100F, CustomLayerID.Construction_Ground.Cast<int>(), QueryTriggerInteraction.Ignore)) //если рэйкаст сталкиваеться с чем нибудь, задаем зданию позицию точки столкновения рэйкаста
        {
            _currentBuilding.transform.position = FrameworkCommander.GlobalData.ConstructionsRepository.RoundPositionToGrid(ray.GetPoint(hit.distance));

            if (Input.GetButtonDown("Fire1"))//подтверждение строительства здания
            {
                if (hit.collider.name == "TileBase")
                {
                    if (!hit.collider.GetComponent<Tile>().Visible)
                    {
                        Destroy(_currentBuilding);
                        spawnBuilding = false;
                        return;
                    }
                }
                
                foreach (MovingUnit unit in pool.movingUnits)
                {
                    if (unit.IsSelected && unit.gameObject.CompareTag("Worker") && CanBuyConstruction(currentConstructionID))
                    {
                        BuyConstruction(currentConstructionID);

                        unit.SetDestination(hit.point);
                        unit.gameObject.transform.GetChild(4).GetComponent<WorkerDuty>().isFindingBuild = true;
                   
                        Spawn(unit, currentConstructionID);

                        Destroy(_currentBuilding);
                        spawnBuilding = false;
                        break;
                    }
                }
            }
            else if (Input.GetButtonDown("Fire2"))//отмена начала строительства
            {
                Destroy(_currentBuilding);
                spawnBuilding = false;
            }
        }
    }


    private bool CanBuyConstruction(ConstructionID id )
    {
        bool flagCanBuy = true;

        foreach (var element in _buildingsWithID[id].Cost.ResourceCost)
             if (element.Value > ResourceGlobalStorage.GetResource(element.Key).CurrentValue)
                 flagCanBuy = false;

        return flagCanBuy;
    }

    private void BuyConstruction(ConstructionID id)
    {
        foreach (var element in _buildingsWithID[id].Cost.ResourceCost)
            ResourceGlobalStorage.GetResource(element.Key).SetValue(ResourceGlobalStorage.GetResource(element.Key).CurrentValue - element.Value);
    }


    private void Spawn(MovingUnit unit, ConstructionID id)
    {
        if (id!= ConstructionID.Town_Hall || (id == ConstructionID.Town_Hall && _numberTownHall < 1))
        {
            if (id == ConstructionID.Town_Hall)
                _numberTownHall++;

            RaycastHit[] raycastHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            int index = raycastHits.IndexOf(hit => !hit.collider.isTrigger);

            if (index > -1)
            {
                Vector3 position = FrameworkCommander.GlobalData.ConstructionsRepository.RoundPositionToGrid(raycastHits[index].point);

                if (FrameworkCommander.GlobalData.ConstructionsRepository.ConstructionExist(position, false))
                {
                    Debug.Log("Invalid place");
                    return;
                }
                BuildingProgressConstruction progressConstruction = _constructionFactory.Create<BuildingProgressConstruction>(ConstructionID.Building_Progress_Construction);
                progressConstruction.transform.position = position;
                FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(position, progressConstruction);
                
                progressConstruction.OnTimerEnd += c => CreateConstruction(c, position);

                progressConstruction.StartBuilding(4, id, unit);
            }
        }
    }

    private void CreateConstruction(BuildingProgressConstruction buildingProgressConstruction, Vector3 position)
    {
        ConstructionBase construction = _constructionFactory.Create<ConstructionBase>(buildingProgressConstruction.BuildingConstructionID);
        
        FrameworkCommander.GlobalData.ConstructionsRepository.GetConstruction(position, true);

        Destroy(buildingProgressConstruction.gameObject);

        FrameworkCommander.GlobalData.ConstructionsRepository.AddConstruction(position, construction);
        construction.transform.position = position;
    }

    public void SpawnMovableBuilding(ConstructionID constructionID)
    {
        if (currentBuilding != null)
        {
            Destroy(currentBuilding.gameObject);
        }

        currentConstructionID = constructionID;
        spawnBuilding = true;
        currentBuilding = Instantiate(_movableBuildingsWithID[constructionID]);
    }
}