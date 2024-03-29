﻿using UnityEngine;
using System.Collections.Generic;

public enum GenderOfNoun
{
	Male , 
	Female ,
	Neutral ,
	Plural
}

public enum WhereState
{
	WhereState_None = 0 ,
	WhereState_Initialize ,
	WhereState_EnterAnswerMode ,
	WhereState_WaitInAnswerMode ,
	WhereState_EnterMoveMode ,
	WhereState_WaitInMoveMode ,
	WhereState_WaitCorrectAnimation ,
	
	WhereState_EnterTeacherMode ,
	WhereState_WaitInTeacherMode ,
}

public class ScreenCollectData
{
	public string Key { get; set; }
	public GameObject DummyObj { get; set; }
	
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
	public GameObject m_ShuffleNextButton = null ;
	public GameObject m_RotateLeftButton = null ;
	public GameObject m_RotateRightButton = null ;
	public GameObject m_AnswerModeButton = null ;
	public GameObject m_MoveModeButton = null ;
	public GameObject m_ExampleButton = null ;
	public UILabel m_ExampleContent = null ;
	public GameObject m_InstructionText = null ;
	public TweenAlpha m_CorrectAlpha = null ;
	public AudioSource m_CorrectAudio = null ;
	public Camera m_2DCamera = null ;
	public GameObject m_MoveModeTouchRegion = null ;
	public Transform m_MoveModeFussballStandbyPos = null ;
	
	public TweenAlpha m_TeacherMenuAlpha = null ;
	public GameObject m_TeacherModeButton = null ;
	public UIPopupList m_TeacherModeScenePopUpList = null ;
	public UILabel m_TeacherModeScenePopUpListMainLabel = null;
	public UIPopupList m_TeacherModeWherePopUpList = null ;
	public UILabel m_TeacherModeWherePopUpListLabel = null;

	private Dictionary<string,string> m_TeacherModeSceneToKey = new Dictionary<string, string>() ;
	private Dictionary<string,string> m_TeacherModeWhereToKey = new Dictionary<string, string>() ;
	
	private string [] m_TargetKey = 
	{
		"Fussball" ,
	} ;
	
