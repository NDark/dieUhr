using UnityEngine;
using System.Collections.Generic;

public enum WhereState
{
	WhereState_None = 0 ,
	WhereState_Initialize ,
	WhereState_EnterAnswerMode ,
	WhereState_WaitInAnswerMode ,
	WhereState_EnterMoveMode ,
	WhereState_WaitInMoveMode ,
	WhereState_WaitCorrectAnimation ,
}

public class WhereSystem : MonoBehaviour 
{
	public GameObject m_Fussball = null ;
	public GameObject m_DeskObj = null ;
	public Transform m_ScenesStandbyPos = null ;
	public Transform m_ScenesDropPos = null ;
	public GameObject m_ReferenceObject = null ;
	
	// 2D
	public UILabel m_AnswerLabel = null ;
	public GameObject m_ArrowUpButton = null ;
	public GameObject m_ArrowDownButton = null ;
	public GameObject m_AnswerModeButton = null ;
	public GameObject m_MoveModeButton = null ;
	public GameObject m_ExampleButton = null ;
	public UILabel m_ExampleContent = null ;
	public GameObject m_InstructionText = null ;
	public TweenAlpha m_CorrectAlpha = null ;
	public AudioSource m_CorrectAudio = null ;
	
	public string [] m_SceneKey = 
	{
	"Desk" ,
	} ;
	
	public string [] m_WhereKey = 
	{
		"Uber" ,
	} ;
	
	Dictionary<string , GameObject> m_Scenes = new Dictionary<string, GameObject>() ;
	
	int m_CurrentWhereIndex = 0 ;
	int m_CurrentSceneIndex = 0 ;
	
	public WhereState m_State = WhereState.WhereState_None ;
	public GameObject m_AnswerModeScrollRegion = null ;
	
	public void TryNextInAnswerMode()
	{
		if( WhereState.WhereState_WaitInAnswerMode == m_State )
		{
		
		}
		
		ReleaveScene( m_DeskObj , m_Fussball ) ;
		m_State = WhereState.WhereState_EnterAnswerMode ;
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		switch( m_State )
		{
		case WhereState.WhereState_None :
			m_State = WhereState.WhereState_Initialize ;
			break ;
		case WhereState.WhereState_Initialize :
			DoWhereState_Initialize() ;
			m_State = WhereState.WhereState_EnterAnswerMode;
			break ;
		case WhereState.WhereState_EnterAnswerMode :
			SetPresentScene( m_DeskObj , m_Fussball ) ;
			m_State = WhereState.WhereState_WaitInAnswerMode ;
			break ;
		case WhereState.WhereState_WaitInAnswerMode :
			break ;
		case WhereState.WhereState_EnterMoveMode :
			break ;
		case WhereState.WhereState_WaitInMoveMode :
			break ;
		case WhereState.WhereState_WaitCorrectAnimation :
			break ;
		}
	}
	
	public void SetPresentScene( GameObject _SceneObj , GameObject _TargetObject )
	{
		if( null != _TargetObject && null != _SceneObj )
		{
			_SceneObj.transform.localPosition = m_ScenesDropPos.position ;
			_SceneObj.transform.localRotation = m_ScenesDropPos.rotation ;
			Rigidbody r = _SceneObj.GetComponent<Rigidbody>() ;
			if( null != r )
			{
				r.isKinematic = false ;
			}
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
			if( null != r )
			{
				r.isKinematic = true ;
			}
			_TargetObject.transform.parent = this.transform ;
		}
	}
	
	public void DoWhereState_Initialize()
	{
	
	}
	
}

