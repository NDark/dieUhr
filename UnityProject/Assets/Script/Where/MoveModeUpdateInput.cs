using UnityEngine;
using System.Collections;

public class MoveModeUpdateInput : MonoBehaviour 
{

	public WhereSystem m_System = null ;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( true == m_IsPressed )
		{
			m_PressedPoint = Input.mousePosition ;
			m_System.DetectUserMouse( m_PressedPoint ) ;
		}
	
	}
	
	void OnPress( bool _Down ) 
	{
		Debug.Log("OnPress()" + _Down);
		m_IsPressed = _Down ;
	}

	private Vector3 m_PressedPoint = Vector3.zero ;	
	
	private bool m_IsPressed = false ;
}
