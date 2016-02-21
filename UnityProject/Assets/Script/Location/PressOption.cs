using UnityEngine;
using System.Collections;

public class PressOption : MonoBehaviour 
{
	public LocationSystem m_LocationSystem = null ;
	public int m_OptionIndex = 0 ;
	
	public void OnPress( bool _Down )
	{
		if( true == _Down )
		{
			m_LocationSystem.TryPress( m_OptionIndex) ;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
