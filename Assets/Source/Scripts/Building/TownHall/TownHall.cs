using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TownHall : ConstructionBase
{
    public override ConstructionID ConstructionID => ConstructionID.Town_Hall;

    #region Main
    protected UI_Controller UI;//контроллер интерфеса
    public float MaxHealPoints => currentLevel.MaxHealPoints;
    public float HealPoints => healPoints;
    protected float healPoints = 0;
    #endregion

    #region Resources
    //максимальное кол-во ресурсов хранимое в ратуши согласно текщему лвл-у
    public float MaxTrees => trees.Capacity;
    public float MaxFlowers => flowers.Capacity;
    public float MaxPlants => plants.Capacity;
    public float MaxWax => wax.Capacity;

    //текущее кол-во ресурсов хранимое в ратуши
    public float CurrentTrees => trees.CurrentValue;
    public float CurrentFlowers => flowers.CurrentValue;
    public float CurrentPlants => plants.CurrentValue;
    public float CurrentWax => wax.CurrentValue;

    //хранилища ресурсов
    ResourceStorage trees;
    ResourceStorage flowers;
    ResourceStorage plants;
    ResourceStorage wax;
    #endregion

    #region level-ups
    [SerializeField] List<TownHallLevel> levels;//массив уровней здания
    TownHallLevel currentLevel;//текущий уровень
    int currentLevelNum = 1;
    //цена в кол-ве ресурсов для повышения до следующего лвл-а ратуши, согласно текущему лвл-у
    public float TreesPrice => currentLevel.TreesPrice;
    public float FlowersPrice => currentLevel.FlowersPrice;
    public float PlantsPrice => currentLevel.PlantsPrice;
    public float WaxPrice => currentLevel.WaxPrice;
    #endregion

    #region Workers Bees
    public bool AlarmOn => alarmOn;
    bool alarmOn = false;//тревога включена?
    public static UnityEvent WorkerBeeAlarmOn = new UnityEvent();//оповещение рабочих пчел о тревоге
    static Stack<GameObject> WorkerBeesInTownHall = new Stack<GameObject>();//массив пчел, которые спрятались в ратуши

    [SerializeField] GameObject beePrefab;//префаб рабочей пчелы
    [SerializeField] Transform workerBeesSpawnPosition;//координаты флага, на котором спавняться рабочие пчелы

    public int MaxWorkerBeesNumber => currentLevel.MaxWorkerBeesNumber;
    public int WorkerBeesNumber => workerBeesNumber;
    int workerBeesNumber = 0;
    public int MaxWorkerBeesQueue => currentLevel.MaxWorkerBeesQueue;
    public int WorkerBeesQueue => workerBeesQueue;
    int workerBeesQueue = 0;
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        gameObject.name = "TownHall";
        UI = GameObject.Find("UI").GetComponent<UI_Controller>();

        currentLevel = levels[0];

        trees = new ResourceStorage(0F, currentLevel.MaxTrees);
        flowers = new ResourceStorage(0F, currentLevel.MaxFlowers);
        plants = new ResourceStorage(0F, currentLevel.MaxPlants);
        wax = new ResourceStorage(0F, currentLevel.MaxWax);
    }


    #region Resource methods
    public void _AddTrees(float addTrees)
    {
        if (trees.CurrentValue + addTrees >= trees.Capacity)
        {
            trees.ChangeValue(addTrees);
            Debug.Log("too much trees");
        }
        else
            trees.ChangeValue(addTrees);
    }
    public void _AddFlowers(float addFlowers)
    {
        if (flowers.CurrentValue + addFlowers >= flowers.Capacity)
        {
            flowers.ChangeValue(addFlowers);
            Debug.Log("too much flowers");
        }
        else
            flowers.ChangeValue(addFlowers);
    }
    public void _AddPlants(float addPlants)
    {
        if (plants.CurrentValue + addPlants >= plants.Capacity)
        {
            plants.ChangeValue(addPlants);
            Debug.Log("too much plants");
        }
        else
            plants.ChangeValue(addPlants);
    }
    public void _AddWax(float addWax)
    {
        if (wax.CurrentValue + addWax >= wax.Capacity)
        {
            wax.ChangeValue(addWax);
            Debug.Log("too much wax");
        }
        else
            wax.ChangeValue(addWax);
    }
    #endregion

    #region  Woreker Bees Methods
    public static void _HideMe(GameObject workerBee)
    {
        WorkerBeesInTownHall.Push(workerBee);
        workerBee.SetActive(false);
    }
    public void _WorkerBeeAlarmer()
    {
        alarmOn = !alarmOn;
        if (alarmOn)
            WorkerBeeAlarmOn?.Invoke();
        else
            StartCoroutine("OutFromTownHall");
    }
    IEnumerator OutFromTownHall()
    {
        GameObject bee;
        while (WorkerBeesInTownHall.Count > 0 && !alarmOn)
        {
            bee = WorkerBeesInTownHall.Pop();
            bee.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }
    public void _SpawnWorkerBee()
    {
        if (workerBeesNumber < currentLevel.MaxWorkerBeesNumber && workerBeesQueue < currentLevel.MaxWorkerBeesQueue)
        {
            workerBeesQueue++;
            StartCoroutine("SpawnWorkerBeeCoroutine");
        }
        else
            Debug.Log("Error: you have max number of workers bees");
    }
    IEnumerator SpawnWorkerBeeCoroutine()
    {
        for (int n = 0; n < currentLevel.TimeWorkerBeeBuild; n++)
            yield return new WaitForSeconds(1f);

        workerBeesQueue--;
        workerBeesNumber++;

        GameObject newBee = Instantiate(beePrefab, workerBeesSpawnPosition.position, workerBeesSpawnPosition.rotation);
        if (alarmOn) _HideMe(newBee);
    }
    #endregion

    #region BuildingsMainMethods
    public void _CallBuildingMenu(string windowName)//вызов меню здания
    {
        UI._SetBuilding(gameObject, windowName);
    }
    public void _NextBuildingLevel()//повышение уровня здания, вызывется через UI/UX
    {
        if (trees.CurrentValue >= currentLevel.TreesPrice && flowers.CurrentValue >= currentLevel.FlowersPrice &&
            plants.CurrentValue >= currentLevel.PlantsPrice && wax.CurrentValue >= currentLevel.WaxPrice)
        {
            if (currentLevelNum == levels.Count)
            {
                Debug.Log("max Town Hall level");
                return;
            }
            currentLevel = levels[currentLevelNum++];

            trees.SetCapacity(currentLevel.MaxTrees);
            flowers.SetCapacity(currentLevel.MaxFlowers);
            plants.SetCapacity(currentLevel.MaxPlants);
            wax.SetCapacity(currentLevel.MaxWax);

            Debug.Log("Building LVL = " + currentLevelNum);
        }
        else
            Debug.Log("Need more resources");
    }
    public void _GetDamage(float damage)//получение урона
    {
        healPoints -= damage;
        if (healPoints <= 0)
        {
            Destroy(gameObject);
            UI._SetWindow("UI_Lose");
        }
    }
    public void _Repair(float addHP)
    {
        healPoints += addHP;
        if (healPoints > MaxHealPoints)
            healPoints = MaxHealPoints;
    }
    #endregion
}