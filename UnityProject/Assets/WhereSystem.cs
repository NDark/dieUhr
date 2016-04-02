using UnityEngine;
using System.Collections;

public class WhereSystem : MonoBehaviour 
{
	public GameObject m_Fussball = null ;
	public GameObject m_DeskObj = null ;
	public Transform m_ScenesStandbyPos = null ;
	public Transform m_ScenesDropPos = null ;

	// Use this for initialization
	void Start () 
	{
		SetPresentScene( m_DeskObj , m_Fussball ) ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ReleaveScene( m_DeskObj , m_Fussball ) ;
	
	}
	
	public void SetPresentScene( GameObject _SceneObj , GameObject _TargetObject )
	{
		if( null != _TargetObject && null != _SceneObj )
		{
			_SceneObj.transform.localPosition = m_ScenesDropPos.position ;
			_SceneObj.transform.localRotation = m_ScenesDropPos.rotation ;
			Transform dummy = _SceneObj.transform.FindChild("Dummy_Uber");
			if( null != dummy )
			{
				_TargetObject.transform.parent = dummy.transform ;
				_TargetObject.transform.localPosition = Vector3.zero ;
			}
		}
		
	}
	
	public void ReleaveScene( GameObject _SceneObj , GameObject _TargetObject )
	{
		if( null != _TargetObject && null != _SceneObj )
		{
			_SceneObj.transform.localPosition = m_ScenesStandbyPos.position ;
			_SceneObj.transform.localRotation = m_ScenesStandbyPos.rotation ;			
			Rigidbody r = _SceneObj.GetComponent<Rigidbody>() ;
			r.isKinematic = true ;
			_TargetObject.transform.parent = this.transform ;
		}
	}
}
