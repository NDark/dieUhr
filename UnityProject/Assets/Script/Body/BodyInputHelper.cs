using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInputHelper : MonoBehaviour
{
	public static BodyManager s_System = null;
	
	// Start is called before the first frame update
	void Start()
    {
		
	}

	// Update is called once per frame
	void Update()
    {
		if (true == m_IsPressed)
		{
			m_PressedPoint = Input.mousePosition;
			// m_System.DetectUserMouse(m_PressedPoint);
		}
	}

	void OnPress(bool _Down)
	{
		if (false == _Down // up
			&& true == m_IsPressed)
		{
			s_System.OnUserClick();
		}
		m_IsPressed = _Down;
	}

	private Vector3 m_PressedPoint = Vector3.zero;
	private bool m_IsPressed = false;

}
