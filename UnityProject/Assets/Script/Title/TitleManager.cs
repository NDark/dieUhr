using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
	public GameObject m_TitleRateMeConfirmMenu = null ;

	public void HideRateMeMenu()
	{
		m_TitleRateMeConfirmMenu.SetActive(false);
	}

	public void ConfirmRateMe()
	{
		m_TitleRateMeConfirmMenu.SetActive(false); 
		PlayerPrefs.SetInt("PlayerPrefs_RateMe", 1);
	}

	void Awake()
	{ 
		++ s_AwakeCount;
	}

	// Start is called before the first frame update
	void Start()
    {
        CheckShowRateMe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static int s_AwakeCount = 0 ;
	void CheckShowRateMe()
	{ 
		if( s_AwakeCount>=2)
		{
			var hasRateMe = PlayerPrefs.GetInt("PlayerPrefs_RateMe", 0);
			if (0 == hasRateMe)
			{
				m_TitleRateMeConfirmMenu.SetActive(true);
			}
		}
		
	}
}
