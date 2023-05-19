using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConstructionUX : MonoBehaviour
{
    [SerializeField] private ConstructionBase Construct;
    [SerializeField] private GameObject abilityBlock;
    [SerializeField] private Transform firstIconTransform;
    [SerializeField] private float distanceBetweenIcons;

    [SerializeField] private Slider healthPointsSlider;
    private List<AbilityBase> _abilities;

    void Start()
    {
        Vector3 firstIconPosition = firstIconTransform.position;

        healthPointsSlider.maxValue = Construct.MaxHealPoints;
        healthPointsSlider.value = Construct.CurrentHealPoints;

        _abilities = Construct.Abilites;
        for (int n = 0; n < Construct.Abilites.Count; n++)
        {
            GameObject newIconFill = Instantiate(abilityBlock, new Vector3(firstIconPosition.x + distanceBetweenIcons * n, firstIconPosition.y, firstIconPosition.z), firstIconTransform.rotation, firstIconTransform);
            newIconFill.GetComponent<AbilityBlock>().SetIcon(_abilities[n].AbilityIcon, _abilities[n]);
        }
    }

    void Update()
    {
        healthPointsSlider.value = Construct.CurrentHealPoints;
    }
}