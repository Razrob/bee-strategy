using System.Collections;
using UnityEngine;

public class PollenStorage : ResourceSourceBase
{
    public override ResourceID ResourceID => ResourceID.Pollen;
    protected ResourceStorage Pollen = new ResourceStorage(100, 100);
    public float MaxPollen => Pollen.Capacity;
    public float CurrentPollen => Pollen.CurrentValue;
    public int timer;
    public bool isPollinated = false;
    public GameObject Model;
    public GameObject pollinatedModel;

    public override void ExtractResource(int extracted)
    {
        Pollen.ChangeValue(-extracted);

        if (CurrentPollen <= 0)
        {
            isPollinated = true;
            pollinatedModel.SetActive(true);
            Model.SetActive(false);
            StartCoroutine(StartRePollinating(timer));
        }
    }
    
    private IEnumerator StartRePollinating(int duration)
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(1f);
            duration--;
        }

        isPollinated = false;
        pollinatedModel.SetActive(false);
        Model.SetActive(true);
    }
}
