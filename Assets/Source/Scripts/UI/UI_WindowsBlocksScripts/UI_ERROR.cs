using System.Collections;
using UnityEngine;
using TMPro;

public class UI_ERROR : UIScreen
{
    [SerializeField] private GameObject error;

    public void _ErrorCall(string errorText)
    {
        if(errorText != null)
            StartCoroutine(ErrorInvis(errorText));
    }
    
    IEnumerator ErrorInvis(string errorText)
    {
        GameObject newError = Instantiate(error, Input.mousePosition, Quaternion.Euler(0,0,0), transform);
        
        TextMeshProUGUI text = newError.GetComponentInChildren<TextMeshProUGUI>();
        text.text = errorText;
        
        for (float alpha = 0f; alpha <= 2; alpha += 0.1f)
            yield return new WaitForSeconds(.1f);
        
        Destroy(newError);
    }
}
