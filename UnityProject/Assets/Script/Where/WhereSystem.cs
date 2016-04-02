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
	
	public float m_3DSceneRotateValue = 0 ;
	
	public WhereState m_State = WhereState.WhereState_None ;
	
	List<ScreenCollectData> m_WhereScreenVecs = new List<ScreenCollectData>() ;
	
	public bool m_IsAbleCollect = false ;
	public bool m_IsCollected = false ;
	
	public void TryNextInAnswerMode()
	{
		if( WhereState.WhereState_WaitInAnswerMode != m_State )
		{
			Debug.LogWarning( "TryNextInAnswerMode() invalid state." ) ;
			return ;
		}
		
		ReleaveScene( m_CurrentScene , m_Fussball ) ;
		
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
		if( WhereState.WhereState_WaitInAnswerMode != m_State )
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
		if( WhereState.WhereState_WaitInMoveMode != m_State )
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
		}
		
		
	}
	
	
	
	public void SetAbleCollectWhereOnScreen()
	{
		this.m_IsAbleCollect = true ;
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
		
			RandonmizeScene() ;
			RandonmizeWhere( m_CurrentScene ) ;
			
			SetPresentScene( m_CurrentScene , null , string.Empty ) ;
			
			m_WhereScreenVecs.Clear() ;
						
			m_IsAbleCollect = false ;
			m_IsCollected = false ;
			
			m_State = WhereState.WhereState_WaitInMoveMode ;
			break ;
		case WhereState.WhereState_WaitInMoveMode :
			
			break ;
		case WhereState.WhereState_WaitCorrectAnimation :
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
		
		_SceneObj.transform.localPosition = m_ScenesDropPos.position ;
		_SceneObj.transform.localRotation = m_ScenesDropPos.rotation ;
		Rigidbody r = _SceneObj.GetComponent<Rigidbody>() ;
		if( null != r )
		{
			r.isKinematic = false ;
		}
		
		if( string.Empty != _WhereKey )
		{
			Transform dummy = _SceneObj.transform.FindChild("Dummy_" + _WhereKey);
			if( null != _TargetObject && null != dummy )
			{
				_TargetObject.transform.parent = dummy.transform ;
				_TargetObject.transform.localPosition = Vector3.zero ;
			}
		}
	}
	
	public void ReleaveScene( GameObject _SceneObj , GameObject _TargetObject )
	{
		Debug.Log("ReleaveScene");
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
	
	private void RandonmizeWhere( GameObject _CurrentScene ) 
	{
		List<string> validWhereKey = new List<string>() ;
		for( int i = 0 ; i < m_WhereKey.Length ; ++i )
		{
			// Debug.Log ("RandonmizeScene() validWhereKey.Add=" + m_WhereKey[ i ] );
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
			Transform dummy = m_CurrentScene.transform.FindChild("Dummy_" + m_WhereKey[ i ] );
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
	
	
}