	private string [] m_SceneKey = 
	{
		"Desk" ,
		"Shelf" ,
		"Cabinet" ,
		"Television" ,		
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
	string m_CurrentSelectKey = string.Empty ;
	string m_CurrentReferenceKey = string.Empty ;
	
	public float m_3DSceneRotateValue = 0 ;
	
	public WhereState m_State = WhereState.WhereState_None ;
	
	List<ScreenCollectData> m_WhereScreenVecs = new List<ScreenCollectData>() ;
	
	public bool m_IsAbleCollect = false ;
	public bool m_IsCollected = false ;
	
	float m_ShowExampleWaitTime = 3.0f ;
	float m_ShowExampleSet = 0.0f ;
	
	public void TryEnterTeacherMode()
	{
		if( WhereState.WhereState_WaitInAnswerMode != m_State 
		   && WhereState.WhereState_WaitInMoveMode != m_State 
		   && WhereState.WhereState_WaitInTeacherMode != m_State 
		   )
		{
			Debug.LogWarning( "TryEnterTeacherMode() invalid state=" + m_State ) ;
			return ;
		}
		
		if( null == m_TeacherMenuAlpha )
		{
			Debug.LogError( "TryEnterTeacherMode() null == m_TeacherMenuAlpha" ) ;		
			return ;
		}
		
		InitTeachersModePopupList() ;
		
		m_TeacherMenuAlpha.PlayForward() ;
		
		NGUITools.SetActive( m_ShuffleNextButton , false ) ;
		NGUITools.SetActive( m_RotateLeftButton , false ) ;
		NGUITools.SetActive( m_RotateRightButton , false ) ;
		NGUITools.SetActive( m_ShuffleNextButton , false ) ;
		NGUITools.SetActive( m_MoveModeTouchRegion , false ) ;
		
		NGUITools.SetActive( m_MoveModeButton , true ) ;
		NGUITools.SetActive( m_AnswerModeButton , true ) ;
		
		ReleaveScene( m_CurrentScene , m_Fussball ) ;
		
		
		
		m_State = WhereState.WhereState_EnterTeacherMode ;
	}
	
	public void TryReselectScene()
	{
		if( WhereState.WhereState_EnterTeacherMode != m_State )
		{
			Debug.LogWarning( "TryWaitInTeacherMode() invalid state=" + m_State ) ;
			return ;
		}	
		
		
		string subjectKey = m_TeacherModeSceneToKey[m_TeacherModeScenePopUpListMainLabel.text ] ;
		m_CurrentSceneKey = subjectKey ;
		m_CurrentScene = m_Scenes[ m_CurrentSceneKey ] ;
		
		List<string> validWhereKeys = CollectValidWhereFromSceneObject( m_CurrentScene , false ) ;
		InitTeachersModePopupList_Where( validWhereKeys ) ;
		
	}
	
	public void TryWaitInTeacherMode()
	{
		if( WhereState.WhereState_EnterTeacherMode != m_State )
		{
			Debug.LogWarning( "TryWaitInTeacherMode() invalid state=" + m_State ) ;
			return ;
		}
		
		if( null == m_TeacherMenuAlpha )
		{
			Debug.LogError( "TryEnterTeacherMode() null == m_TeacherMenuAlpha" ) ;		
			return ;
		}
		
		m_TeacherMenuAlpha.PlayReverse() ;
		
		string subjectKey = m_TeacherModeSceneToKey[ m_TeacherModeScenePopUpListMainLabel.text ] ;
		string whereKey = m_TeacherModeWhereToKey[ m_TeacherModeWherePopUpListLabel.text ] ;
		m_CurrentSceneKey = subjectKey ;
		m_CurrentScene = m_Scenes[ m_CurrentSceneKey ] ;
		m_CurrentWhereKey = whereKey ;
		CheckWhereIsReference( m_CurrentScene ) ;
		
		ResetAnswerContent() ;
		ResetExampleContent() ;
		
		SetPresentScene( m_CurrentScene , m_Fussball , m_CurrentWhereKey ) ;
		
		m_State = WhereState.WhereState_WaitInTeacherMode ;
	}
	
	public void TryNextInAnswerMode()
	{
		if( WhereState.WhereState_WaitInAnswerMode != m_State )
		{
			Debug.LogWarning( "TryNextInAnswerMode() invalid state." ) ;
			return ;
		}
		
		ReleaveScene( m_CurrentScene , m_Fussball ) ;
		
		NGUITools.SetActive( this.m_InstructionText , false ) ;
		
		m_State = WhereState.WhereState_EnterAnswerMode ;
	}
	
	public void TryRotateLeft()
	{
		float nextAngle = m_3DSceneRotateValue - 20 ; 
		if( nextAngle < -80 )
		{
			return ;
		}
		
		m_3DSceneRotateValue = nextAngle ;
		
		this.transform.rotation = Quaternion.AngleAxis( m_3DSceneRotateValue , Vector3.up ) ;
	}
	
	public void TryRotateRight()
	{
		float nextAngle = m_3DSceneRotateValue + 20 ; 
		if( nextAngle > 60 )
		{
			return ;
		}
		
		m_3DSceneRotateValue = nextAngle ;
		
		this.transform.rotation = Quaternion.AngleAxis( m_3DSceneRotateValue , Vector3.up ) ;
	}
	
	
	public void TrySwitchToMoveMode()
	{
		if( WhereState.WhereState_WaitInAnswerMode != m_State 
		   && WhereState.WhereState_WaitInTeacherMode != m_State )
		{
			Debug.LogWarning( "TrySwitchToMoveMode() invalid state." ) ;
			return ;
		}
		
		ReleaveScene( m_CurrentScene , m_Fussball ) ;
		
		SwitchGUI( false ) ;
		
		m_State = WhereState.WhereState_EnterMoveMode ;
	}

	public void TrySwitchToAnswerMode()
	{
		if( WhereState.WhereState_WaitInMoveMode != m_State 
		   && WhereState.WhereState_WaitInTeacherMode != m_State )
		{
			Debug.LogWarning( "TrySwitchToAnswerMode() invalid state." ) ;
			return ;
		}
		
		
		ReleaveScene( m_CurrentScene , m_Fussball ) ;
		
		
		this.transform.rotation = Quaternion.identity ;
		SwitchGUI( true ) ;
		
		m_State = WhereState.WhereState_EnterAnswerMode ;
	}
	
	public void DetectUserMouse( Vector3 _MousePosition )
	{
		if( m_State != WhereState.WhereState_WaitInMoveMode )
		{
			Debug.LogError("DetectUserMouse() invalid state.");
			return ;
		}
		
		if( false == m_IsAbleCollect )
		{
			Debug.LogWarning( "DetectUserMouse() false == m_IsAbleCollect." ) ;
			return ;
		}
		
	
		if( WhereState.WhereState_WaitInMoveMode != m_State )
		{
			Debug.LogWarning( "DetectUserMouse() invalid state." ) ;
			return ;
		}
		
		if( false == m_IsCollected )
		{
			CollectWhereOnScreen() ;
			
		}
		
		if( m_WhereScreenVecs.Count <= 0 )
		{
			Debug.LogWarning( "DetectUserMouse() m_WhereScreenVecs is empty." ) ;
			return ;
		}
		
		NGUITools.SetActive( this.m_InstructionText , false ) ;
		
		int minIndex = -1 ;		
		float minDistance = 999.99f ;
		Debug.Log("_MousePosition" + _MousePosition );
		for( int i = 0 ; i < m_WhereScreenVecs.Count ; ++i )
		{
			Vector3 screenPos = m_WhereScreenVecs[ i ].DummyObj.transform.position ;
			
			screenPos = Camera.main.WorldToScreenPoint( screenPos ) ;
			screenPos.z = 0 ;
			// Debug.Log("screenPos=" + screenPos );
						
			float tmpDis = Vector3.Distance( _MousePosition , screenPos ) ;
			if( tmpDis < minDistance )
			{
				minDistance = tmpDis ;
				minIndex = i ;
			}
		}

		if( -1 != minIndex && minIndex < m_WhereScreenVecs.Count )
		{
			Debug.LogWarning("minIndex=" + m_WhereScreenVecs[ minIndex ].Key );
			m_Fussball.transform.position = m_WhereScreenVecs[ minIndex ].DummyObj.transform.position ;
			Debug.LogWarning("m_CurrentWhereKey=" + m_CurrentWhereKey );
			m_CurrentSelectKey = m_WhereScreenVecs[ minIndex ].Key ;
			
		}
		
		
	}
	
	public void DecideUserChoice()
	{
		if( m_State != WhereState.WhereState_WaitInMoveMode )
		{
			Debug.LogError("DetectUserMouse() invalid state.");
			return ;
		}
		
		if( m_CurrentWhereKey == m_CurrentSelectKey )
		{
			PlayCorrectAnimation( true ) ;
			m_CorrectAnswerWaitTime = Time.timeSinceLevelLoad + m_CorrectAnswerWaitSec ;
			m_State = WhereState.WhereState_WaitCorrectAnimation ;
		}
	}
	
	
	public void SetAbleCollectWhereOnScreen()
	{
		this.m_IsAbleCollect = true ;
		
		
		
		if( WhereState.WhereState_WaitInMoveMode == m_State )
		{
			// set fuss ball to specified standby positon
			m_Fussball.transform.position = m_MoveModeFussballStandbyPos.position ;
		}
		
	}
	
	public void ResetAnswerContent()
	{
		if( m_State == WhereState.WhereState_WaitInAnswerMode 
		   || m_State == WhereState.WhereState_EnterAnswerMode 
		   || m_State == WhereState.WhereState_EnterTeacherMode 
		   || m_State == WhereState.WhereState_WaitInTeacherMode 
		   )
		{
			m_AnswerLabel.text = CreateAnswer( m_TargetKey[ 0 ] , m_CurrentSceneKey , m_CurrentWhereKey , this.m_CurrentReferenceKey ) ;
		}
		else if( m_State == WhereState.WhereState_WaitInMoveMode 
		        || m_State == WhereState.WhereState_EnterMoveMode )
		{
			m_AnswerLabel.text = CreateInstruction( m_TargetKey[ 0 ] , m_CurrentSceneKey , m_CurrentWhereKey , this.m_CurrentReferenceKey  ) ;
		}
		
	}
	
	public void ResetExampleContent()
	{
		string exampleKey = GetExampleKey( m_CurrentWhereKey ) ;
		string exampleSentence = Localization.Get( exampleKey );
		UpdateExampleContent( exampleSentence ) ;
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
			
			ResetAnswerContent() ;
			ResetExampleContent() ;
			
			SetPresentScene( m_CurrentScene , m_Fussball , m_CurrentWhereKey ) ;
			m_State = WhereState.WhereState_WaitInAnswerMode ;
			break ;
		case WhereState.WhereState_WaitInAnswerMode :
		
			CheckExampleTimer() ;
			
			break ;
		case WhereState.WhereState_EnterMoveMode :
		
			RandonmizeScene() ;
			RandonmizeWhere( m_CurrentScene ) ;
			
			ResetAnswerContent() ;
			ResetExampleContent() ;
			
			SetPresentScene( m_CurrentScene , null , string.Empty ) ;
			
			m_WhereScreenVecs.Clear() ;
						
			m_IsAbleCollect = false ;
			m_IsCollected = false ;
			
			m_State = WhereState.WhereState_WaitInMoveMode ;
			break ;
		case WhereState.WhereState_WaitInMoveMode :
			CheckExampleTimer() ;
			break ;
		case WhereState.WhereState_WaitCorrectAnimation :
			DoWhereState_WaitCorrectAnimation() ;
			break ;
			
		case WhereState.WhereState_EnterTeacherMode :
			break ;
		case WhereState.WhereState_WaitInTeacherMode :
			
			CheckExampleTimer() ;
			
			break ;
		}
	}
	
