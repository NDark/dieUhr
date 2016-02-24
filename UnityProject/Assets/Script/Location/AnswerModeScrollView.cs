using UnityEngine;
using System.Collections;

public class AnswerModeScrollView : MonoBehaviour 
{

	public LocationSystem m_System = null ;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPress( bool _Down ) 
	{
		Debug.Log("OnPress()" + _Down);
		
		if( true == _Down )
		{
			m_PressedPoint = Input.mousePosition ;
		}
		else if( true == m_IsPressed )
		{
			// release
			Debug.Log("m_PressedPoint" + m_PressedPoint ); 
			Debug.Log("Input.mousePosition" + Input.mousePosition ); 
			float yDiff = Input.mousePosition.y - m_PressedPoint.y ;
			if( null != m_System )
			{
				if( yDiff < 0 )
				{
					// try move down
					m_System.TryMoveDown() ;
				}
				else if( yDiff > 0 )
				{
					m_System.TryMoveUp() ;
				}
			}
		}
		
		m_IsPressed = _Down ;
	}

	private Vector3 m_PressedPoint = Vector3.zero ;	
	
	private bool m_IsPressed = false ;
}
