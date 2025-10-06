using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyVibrationHelper : MonoBehaviour
{
	public Transform m_VibrationTarget = null;
	Vector3 m_InitLocalPos = Vector3.zero;
	float m_VibrationRange = 0.05f ;
	float m_VibrationDuration = 0.3f;

	// Start is called before the first frame update
	void Start()
    {
		if(null!= m_VibrationTarget)
		{
			m_InitLocalPos = m_VibrationTarget.localPosition;
		}
	}

	float m_VibrationCheckTime = 0.0f ;
	bool m_IsVibration = false ;
	public void ActiveVibration()
	{
		m_IsVibration = true;
		m_VibrationCheckTime = Time.time + m_VibrationDuration;
	}

    // Update is called once per frame
    void Update()
    {
        if(m_IsVibration) 
		{
			Vector3 pos = m_InitLocalPos + Random.insideUnitSphere * m_VibrationRange;
			if (null != m_VibrationTarget)
			{
				m_VibrationTarget.localPosition = pos;
			}
			if(Time.time > m_VibrationCheckTime )
			{ 
				m_IsVibration = false;;
				m_VibrationTarget.localPosition = m_InitLocalPos;
			}
		}
    }
}
