using UnityEngine;
using System.Collections;

public enum AnswerMode
{
	AnswerMode_Invalid = 0 ,
	AnswerMode_ChangeToAnswerMode ,
	AnswerMode_WaitAtAnswerMode ,
	AnswerMode_ChangeToOptionMode ,
	AnswerMode_WaitAnimation ,
	AnswerMode_WaitPressOption ,
	AnswerMode_WaitCorrectAnimation ,
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
	public GameObject exampleButton = null ;
	public GameObject instructionText = null ;
	public UILabel exampleContent = null ;
	public TweenAlpha correctAlpha = null ;
	public AudioSource correctAudio = null ;
	
	private int m_TargetIndex = 0 ;
	private Vector3 [] m_TargetPositions = new Vector3[2] ;
	private string [] m_AnswerStrings = null ;
	private string [] m_Keys = null ;
	
	public bool IsInAnimation { get ; set ; }
	
	public float m_MovingSpeed = 0.1f ;
	
	public UILabel [] m_Options = null ;
	
	public AnswerMode m_AnswerMode = AnswerMode.AnswerMode_Invalid ;
	
	public float m_CorrectAnswerWaitSec = 0.5f ;
	public float m_CorrectAnswerWaitTime = 0.0f ;
	
	private string GetDescribKey( int _Index )
	{
		return "LocationKey_" + m_Keys[ _Index ] ;
	}
	
	private string GetExampleKey( int _Index )
	{
		return "ExampleSentence_" + m_Keys[ _Index ] ;
	}
	
	string [] m_DefaultLocationKey = 
	{ 
		"Straight"
		, "AlongRiver" 
		, "Bridge" 
		, "CrossStreet"
		, "Pass"
		, "RoundAbout"
		, "Intersection"
		, "TurnLeft"
		, "TurnRight"
		, "ThroughInterception"
		, "Through"
		
	} ;
	
	
	int [] m_RemapTable = null ;
	
	float m_ShowExampleWaitTime = 10.0f ;
	float m_ShowExampleSet = 0.0f ;
	
	public void ResetDescribeString()
	{
		string describKey = "" ;
		for( int i = 0 ; i < m_Keys.Length ; ++i )
		{
			describKey = GetDescribKey( i ) ;
			m_AnswerStrings[ i ] = Localization.Get( describKey ) ;	
		}
		
		ResetOptionsText() ;
		
		answerLabel.text = m_AnswerStrings[ m_TargetIndex ] ;
	}
	
	public void TrySwitchToAnswerMode()
	{
		if( m_AnswerMode != AnswerMode.AnswerMode_WaitPressOption )
		{
			return ;
		}
		m_AnswerMode = AnswerMode.AnswerMode_ChangeToAnswerMode ;
	}
	
	public void TrySwitchToOptionMode()
	{
		if( m_AnswerMode != AnswerMode.AnswerMode_WaitAtAnswerMode )
		{
			return ;
		}
		m_AnswerMode = AnswerMode.AnswerMode_ChangeToOptionMode ;
	}
	
	
	public void TryPress( int _OptionIndex )
	{
		// Debug.Log("TryPress" + _OptionIndex ) ;
		
		int pressAnserIndex = m_RemapTable[ _OptionIndex ] ;
		if( m_TargetIndex == pressAnserIndex )
		{
			// turn option to green
			PlayCorrectAnimation( true ) ;
			m_CorrectAnswerWaitTime = Time.timeSinceLevelLoad + m_CorrectAnswerWaitSec ;
			m_AnswerMode = AnswerMode.AnswerMode_WaitCorrectAnimation ;
		}
		else
		{
			// turn option to red
			TweenColor tween = m_Options[ _OptionIndex ].gameObject.GetComponent<TweenColor>() ;
			if( null != tween )
			{
				tween.PlayForward() ;
			}
		}
	}
	
	public void TryMoveUp()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		
		// actually we move reference up 70
		ChangeTargetAnimation( m_TargetIndex - 1 ) ;
		
