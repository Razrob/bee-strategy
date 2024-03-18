using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    private static UserBuilder builder;
    private static GameObject UI_ActivScreen;
    private static UI_Gameplay UI_GameplayWindows;
              
    private static GameObject UI_PrevActivScreen;
    private static ConstructionBase selectedConstruction;

    private static UI_ERROR _uiError;

    private void Start()
    {
        builder = GameObject.Find("Builder").GetComponent<UserBuilder>();
        if(builder == null)
            Debug.LogError("Builder is null");
        
        UI_GameplayWindows = UIScreenRepository.GetScreen<UI_Gameplay>().gameObject.GetComponent<UI_Gameplay>();

        //определяем, какое окно у нас активно при запуске.
        if (UIScreenRepository.GetScreen<UI_Gameplay>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Gameplay>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_Buildings>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Buildings>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_Tactics>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Tactics>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_GameplayMenu>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_GameplayMenu>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_Settings>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Settings>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_Win>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Win>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_Lose>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Lose>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_MainMenu>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_MainMenu>().gameObject;
        else
        if (UIScreenRepository.GetScreen<UI_Saves>().isActiveAndEnabled)
            UI_ActivScreen = UIScreenRepository.GetScreen<UI_Saves>().gameObject;

        UI_PrevActivScreen = UI_ActivScreen;
        _uiError =  UIScreenRepository.GetScreen<UI_ERROR>();
    }

    #region Spawn of buildings
    public static void _SpawnTownHall()
    {
        builder.SpawnConstructionMovableModel(ConstructionID.BeeTownHall);
    }
    
    public static void _SpawnBarrack()
    {
        builder.SpawnConstructionMovableModel(ConstructionID.BeeBarrack);
    }
    
    public static void _SpawnBeeHouse()
    {
        builder.SpawnConstructionMovableModel(ConstructionID.BeeHouse);
    }
    
    public static void _SpawnWaxFactory()
    {
        builder.SpawnConstructionMovableModel(ConstructionID.BeeWaxProduceConstruction);
    }

    public static void _SpawnStickyTile()
    {
        builder.SpawnConstructionMovableModel(ConstructionID.BeeStickyTileConstruction);
    }

    #endregion

    public static void _ChoiceTactic()
    { Debug.Log("Error: tactics is empty"); }

    public static void _ChoiceGroup()
    { Debug.Log("Error: groups is empty"); }

    public static void _SetWindow(string windowName)
    {
        GameObject screenBuffer = UI_ActivScreen;
        UI_ActivScreen.SetActive(false);

        switch (windowName)
        {
            case "UI_Gameplay":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_Gameplay>().gameObject; break;
            case "UI_GameplayMain":
                UI_GameplayWindows.SetGameplayWindow(windowName, null); break;
            case "UI_Buildings":
                UI_GameplayWindows.SetGameplayWindow(windowName, null); break;
            case "UI_Tactics":
                UI_GameplayWindows.SetGameplayWindow(windowName, null); break;
            case "UI_TownHallMenu":
                UI_GameplayWindows.SetGameplayWindow(windowName, selectedConstruction); break;
            case "UI_BarracksMenu":
                UI_GameplayWindows.SetGameplayWindow(windowName, selectedConstruction); break;
            case "UI_BeeHouseMenu":
                UI_GameplayWindows.SetGameplayWindow(windowName, selectedConstruction); break;
            case "UI_BeesWaxProduceConstructionMenu":
                UI_GameplayWindows.SetGameplayWindow(windowName, selectedConstruction); break;

            case "UI_GameplayMenu":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_GameplayMenu>().gameObject; break;
            case "UI_Settings":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_Settings>().gameObject; break;
            case "UI_Win":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_Win>().gameObject; break;
            case "UI_Lose":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_Lose>().gameObject; break;
            case "UI_MainMenu":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_MainMenu>().gameObject; break;
            case "UI_Saves":
                UI_ActivScreen = UIScreenRepository.GetScreen<UI_Saves>().gameObject; break;

            case "Back":
                UI_ActivScreen = UI_PrevActivScreen; break;
            default:
                Debug.Log("Error: invalid string parametr in _SetWindow(string windowName)"); break;
        }

        UI_PrevActivScreen = screenBuffer;
        UI_ActivScreen.SetActive(true);
    }

    public static void LoadScene(string sceneName)//загрузка сцены. принимает название сцены
    {
        if (sceneName == "empty")
        {
            Debug.Log("Error: scene name is not set");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void SetBuilding(ConstructionBase newConstruction)//установка текущего выделеного здания здания
    {
        string windowName;
        switch (newConstruction.ConstructionID)
        {
            case (ConstructionID.BeeTownHall):
            {
                windowName = "UI_TownHallMenu";
                break;
            }
            case (ConstructionID.BeeBarrack):
            {
                windowName = "UI_BarracksMenu";
                break;
            }
            case (ConstructionID.BeeHouse):
            {
                windowName = "UI_BeeHouseMenu";
                break;
            }
            case (ConstructionID.BeeWaxProduceConstruction):
            {
                windowName = "UI_BeesWaxProduceConstructionMenu";
                break;
            }
            default:
            {
                windowName = "UI_GameplayMain";
                break;
            }
        }
        
        selectedConstruction = newConstruction;
        _SetWindow(windowName);
    }

    public static void _ErrorCall(string error)
    {
        _uiError._ErrorCall(error);
    }
    
    public static void _Quite()
    {
        Application.Quit();
    }
}