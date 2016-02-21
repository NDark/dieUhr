using UnityEngine;
using System.Collections;

public class LocationSystem : MonoBehaviour 
{
	public GameObject referenceObject = null ;
	
	private Vector3 m_TargetPos = Vector3.zero ;
	public bool IsInAnimation { get ; set ; }
	
	public float m_MovingSpeed = 0.1f ;
	
	public void TryMoveUp()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		// actually we move reference up 70
		m_TargetPos = new Vector3( 0 , 0 ) ;
		this.IsInAnimation = true ;
	}
	public void TryMoveDown()
	{
		if( true == this.IsInAnimation )
		{
			return ;
		}
		m_TargetPos = new Vector3( 0 , -70 ) ;
		this.IsInAnimation = true ;
	}
	

	// Use this for initialization
	void Start () {
	
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
		if( null == referenceObject )
		{
			return ;
		}
		
		Vector3 currentPos = referenceObject.transform.position ;
		float dist = Vector3.Distance( currentPos , m_TargetPos ) ;
		if( dist <= 0.5f )
		{
			referenceObject.transform.position = m_TargetPos ;
			this.IsInAnimation = false ;
		}
		else
		{
			Vector3 nextPos = Vector3.Lerp( currentPos , m_TargetPos 
			                               , m_MovingSpeed ) ;
			referenceObject.transform.position = nextPos;
		}
	}
}
