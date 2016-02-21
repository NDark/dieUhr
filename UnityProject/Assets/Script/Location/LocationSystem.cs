using UnityEngine;
using System.Collections;

public enum AnswerMode
{
	AnswerMode_Invalid = 0 ,
	AnswerMode_EnterCorrectAnswer ,
	AnswerMode_WaitCorrectAnswer ,
	AnswerMode_ToOptionMode ,
	AnswerMode_WaitAnimation ,
	AnswerMode_WaitPressOption ,
}

public class LocationSystem : MonoBehaviour 
{
	public GameObject referenceObject = null ;
	public UILabel answerLabel = null ;
	public GameObject arrowUpButton = null ;
	public GameObject arrowDownButton = null ;
	public GameObject answerModeButton = null ;
	public GameObject optionModeModeButton = null ;
	public UIGrid grid = null ;
	
	private int m_TargetIndex = 0 ;
	private Vector3 [] m_TargetPositions = new Vector3[2] ;
	private string [] m_AnswerStrings = null ;
	private string [] m_Keys = null ;
	
	public bool IsInAnimation { get ; set ; }
	
	public float m_MovingSpeed = 0.1f ;
	
	public UILabel [] m_Options = null ;
	
	
	public AnswerMode m_AnswerMode = AnswerMode.AnswerMode_Invalid ;
	
	public void TrySwitchToAnswerMode()
	{
		if( m_AnswerMode != AnswerMode.AnswerMode_WaitPressOption )
		{
			return ;
		}
		m_AnswerMode = AnswerMode.AnswerMode_EnterCorrectAnswer ;
	}
	
	public void TrySwitchToOptionMode()
	{
		if( m_AnswerMode != AnswerMode.AnswerMode_WaitCorrectAnswer )
		{
			return ;
		}
		m_AnswerMode = AnswerMode.AnswerMode_ToOptionMode ;
	}
	
	
	public void TryPress( int _OptionIndex )
	{
		Debug.Log("TryPress" + _OptionIndex ) ;
	}
	
