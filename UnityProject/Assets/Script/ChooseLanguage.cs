using UnityEngine;
using System.Collections;

public class ChooseLanguage : MonoBehaviour {

    public void ChooseDeutsch()
    {
        Localization.language = "Deutsch";
    }
    public void ChooseEnglish()
    {
        Localization.language = "English";
    }
    public void ChooseTraditionChinese()
    {
        Localization.language = "TraditionalChinese";
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
