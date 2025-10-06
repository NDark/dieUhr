using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
	public GameObject m_TitleRateMeConfirmMenu = null ;
	public OnClickOpenBrower m_OpenBrower = null;

	public void HideRateMeMenu()
	{
		m_TitleRateMeConfirmMenu.SetActive(false);
	}

	public void ConfirmRateMe()
	{
		m_TitleRateMeConfirmMenu.SetActive(false); 
		PlayerPrefs.SetInt("PlayerPrefs_RateMe", 1);
		this.RedirectStore();
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


	void RedirectStore()
	{

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			m_OpenBrower.m_Url = "https://apps.apple.com/app/id1063018346";
		}
		else // if (Application.platform == RuntimePlatform.Android)
		{
			m_OpenBrower.m_Url = "https://play.google.com/store/apps/details?id=org.ndark.dieuhr";
		}
		
		m_OpenBrower.OpenBrower();
	}
}
