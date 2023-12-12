using UnityEngine;

public class UI_Tactics : UIScreen
{
    [SerializeField] private ScreenButton[] _screenButtons;
    [SerializeField] private UnitSelection _unitSelection;

    private void Awake()
    {
        // _screenButtons[0].ScreenButtonClicked += OnTactic1Clicked;
        // _screenButtons[1].ScreenButtonClicked += OnTactic2Clicked;
        // _screenButtons[2].ScreenButtonClicked += OnTactic3Clicked;
    }

    private void OnTactic1Clicked()
    {
        ///
       /* foreach (var unit in _unitSelection.Pool.movingUnits)
            unit.SetStateBehaviorUnit(StateBehaviorUnitID.Attack);*/

        Debug.Log("Set attack state");
    }

    private void OnTactic2Clicked()
    {
/*        foreach (var unit in _unitSelection.Pool.movingUnits)
            unit.SetStateBehaviorUnit(StateBehaviorUnitID.Deffense);*/
        Debug.Log("Set deffense state");
    }

    private void OnTactic3Clicked()
    {
/*        foreach (var unit in _unitSelection.Pool.movingUnits)
            unit.SetStateBehaviorUnit(StateBehaviorUnitID.Neutral);*/
        Debug.Log("Set neutral state");
    }

}
