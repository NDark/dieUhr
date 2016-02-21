using UnityEngine;
using System.Collections;

public enum AnswerMode
{
	AnswerMode_Invalid = 0 ,
	AnswerMode_EnterCorrectAnswer ,
	AnswerMode_WaitCorrectAnswer ,
	AnswerMode_ToOptionMode ,
	AnswerMode_RandomizeAOption ,
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
	private string [] m_Answers = new string[2] ;
	
	public bool IsInAnimation { get ; set ; }
	
	public float m_MovingSpeed = 0.1f ;
	
	public UILabel [] m_Options = new UILabel[2] ;
	
	
	AnswerMode m_AnswerMode = AnswerMode.AnswerMode_Invalid ;
	
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
		m_TargetIndex = 0 ;
		NGUITools.SetActive( answerLabel.gameObject , false ) ;
		NGUITools.SetActiveChildren( answerLabel.gameObject , false ) ;
		this.IsInAnimation = true ;
	}
	public void TryMoveDown()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		m_TargetIndex = 1 ;
		NGUITools.SetActive( answerLabel.gameObject , false ) ;
		NGUITools.SetActiveChildren( answerLabel.gameObject , false ) ;
		this.IsInAnimation = true ;
	}
	

	// Use this for initialization
	void Start () 
	{
		m_TargetPositions[ 0 ] = new Vector3( 0 , -70 ) ;
		m_Answers[ 0 ] = "Bridge" ;
		m_TargetPositions[ 1 ] = new Vector3( 0 , 0 ) ;
		m_Answers[ 1 ] = "Roundabout" ;
		
		RandomizeTheOptions() ;
		IsInAnimation = false ;
		
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
			break ;
		case AnswerMode.AnswerMode_ToOptionMode :
			AnswerMode_ToOptionMode() ;
			m_AnswerMode = AnswerMode.AnswerMode_WaitPressOption ;
			break ;
		case AnswerMode.AnswerMode_RandomizeAOption :
			break ;
		case AnswerMode.AnswerMode_WaitAnimation :
			break ;
		case AnswerMode.AnswerMode_WaitPressOption :
			break ;
			
		}
		
		if( true == this.IsInAnimation )
		{
			UpdateReference() ;
		}
	
	}
	
	private void UpdateReference()
	{
		if( null == referenceObject || null == answerLabel )
		{
			return ;
		}
		
		Vector3 currentPos = referenceObject.transform.position ;
		Vector3 targetPos = m_TargetPositions [ m_TargetIndex ] ;
		float dist = Vector3.Distance( currentPos , targetPos ) ;
		if( dist <= 0.5f )
		{
			referenceObject.transform.position = targetPos ;
			answerLabel.text = m_Answers[ m_TargetIndex ] ;
			NGUITools.SetActive( answerLabel.gameObject , true ) ;
			NGUITools.SetActiveChildren( answerLabel.gameObject , true ) ;
			this.IsInAnimation = false ;
		}
		else
		{
			Vector3 nextPos = Vector3.Lerp( currentPos , targetPos 
			                               , m_MovingSpeed ) ;
			referenceObject.transform.position = nextPos;
		}
	}
	
	private void RandomizeTheOptions()
	{
		
		int []remapTable = new int[ m_Answers.Length ] ;
		for( int i = 0 ; i < remapTable.Length ; ++i )
		{
			remapTable[ i ] = i ;
		}
		for( int i = 0 ; i < 10 ; ++i )
		{
			int index0 = Random.Range( 0 , 2 ) ;
			int index1 = Random.Range( 0 , 2 ) ;
			int tmp = remapTable[ index0 ] ;
			remapTable[ index0 ] = remapTable[ index1 ] ;
			remapTable[ index1 ] = tmp ;
		}
		
		
		for( int i = 0 ; i < this.m_Options.Length ; ++i )
		{
			if( i < m_Answers.Length )
			{
				m_Options[ i ].text = m_Answers[ remapTable[ i ] ] ;
			}
			else
			{
				// hide the options
			}
		}
		
		if( null != grid )
		{
			grid.Reposition() ;
		}
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
	
}