	public void TryMoveUp()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		
		// actually we move reference up 70
		ChangeTargetAnimation( m_TargetIndex - 1 ) ;

	}
	public void TryMoveDown()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		ChangeTargetAnimation( m_TargetIndex + 1 ) ;
	}
	

	// Use this for initialization
	void Start () 
	{
		InitializeReferencePlanes() ;
		InitializeOptions() ;
		
		IsInAnimation = true ;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch( m_AnswerMode )
		{
		case AnswerMode.AnswerMode_Invalid :
			m_AnswerMode = AnswerMode.AnswerMode_EnterCorrectAnswer ;
			break ;
		case AnswerMode.AnswerMode_EnterCorrectAnswer :
			AnswerMode_EnterCorrectAnswer() ;
			m_AnswerMode = AnswerMode.AnswerMode_WaitCorrectAnswer ;
			break ;
		case AnswerMode.AnswerMode_WaitCorrectAnswer :
			if( true == this.IsInAnimation )
			{
				if( true == UpdateReference() )
				{
					NGUITools.SetActive( answerLabel.gameObject , true ) ;
					NGUITools.SetActiveChildren( answerLabel.gameObject , true ) ;
				}
			}		
			break ;
		case AnswerMode.AnswerMode_ToOptionMode :
			RandomizeTheOptions() ;
			AnswerMode_ToOptionMode() ;
			m_AnswerMode = AnswerMode.AnswerMode_WaitAnimation ;
			break ;
		case AnswerMode.AnswerMode_WaitAnimation :
			if( true == this.IsInAnimation )
			{
				if( true == UpdateReference() )
				{

				}
			}
			else
			{
				m_AnswerMode = AnswerMode.AnswerMode_WaitPressOption ;
			}
			break ;
		case AnswerMode.AnswerMode_WaitPressOption :

			break ;
			
		}
		
	
	}
	
	private bool UpdateReference()
	{
		if( null == referenceObject || null == answerLabel )
		{
			return false ;
		}
		// Debug.Log("m_TargetIndex=" + m_TargetIndex ) ;
		Vector3 currentPos = referenceObject.transform.position ;
		Vector3 targetPos = m_TargetPositions [ m_TargetIndex ] ;
		float dist = Vector3.Distance( currentPos , targetPos ) ;
		if( dist <= 0.5f )
		{
			referenceObject.transform.position = targetPos ;
			answerLabel.text = m_AnswerStrings[ m_TargetIndex ] ;
			this.IsInAnimation = false ;
			return true ;
		}
		else
		{
			Vector3 nextPos = Vector3.Lerp( currentPos , targetPos 
			                               , m_MovingSpeed ) ;
			referenceObject.transform.position = nextPos;
		}
		return false ;
	}
	
	private void RandomizeTheOptions()
	{
		
		int []remapTable = new int[ m_AnswerStrings.Length ] ;
		for( int i = 0 ; i < remapTable.Length ; ++i )
		{
			remapTable[ i ] = i ;
		}
		
		for( int i = 0 ; i < 10 ; ++i )
		{
			int index0 = Random.Range( 0 , m_AnswerStrings.Length ) ;
			int index1 = Random.Range( 0 , m_AnswerStrings.Length ) ;
			int tmp = remapTable[ index0 ] ;
			remapTable[ index0 ] = remapTable[ index1 ] ;
			remapTable[ index1 ] = tmp ;
		}
		
		
		for( int i = 0 ; i < this.m_Options.Length ; ++i )
		{
			if( i < m_AnswerStrings.Length )
			{
				// Debug.Log("remapTable[ i ]" + remapTable[ i ]);
				m_Options[ i ].text = m_AnswerStrings[ remapTable[ i ] ] ;
			}
			else
			{
				// hide the options
				NGUITools.SetActive( m_Options[ i ].gameObject , false ) ;
			}
		}
		
		if( null != grid )
		{
			grid.Reposition() ;
		}
		
		int minSize = Mathf.Min( m_AnswerStrings.Length , this.m_Options.Length ) ;
		int randomTarget = Random.Range( 0 , minSize ) ;
		ChangeTargetAnimation( randomTarget ) ; // button index
	}
	
	private void AnswerMode_EnterCorrectAnswer()
	{
		SwitchModeGUI( true ) ;
	}
	
	private void AnswerMode_ToOptionMode()
	{
		SwitchModeGUI( false ) ;
	}
	
	private void SwitchModeGUI( bool _AnswerMode )
	{
		if( null != grid )
		{
			NGUITools.SetActive( grid.gameObject , !_AnswerMode );
		}
		
		// hide AnswerMode button
		// show OptionMode button
		if( null != answerModeButton )
		{
			NGUITools.SetActive( answerModeButton , !_AnswerMode );
		}
		
		if( null != optionModeModeButton )
		{
			NGUITools.SetActive( optionModeModeButton , _AnswerMode );
		}
		
		if( null != arrowUpButton )
		{
			NGUITools.SetActive( arrowUpButton , _AnswerMode );
		}
		if( null != arrowDownButton )
		{
			NGUITools.SetActive( arrowDownButton , _AnswerMode );
		}
		
		if( null != answerLabel )
		{
			NGUITools.SetActive( answerLabel.gameObject , _AnswerMode );
		}
	}
	
	private void ChangeTargetAnimation( int _Index )
	{
		if( _Index < 0 || _Index >= m_Keys.Length )
		{
			return ;
		}
		// Debug.Log("ChangeTargetAnimation _Index" + _Index );
		
		m_TargetIndex = _Index ;
		NGUITools.SetActive( answerLabel.gameObject , false ) ;
		NGUITools.SetActiveChildren( answerLabel.gameObject , false ) ;
		this.IsInAnimation = true ;
	}
	
	private void InitializeReferencePlanes()
	{
		
		if( null == referenceObject )
		{
			return ;
		}	
		
		Object prefab = Resources.Load("Location/PlanePrefab");
		if( null == prefab )
		{
			return ;
		}
		
		string [] locationKey = 
		{ 
			"AlongRiver" 
			, "Bridge" 
			, "CrossStreet"
			, "Intersection"
			, "Pass"
			, "RoundAbout"
			, "Through"
		} ;
		string [] describeString = 
		{ 
			"<das Fussufer> entlang" 
			, "über <die Brücke>" 
			, "gegenüber <...>"
			, "bis zur <Kreuzung>"
			, "am <...> vorbei"
			, "um <den Kreisverkehr> herum"
			, "durch <das Zentrum> fahren"
		} ;
		
		m_Keys = new string[ locationKey.Length ] ;
		m_TargetPositions = new Vector3[ locationKey.Length ] ;
		m_AnswerStrings = new string[ locationKey.Length ] ;
		for( int i = 0 ; i < locationKey.Length ; ++i )
		{
			m_Keys[ i ] = locationKey[ i ] ;
			m_AnswerStrings[ i ] = describeString[ i ] ;
			m_TargetPositions[ i ] = new Vector3( 0 , -70 * i ) ;
			GameObject addObj = GameObject.Instantiate( prefab) as GameObject ;
			if( null != addObj )
			{
				addObj.transform.parent = referenceObject.transform ;
				
				addObj.name = i.ToString() ;
				addObj.transform.position = new Vector3( 0 , 70 * i , 0 ) ;
				addObj.transform.localRotation = Quaternion.Euler( 90 , 0 , 0 ) ;
				addObj.transform.localScale = new Vector3( 1 , 1 , 1 ) ;
				
				SpriteRenderer sr = addObj.GetComponent<SpriteRenderer>();
				if( null != sr )
				{
					string spriteName = "Location/" + m_Keys[i] ;
					// Debug.Log("spriteName=" + spriteName );
					sr.sprite = Resources.Load<Sprite>( spriteName );
				}
				
			}
		}
		
		
	}
	
	private void InitializeOptions()
	{
		
		if( null == grid )
		{
			return ;
		}	
		
		GameObject prefab = Resources.Load("Location/OptionPrefab") as GameObject;
		if( null == prefab )
		{
			return ;
		}
		
		m_Options = new UILabel[ m_Keys.Length ] ;
		for( int i = 0 ; i < m_Keys.Length ; ++i )
		{
			GameObject addObj = NGUITools.AddChild( grid.gameObject , prefab ) ;
			if( null != addObj )
			{
				addObj.name = i.ToString() ;
				
				PressOption po = addObj.AddComponent<PressOption>() ;
				if( null != po )
				{
					po.m_LocationSystem = this ;
					po.m_OptionIndex = i ;
				}
				m_Options[ i ] = addObj.GetComponent<UILabel>() ;
			}
		}
		
		if( null != grid )
		{
			grid.Reposition() ;
		}
		
	}
}
