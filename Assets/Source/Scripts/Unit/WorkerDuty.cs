using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerDuty : MonoBehaviour
{
    public bool isGathering, isFindingRes, gotResource, isFindingBuild, isBuilding;
    private UI_GameplayMain UI_Counter;
    public Vector3 destination;
    public int loadCapacity;
    [SerializeField] private GameObject ResourceSkin;
    public GameObject WorkingOnGO;

    public float GatherTimer;

    void Start()
    {
        isGathering = false;
        UI_Counter = GameObject.Find("UI_GameplayMain").GetComponent<UI_GameplayMain>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {

        switch (collider.gameObject.tag)
        {

            case "PollenSource":
                {

                }
                break;

            case "BuildMark":
                {
                    isBuilding = true;
                    isFindingBuild = false;
                    collider.gameObject.GetComponent<BuildingProgressConstruction>().WorkerArrived = true;
                }
                break;

            default:
                {
                    if (gotResource && collider.gameObject.name == "TownHall")
                    {
                        gotResource = false;
                        ResourceSkin.SetActive(false);
                        isFindingRes = true;
                        ResourceGlobalStorage.ChangeValue(ResourceID.Pollen, loadCapacity);

                        this.gameObject.transform.parent.GetComponent<MovingUnit>().SetDestination(destination);
                    }
                }
                break;
        }

    }

    void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject == WorkingOnGO)
        {
            GatherTimer = 0;
        }


        if (collider.gameObject.tag == "BuildMark")
        {
            collider.gameObject.GetComponent<BuildingProgressConstruction>().WorkerArrived = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject == WorkingOnGO && !collider.GetComponent<PollenStorage>().isPollinated)
        {
            if (isFindingRes && !isGathering)
            {
                isGathering = true;
            }

            if (isGathering)
            {
                GatherTimer += Time.deltaTime;
            }

            if (GatherTimer > 3 && isGathering)
            {
                gotResource = true;
                ResourceSkin.SetActive(true);
                isFindingRes = false;
                isGathering = false;

                collider.GetComponent<PollenStorage>().ExtractResource(loadCapacity);

                
                GameObject Base = GameObject.Find("TownHall");
                destination = Base.transform.position;
                this.gameObject.transform.parent.GetComponent<MovingUnit>().SetDestination(destination);
                destination = WorkingOnGO.transform.position;
            }
        }
    }

}
