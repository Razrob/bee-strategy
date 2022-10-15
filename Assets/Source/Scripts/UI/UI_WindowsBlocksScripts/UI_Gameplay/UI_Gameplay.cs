using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gameplay : UIScreen
{
    GameObject _UI_Buildings;
    GameObject _UI_Tactics;
    GameObject _UI_TownHallMenu;
    GameObject _UI_BarracksMenu;

    void Start()
    {
        _UI_Buildings = UIScreenRepository.GetScreen<UI_Buildings>().gameObject;
        _UI_Tactics = UIScreenRepository.GetScreen<UI_Tactics>().gameObject;
        _UI_TownHallMenu = UIScreenRepository.GetScreen<UI_TownHallMenu>().gameObject;
        _UI_BarracksMenu = UIScreenRepository.GetScreen<UI_BarracksMenu>().gameObject;
    }

    GameObject UI_Activ;//текущее активное окно. необходимо для работы _SetWindow()
    public void _SetGameplayWindow(string gemeplayWindowName)
    {
        switch (gemeplayWindowName)
        {
            case "UI_GameplayMain":
                {
                    _UI_Buildings.SetActive(false);
                    _UI_Tactics.SetActive(false);
                    _UI_TownHallMenu.SetActive(false);
                    _UI_BarracksMenu.SetActive(false);
                    break;
                }
            case "UI_Buildings":
                {
                    _UI_Buildings.SetActive(true);
                    _UI_Tactics.SetActive(false);
                    _UI_TownHallMenu.SetActive(false);
                    _UI_BarracksMenu.SetActive(false);
                    break;
                }
            case "UI_Tactics":
                {
                    _UI_Buildings.SetActive(false);
                    _UI_Tactics.SetActive(true);
                    _UI_TownHallMenu.SetActive(false);
                    _UI_BarracksMenu.SetActive(false);
                    break;
                }
            case "UI_TownHallMenu":
                {
                    _UI_Buildings.SetActive(false);
                    _UI_Tactics.SetActive(false);
                    _UI_TownHallMenu.SetActive(true);
                    _UI_BarracksMenu.SetActive(false);
                    break;
                }
            case "UI_BarracksMenu":
                {
                    _UI_Buildings.SetActive(false);
                    _UI_Tactics.SetActive(false);
                    _UI_TownHallMenu.SetActive(false);
                    _UI_BarracksMenu.SetActive(true);
                    break;
                }
            default:
                Debug.Log("Error: invalid string parametr in   _SetGameplayWindow(string gemeplayWindowName)"); break;
        }
    }
}