using UnityEngine;
using System.Collections;

public class WhereSystem : MonoBehaviour 
{
	public GameObject m_Fussball = null ;
	public GameObject m_DeskObj = null ;

	// Use this for initialization
	void Start () 
	{
		if( null != m_Fussball && null != m_DeskObj )
		{
			Transform dummy = m_DeskObj.transform.FindChild("Dummy_Uber");
			if( null != dummy )
			{
				m_Fussball.transform.parent = dummy.transform ;
				m_Fussball.transform.localPosition = Vector3.zero ;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
