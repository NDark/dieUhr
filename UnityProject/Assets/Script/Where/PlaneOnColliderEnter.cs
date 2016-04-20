using UnityEngine;
using System.Collections;

public class PlaneOnColliderEnter : MonoBehaviour 
{
	public WhereSystem m_System = null ;
	void OnTriggerEnter( Collider _Collider )
	{
		Debug.LogWarning("OnTriggerEnter()");
		m_System.SetAbleCollectWhereOnScreen() ;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
