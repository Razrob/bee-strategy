using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BuildingProgressConstruction : ConstructionBase
{
    [SerializeField] private TMP_Text _timerText;

    public override ConstructionID ConstructionID => ConstructionID.Building_Progress_Construction;
    public ConstructionID BuildingConstructionID { get; private set; }

    public BuildingProgressState BuildingProgressState { get; private set; } = BuildingProgressState.Waiting;

    public event Action<BuildingProgressConstruction> OnTimerEnd;

    private GameObject currentWorker;
    public UnitPool pool;
    public bool WorkerArrived;

    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        pool = controller.GetComponent<UnitPool>();
    }

    public void StartBuilding(int duration, ConstructionID constructionID, MovingUnit unit)
    {
        if (BuildingProgressState != BuildingProgressState.Waiting)
            return;
        BuildingProgressState = BuildingProgressState.Started;
        BuildingConstructionID = constructionID;
        StartCoroutine(StartBuildingCoroutine(duration, unit));
    }

    private IEnumerator StartBuildingCoroutine(int duration, MovingUnit currentWorker)
    {
        int timer = duration;

        while (timer > 0)
        {
            _timerText.text = $"{timer.SecsToMins()}";
            yield return new WaitForSeconds(1f);
            if (//currentWorker.gameObject.transform.GetComponentInChildren<WorkerDuty>().isBuilding && 
                WorkerArrived)
            {
                timer--;
            }
        }

        _timerText.text = $"{timer.SecsToMins()}";

        yield return new WaitForSeconds(1f);
		BuildingProgressState = BuildingProgressState.Completed;
        // currentWorker.gameObject.transform.GetComponentInChildren<WorkerDuty>().isBuilding = false;


        OnTimerEnd?.Invoke(this);
    }
}
