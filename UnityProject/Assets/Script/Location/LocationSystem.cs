using UnityEngine;
using System.Collections;

public class LocationSystem : MonoBehaviour 
{
	public GameObject referenceObject = null ;
	public UILabel answerLabel = null ;
	
	private int m_TargetIndex = 0 ;
	private Vector3 [] m_TargetPositions = new Vector3[2] ;
	private string [] m_Answers = new string[2] ;
	
	public bool IsInAnimation { get ; set ; }
	
	public float m_MovingSpeed = 0.1f ;
	
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
		
		IsInAnimation = false ;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
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
}