	public void SetPresentScene( GameObject _SceneObj , GameObject _TargetObject , string _WhereKey )
	{
		if( null == _SceneObj )
		{
			Debug.LogError("null == _SceneObj");
			return ;
		}
		
		_SceneObj.transform.position = m_ScenesDropPos.position ;
		_SceneObj.transform.rotation = m_ScenesDropPos.rotation ;
		Rigidbody r = _SceneObj.GetComponent<Rigidbody>() ;
		if( null != r )
		{
			r.isKinematic = false ;
		}
		
		if( string.Empty != _WhereKey )
		{
			Transform dummy = _SceneObj.transform.Find("Dummy_" + _WhereKey);
			if( null != _TargetObject && null != dummy )
			{
				_TargetObject.transform.parent = dummy.transform ;
				_TargetObject.transform.localPosition = Vector3.zero ;
			}
		}
		
		ResetExampleTimer() ;
	}
	
	public void ReleaveScene( GameObject _SceneObj , GameObject _TargetObject )
	{
		if( null == _SceneObj )
		{
			Debug.LogError("null == _SceneObj");
			return ;
		}
		if( null == _TargetObject )
		{
			Debug.LogError("null == _TargetObject");
			return ;
		}
		

		_SceneObj.transform.localPosition = m_ScenesStandbyPos.position ;
		_SceneObj.transform.localRotation = m_ScenesStandbyPos.rotation ;
		Rigidbody r = _SceneObj.GetComponent<Rigidbody>() ;
		if( null != r )
		{
			r.isKinematic = true ;
		}
		_TargetObject.transform.parent = this.transform ;
		_TargetObject.transform.position = m_ScenesStandbyPos.position ;
		
		Transform referenceDummy = _SceneObj.transform.Find("Dummy_Reference" );
		if( null != referenceDummy )
		{
			for( int j = 0 ; j < referenceDummy.childCount ; ++j )
			{
				Transform existChild = referenceDummy.GetChild( j ) ;
				if( null != existChild )
				{
					existChild.transform.parent = this.transform ;
					existChild.transform.position = m_ScenesStandbyPos.position ;
				}
			}
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
			Transform trans = m_SceneParent.transform.Find( m_SceneKey[i] + "Prefab" ) ;
			if( null != trans )
			{
				m_Scenes.Add( m_SceneKey[ i ] , trans.gameObject ) ;
			}
		}
		
		this.transform.rotation = Quaternion.identity ;
		SwitchGUI( true ) ;
		
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
	
	private List<string> CollectValidWhereFromSceneObject( GameObject _CurrentScene , bool m_CurrentWhereKeyExclusive ) 
	{
		List<string> ret = new List<string>() ;
		for( int i = 0 ; i < m_WhereKey.Length ; ++i )
		{
			if( true == m_CurrentWhereKeyExclusive 
			&& m_CurrentWhereKey == m_WhereKey[ i ] )
			{
				continue ;
			}
			
			// Debug.Log ("RandonmizeWhere() validWhereKey.Add=" + m_WhereKey[ i ] );
			Transform dummy = _CurrentScene.transform.Find("Dummy_" + m_WhereKey[ i ] );
			if( null != dummy )
			{
				ret.Add( m_WhereKey[ i ] ) ;
			}					
		}
		return ret ;
	}
	
	private void CheckWhereIsReference( GameObject _CurrentScene )
	{
		if( "Zwischen" == m_CurrentWhereKey )
		{
			// randomize a reference object from scene
			GameObject referenceObj = RandomizeAnotherScene( m_CurrentSceneKey ) ;
			
			Transform referenceDummy = _CurrentScene.transform.Find("Dummy_Reference" );
			if( null != referenceDummy )
			{
				referenceObj.transform.parent = referenceDummy.transform ;
				referenceObj.transform.localPosition = Vector3.zero ;
				referenceObj.transform.localRotation = Quaternion.identity ;
				// Debug.LogWarning ("RandonmizeWhere() referenceObj=" + referenceObj.name );	
			}	
		}
	}
	
	private void RandonmizeWhere( GameObject _CurrentScene ) 
	{
		List<string> validWhereKey 
		= CollectValidWhereFromSceneObject( _CurrentScene , true ) ;
		
		if( 0 == validWhereKey.Count )
		{
			Debug.LogError("0 == validWhereKey");
			return ;
		}
		
		int randomIndex = Random.Range( 0 , validWhereKey.Count ) ;
		m_CurrentWhereKey = validWhereKey[ randomIndex ] ;
		// Debug.LogWarning ("RandonmizeWhere() m_CurrentWhereKey=" + m_CurrentWhereKey);
		
		CheckWhereIsReference( _CurrentScene ) ;

		
		ShowExampleButton( false ) ;
	}
	
	GameObject RandomizeAnotherScene( string _ExclusiveScene )
	{
		List<string> validVec = new List<string>() ;
		var sceneEnum = m_Scenes.GetEnumerator() ;
		while( sceneEnum.MoveNext() )
		{
			if( sceneEnum.Current.Key != _ExclusiveScene )
			{
				validVec.Add( sceneEnum.Current.Key ) ;
			}
		}	
		
		int randomIndex = Random.Range( 0 , validVec.Count ) ;
		m_CurrentReferenceKey = validVec[ randomIndex ] ;
		return m_Scenes[ m_CurrentReferenceKey ] ;
	}
	
	void SwitchGUI( bool _AnswerMode )
	{
		NGUITools.SetActive( m_RotateLeftButton , !_AnswerMode ) ;
		NGUITools.SetActive( m_RotateRightButton , !_AnswerMode ) ;
		NGUITools.SetActive( m_ShuffleNextButton , _AnswerMode ) ;
		
		NGUITools.SetActive( m_MoveModeButton , _AnswerMode ) ;
		NGUITools.SetActive( m_AnswerModeButton , !_AnswerMode ) ;
		
		NGUITools.SetActive( m_MoveModeTouchRegion , !_AnswerMode ) ;
	}

	void CollectWhereOnScreen()
	{
		
		for( int i = 0 ; i < m_WhereKey.Length ; ++i )
		{
			if( "Zwischen" != m_CurrentWhereKey 
			   && "Zwischen" == m_WhereKey[ i ] )
			{
				continue ;
			}
			
			Transform dummy = m_CurrentScene.transform.Find("Dummy_" + m_WhereKey[ i ] );
			if( null != dummy )
			{
				ScreenCollectData data = new ScreenCollectData() ;
				data.Key = m_WhereKey[ i ] ;
				data.DummyObj = dummy.gameObject ;
				
				this.m_WhereScreenVecs.Add( data ) ;
			}					
		}
		
		
		m_IsCollected = true ;
	}
	
	private void ResetExampleTimer()
	{
		float waitTime = Random.Range( m_ShowExampleWaitTime/2.0f , m_ShowExampleWaitTime ) ;
		m_ShowExampleSet = Time.timeSinceLevelLoad + waitTime ;
	}
	
	private void CheckExampleTimer()
	{
		if( Time.timeSinceLevelLoad > m_ShowExampleSet )
		{
			ShowExampleButton( true ) ;
		}
	}
	
	private void ShowExampleButton( bool _Show )
	{
		if( null == this.m_ExampleButton )
		{
			return ;
		}
		
		NGUITools.SetActive( m_ExampleButton , _Show ) ;
	}
	
	private void UpdateExampleContent( string _Content )
	{
		if( null == m_ExampleContent )
		{
			return ;
		}
		
		m_ExampleContent.text = _Content ; 
	}

	public string CreateAnswer( string _TargetKey 
	, string _SceneKey 
	                           , string _WhereKey 
	                           , string _ReferenceKey )
	{
//		Debug.Log("CreateAnswer()" + _TargetKey );
//		Debug.Log("CreateAnswer()" + _SceneKey );
//		Debug.Log("CreateAnswer()" + _WhereKey );
//		
		string localizationWhereKey = "WhereKey_" + _WhereKey ;
		string localWhereString = Localization.Get( localizationWhereKey ) ;
		
		string targetString = Localization.Get( "WhereTarget_" + _TargetKey ) ;
		
		string sceneString = Localization.Get( "WhereScene_" + _SceneKey ) ;
		sceneString = (Localization.language == "Polish")
			? Polish_ConjugateTheNoun(sceneString, _SceneKey, _WhereKey)
			: German_DativTheNoun(sceneString);
		
		string referenceString = string.Empty;
		if( string.Empty != _ReferenceKey)// between there are 2 objects.
		{
			referenceString = Localization.Get( "WhereScene_" + _ReferenceKey ) ;
			referenceString = (Localization.language == "Polish")
				? Polish_ConjugateTheNoun(referenceString, _ReferenceKey, _WhereKey)
				: German_DativTheNoun(referenceString);

		}
		
		localWhereString = localWhereString.Replace( "<target>" , targetString ) ;
		localWhereString = localWhereString.Replace( "<scene>" , sceneString ) ;
		localWhereString = localWhereString.Replace( "<reference>" , referenceString ) ;
		localWhereString = ReplaceDativShort( localWhereString ) ;
		localWhereString = ReplaceFirstUpperCase( localWhereString ) ;
		return localWhereString ;
	}	
	
	public string CreateInstruction( string _TargetKey 
	                           , string _SceneKey 
	                           , string _WhereKey 
	                           , string _ReferenceKey )
	{
//		Debug.Log("CreateInstruction()" + _TargetKey );
//		Debug.Log("CreateInstruction()" + _SceneKey );
//		Debug.Log("CreateInstruction()" + _WhereKey );
//		
		string localizationWhereKey = "WhereInstruction_" + _WhereKey ;
		string localWhereString = Localization.Get( localizationWhereKey ) ;
		
		string targetString = Localization.Get( "WhereTarget_" + _TargetKey ) ;
		targetString = AkkusativTheNoun( targetString ) ;
		string sceneString = Localization.Get( "WhereScene_" + _SceneKey ) ;
		sceneString = AkkusativTheNoun( sceneString ) ;
		
		string referenceString = string.Empty ;
		if( string.Empty != _ReferenceKey )
		{
			referenceString = Localization.Get( "WhereScene_" + _ReferenceKey ) ;
			referenceString = AkkusativTheNoun( referenceString ) ;
		}
		
		
		localWhereString = localWhereString.Replace( "<target>" , targetString ) ;
		localWhereString = localWhereString.Replace( "<scene>" , sceneString ) ;
		localWhereString = localWhereString.Replace( "<reference>" , referenceString ) ;
		localWhereString = ReplaceDativShort( localWhereString ) ;
		localWhereString = ReplaceFirstUpperCase( localWhereString ) ;
		return localWhereString ;
	}	
	
	private string ReplaceDativShort( string _Src )
	{
		string ret = _Src ;
		ret = ret.Replace("in dem" , "im" ) ;
		ret = ret.Replace("an dem" , "am" ) ;
		return ret ;
	}
	private string ReplaceFirstUpperCase( string _Src )
	{
		string ret = string.Empty ;
		string p1 = _Src.Substring( 0 , 1 ) ;
		string p2 = _Src.Substring( 1 ) ;
		ret = p1.ToUpper() + p2 ;
		return ret ;
	}
	
	private void PlayCorrectAnimation( bool _Forward )
	{
		if( null == m_CorrectAlpha )
		{
			return ;
		}
		
		if( true == _Forward )
		{
			m_CorrectAlpha.PlayForward() ;
			m_CorrectAudio.Play() ;
		}
		else
		{
			m_CorrectAlpha.PlayReverse() ;
		}
	}

	private void DoWhereState_WaitCorrectAnimation()
	{
		if( Time.timeSinceLevelLoad > m_CorrectAnswerWaitTime )
		{
			ReleaveScene( m_CurrentScene , m_Fussball ) ;			
			PlayCorrectAnimation( false ) ;
			m_State = WhereState.WhereState_EnterMoveMode ;
		}
	}	
	
	private GenderOfNoun GetGender( string _Input )
	{
		string lowerString = _Input.ToLower() ;
		if( -1 != lowerString.IndexOf( "der" ) )
		{
			return GenderOfNoun.Male ;
		}
		else if( -1 != lowerString.IndexOf( "die" ) 
		        && lowerString.Length - 1 == lowerString.LastIndexOf( "n" ) )
		{
			return GenderOfNoun.Plural ;
		}
		else if( -1 != lowerString.IndexOf( "das" ) )
		{
			return GenderOfNoun.Neutral ;
		}
		else // if( -1 != lowerString.IndexOf( "die" ) )
		{
			return GenderOfNoun.Female ;
		}

	}
	
	private string AkkusativTheNoun( string _Input )
	{
		string ret = _Input ;
		GenderOfNoun gender = GetGender( _Input ) ;
		switch( gender )
		{
		case GenderOfNoun.Male :
			ret = ret.Replace( "der" , "den" ) ;
			break ;
		}
		return ret ;
	}
	
	
	private string German_DativTheNoun( string _Input )
	{
		string ret = _Input ;
		GenderOfNoun gender = GetGender( _Input ) ;
		switch( gender )
		{
		case GenderOfNoun.Male :
			ret = ret.Replace( "der" , "dem" ) ;
			break ;
		case GenderOfNoun.Female :
			ret = ret.Replace( "die" , "der" ) ;
			break ;
		case GenderOfNoun.Neutral :
			ret = ret.Replace( "das" , "dem" ) ;
			break ;
		case GenderOfNoun.Plural :
			ret = ret.Replace( "die" , "den" ) ;
			break ;
		}
		return ret ;
	}

	private string Polish_ConjugateTheNoun(string _Input , string objKey , string whereKey )
	{
		string ret = _Input;

		string orgWhereValue = Localization.Get("WhereScene_" + objKey);
		string replaceWhereValue = string.Empty ;

		switch (whereKey)
		{
			case "Uber":
			case "In":
				// _dative
				replaceWhereValue = Localization.Get(objKey + "_dative");
				// biurko -> biurka
				ret = ret.Replace(orgWhereValue, replaceWhereValue);
				break;

			case "Vor":
			case "Hinter":
			case "An":
			case "Neben":
				// _genitive
				replaceWhereValue = Localization.Get(objKey + "_genitive");
				// biurko -> biurku
				ret = ret.Replace(orgWhereValue, replaceWhereValue);
				break;

			case "Auf":
			case "Unter":
			case "Zwischen":
				// _instrumental
				replaceWhereValue = Localization.Get(objKey + "_instrumental");
				// biurko -> biurkiem
				ret = ret.Replace(orgWhereValue, replaceWhereValue);
				break;
		}
		return ret;
	}

	private string GetExampleKey( string _WhereKey )
	{
		return "WhereExample_" + _WhereKey ;
	}
	
	private void InitTeachersModePopupList_Where( List<string> _WhereKey )
	{
		
		if( null != m_TeacherModeWherePopUpList )
		{
			m_TeacherModeWherePopUpList.Clear() ;
			m_TeacherModeWhereToKey.Clear() ;
			string whereString = "" ;
			for( int i = 0 ; i < _WhereKey.Count ; ++i )
			{
				whereString = Localization.Get( "WhereKey_" + _WhereKey[i] ) ;
				whereString = whereString.Replace( "<target>" , "..." ) ;
				whereString = whereString.Replace( "<scene>" , "..." ) ;
				whereString = whereString.Replace( "<reference>" , "..." ) ;
				whereString = whereString.Replace( "(Dativ)" , "" ) ;
				
				m_TeacherModeWherePopUpList.AddItem( whereString ) ;
				m_TeacherModeWhereToKey.Add( whereString , _WhereKey[ i ] ) ;
			}
			m_TeacherModeWherePopUpListLabel.text = whereString ;
			
		}
	}
	
	private void InitTeachersModePopupList()
	{
		if( null != m_TeacherModeScenePopUpList )
		{
			m_TeacherModeScenePopUpList.Clear() ;
			m_TeacherModeSceneToKey.Clear() ;
			string sceneString = string.Empty ;
			for( int i = 0 ; i < m_SceneKey.Length ; ++i )
			{
				sceneString = Localization.Get( "WhereScene_" + m_SceneKey[i] ) ;
				m_TeacherModeScenePopUpList.AddItem( sceneString ) ;
				m_TeacherModeSceneToKey.Add( sceneString , m_SceneKey[ i ] ) ;
			}
			
			m_TeacherModeScenePopUpListMainLabel.text = sceneString ;

		}
		
		List<string> validWhereKeys = 
			CollectValidWhereFromSceneObject( 
				m_Scenes[ 
					m_TeacherModeSceneToKey[ m_TeacherModeScenePopUpListMainLabel.text ] ] 
			, false ) ;
		InitTeachersModePopupList_Where( validWhereKeys ) ;
		
	}
	
	private float m_CorrectAnswerWaitSec = 1.0f ;
	private float m_CorrectAnswerWaitTime = 0.0f ;		
}