		HideInstructionText() ;

	}
	public void TryMoveDown()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		
		ChangeTargetAnimation( m_TargetIndex + 1 ) ;
		
		HideInstructionText() ;
	}
	

	// Use this for initialization
	void Start () 
	{
		InitializeReferencePlanes() ;
		InitializeOptions() ;
		CheckIndexGUI( m_TargetIndex ) ;
		IsInAnimation = true ;
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch( m_AnswerMode )
		{
		case AnswerMode.AnswerMode_Invalid :
			m_AnswerMode = AnswerMode.AnswerMode_ChangeToAnswerMode ;
			break ;
		case AnswerMode.AnswerMode_ChangeToAnswerMode :
			AnswerMode_ChangeToAnswerMode() ;
			m_AnswerMode = AnswerMode.AnswerMode_WaitAtAnswerMode ;
			break ;
		case AnswerMode.AnswerMode_WaitAtAnswerMode :
			if( true == this.IsInAnimation )
			{
				if( true == UpdateReference() )
				{
					NGUITools.SetActive( answerLabel.gameObject , true ) ;
					NGUITools.SetActiveChildren( answerLabel.gameObject , true ) ;
					ResetExampleTimer() ;
				}
			}
			else
			{
				CheckExampleTimer() ;
			}
			break ;
		case AnswerMode.AnswerMode_ChangeToOptionMode :
			RandomizeTheOptions() ;
			AnswerMode_ChangeToOptionMode() ;
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
		case AnswerMode.AnswerMode_WaitCorrectAnimation :
			AnswerMode_WaitCorrectAnimation() ;
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
			string exampleKey = GetExampleKey( m_TargetIndex ) ;
			string exampleSentence = Localization.Get( exampleKey );
			Debug.Log("exampleSentence" + exampleSentence);
			UpdateExampleContent( exampleSentence ) ;
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
		if( null == m_RemapTable )
		{
			return ;
		}
		
		for( int i = 0 ; i < 10 ; ++i )
		{
			int index0 = Random.Range( 0 , m_RemapTable.Length ) ;
			int index1 = Random.Range( 0 , m_RemapTable.Length ) ;
			int tmp = m_RemapTable[ index0 ] ;
			m_RemapTable[ index0 ] = m_RemapTable[ index1 ] ;
			m_RemapTable[ index1 ] = tmp ;
		}
		
		
		ResetOptionsText() ;
		
		
		int minSize = Mathf.Min( m_AnswerStrings.Length , this.m_Options.Length ) ;
		int randomTarget = m_TargetIndex ;
		while( randomTarget == m_TargetIndex )
		{
			randomTarget = Random.Range( 0 , minSize ) ;
		}
		
		ChangeTargetAnimation( randomTarget ) ; // button index
	}
	
	private void ResetOptionsText()	
	{
		if( null == this.m_Options )
		{
			return ;
		}
		
		for( int i = 0 ; i < this.m_Options.Length ; ++i )
		{
			if( null == m_Options[ i ] )
			{
				return ;
			}
			
			if( i < m_AnswerStrings.Length )
			{
				m_Options[ i ].text = m_AnswerStrings[ m_RemapTable[ i ] ] ;
				TweenColor tween = m_Options[ i ].gameObject.GetComponent<TweenColor>() ;
				if( null != tween )
				{
					tween.PlayReverse() ;
				}				
				// Debug.Log("button i=" + i);
				// Debug.Log("m_RemapTable[ i ] =" + m_RemapTable[ i ]);
				// Debug.Log("m_Options[ i ].text" + m_Options[ i ].text);
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
		
	}
	
	private void AnswerMode_ChangeToAnswerMode()
	{
		SwitchModeGUI( true ) ;
	}
	
	private void AnswerMode_ChangeToOptionMode()
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
			NGUITools.SetActive( arrowUpButton , _AnswerMode && m_TargetIndex > 0 );
		}
		if( null != arrowDownButton )
		{
			NGUITools.SetActive( arrowDownButton , _AnswerMode && m_TargetIndex < m_Keys.Length - 1 );
		}
		
		if( null != answerLabel )
		{
			NGUITools.SetActive( answerLabel.gameObject , _AnswerMode );
		}
	}
	
	private void CheckIndexGUI( int _Index )
	{
		NGUITools.SetActive( arrowUpButton , _Index > 0  ) ;
		NGUITools.SetActive( arrowDownButton , _Index < m_Keys.Length - 1 ) ;
	}
	
	private void ChangeTargetAnimation( int _Index )
	{
		CheckIndexGUI( _Index ) ;
		if( _Index < 0 )
		{
			return ;
		}
		
		if( _Index >= m_Keys.Length )
		{
			return ;
		}
		// Debug.Log("ChangeTargetAnimation _Index" + _Index );
		
		m_TargetIndex = _Index ;
		NGUITools.SetActive( answerLabel.gameObject , false ) ;
		NGUITools.SetActiveChildren( answerLabel.gameObject , false ) ;
		ShowExampleButton( false ) ;
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
		
		
		m_Keys = new string[ m_DefaultLocationKey.Length ] ;
		for( int i = 0 ; i < m_DefaultLocationKey.Length ; ++i )
		{
			m_Keys[ i ] = m_DefaultLocationKey[ i ] ;
		}				

				
		m_TargetPositions = new Vector3[ m_DefaultLocationKey.Length ] ;
		m_AnswerStrings = new string[ m_Keys.Length ] ;
		
		m_RemapTable = new int[ m_Keys.Length ] ;
		for( int i = 0 ; i < m_RemapTable.Length ; ++i )
		{
			m_RemapTable[ i ] = i ;
		}
		
		ResetDescribeString() ;
		
		for( int i = 0 ; i < m_Keys.Length ; ++i )
		{
			m_TargetPositions[ i ] = new Vector3( 0 , -70 * i ) ;
			GameObject addObj = GameObject.Instantiate( prefab) as GameObject ;
			if( null != addObj )
			{
				addObj.transform.parent = referenceObject.transform ;
				
				addObj.name = i.ToString() ;
				addObj.transform.position = new Vector3( 0 , 70 * i , 0 ) ;
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
	
	private void ShowExampleButton( bool _Show )
	{
		if( null == exampleButton )
		{
			return ;
		}
		
		NGUITools.SetActive( exampleButton , _Show ) ;
	}
	
	private void HideInstructionText()
	{
		if( null == instructionText )
		{
			return ;
		}
		
		NGUITools.SetActive( instructionText , false ) ;	
	}
	
	private void UpdateExampleContent( string _Content )
	{
		if( null == exampleContent )
		{
			return ;
		}
		
		exampleContent.text = _Content ; 
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
	
	private void PlayCorrectAnimation( bool _Forward )
	{
		if( null == correctAlpha )
		{
			return ;
		}
		
		if( true == _Forward )
		{
			correctAlpha.PlayForward() ;
			correctAudio.Play() ;
		}
		else
		{
			correctAlpha.PlayReverse() ;
		}
	}
	
	private void AnswerMode_WaitCorrectAnimation()
	{
		if( Time.timeSinceLevelLoad > m_CorrectAnswerWaitTime )
		{
			m_AnswerMode = AnswerMode.AnswerMode_ChangeToOptionMode ;		
			PlayCorrectAnimation( false ) ;
		}
		
	}
}
