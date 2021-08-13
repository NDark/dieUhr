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

	public void ChoosePolish()
    {
        Localization.language = "Polish";
    }

}
