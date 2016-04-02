﻿using UnityEngine;
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
	public GameObject m_CurrentScene = null ;
	public Transform m_ScenesStandbyPos = null ;
	public Transform m_ScenesDropPos = null ;
	public GameObject m_ReferenceObject = null ;
	public GameObject m_SceneParent = null ;
	
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
	
	private string [] m_SceneKey = 
	{
	"Desk" ,
		"Shelf" ,
	} ;
	
	private string [] m_WhereKey = 
	{
		"Uber" ,
		"Auf" ,
		"Unter" ,
		"Vor" ,
		"Hinter" ,
		"Neben" ,
		"An" ,
		"In" ,
		"Zwischen" ,
	} ;
	
	Dictionary<string , GameObject> m_Scenes = new Dictionary<string, GameObject>() ;
	
	string m_CurrentWhereKey = string.Empty ;
	string m_CurrentSceneKey = string.Empty ;
	
	public WhereState m_State = WhereState.WhereState_None ;
	public GameObject m_AnswerModeScrollRegion = null ;
	
	public void TryNextInAnswerMode()
	{
		if( WhereState.WhereState_WaitInAnswerMode == m_State )
		{
		
		}
		
		ReleaveScene( m_CurrentScene , m_Fussball ) ;
		
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
			
			RandonmizeScene() ;
			RandonmizeWhere( m_CurrentScene ) ;
			
			SetPresentScene( m_CurrentScene , m_Fussball , m_CurrentWhereKey ) ;
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
	
	public void SetPresentScene( GameObject _SceneObj , GameObject _TargetObject , string _WhereKey )
	{
		if( null == _TargetObject )
		{
			Debug.LogError("null == _TargetObject");
			return ;
		}
		if( null == _SceneObj )
		{
			Debug.LogError("null == _SceneObj");
			return ;
		}
		
		_SceneObj.transform.localPosition = m_ScenesDropPos.position ;
		_SceneObj.transform.localRotation = m_ScenesDropPos.rotation ;
		Rigidbody r = _SceneObj.GetComponent<Rigidbody>() ;
		if( null != r )
		{
			r.isKinematic = false ;
		}
		Transform dummy = _SceneObj.transform.FindChild("Dummy_" + _WhereKey);
		if( null != dummy )
		{
			_TargetObject.transform.parent = dummy.transform ;
			_TargetObject.transform.localPosition = Vector3.zero ;
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
	
	private void DoWhereState_Initialize()
	{
		if( null == m_SceneParent )
		{
			Debug.LogError("DoWhereState_Initialize() null == m_SceneParent") ;
			return ;
		}
		
		for( int i = 0 ; i < m_SceneKey.Length ; ++i )
		{
			Transform trans = m_SceneParent.transform.FindChild( m_SceneKey[i] + "Prefab" ) ;
			if( null != trans )
			{
				m_Scenes.Add( m_SceneKey[ i ] , trans.gameObject ) ;
			}
		}
	}

	private void RandonmizeScene()
	{
		int count = 0 ;
		int randomIndex = Random.Range( 0 , m_Scenes.Count ) ;
		var sceneEnum = m_Scenes.GetEnumerator() ;
		while( sceneEnum.MoveNext() )
		{
			if( count == randomIndex )
			{
				m_CurrentSceneKey = sceneEnum.Current.Key ;
				m_CurrentScene = sceneEnum.Current.Value ;
				Debug.LogWarning ("RandonmizeScene() m_CurrentSceneKey=" + m_CurrentSceneKey);
				break ;
			}
			++count ;
		}
	}
	
	private void RandonmizeWhere( GameObject _CurrentScene ) 
	{
		List<string> validWhereKey = new List<string>() ;
		for( int i = 0 ; i < m_WhereKey.Length ; ++i )
		{
			Debug.Log ("RandonmizeScene() validWhereKey.Add=" + m_WhereKey[ i ] );
			Transform dummy = _CurrentScene.transform.FindChild("Dummy_" + m_WhereKey[ i ] );
			if( null != dummy )
			{
				
				validWhereKey.Add( m_WhereKey[ i ] ) ;
			}					
		}
		
		
		int randomIndex = Random.Range( 0 , validWhereKey.Count ) ;
		m_CurrentWhereKey = validWhereKey[ randomIndex ] ;
		Debug.LogWarning ("RandonmizeScene() m_CurrentWhereKey=" + m_CurrentWhereKey);
	}
}

