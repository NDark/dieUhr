using UnityEngine;
using System.Collections;

public class TweemPingPongAfterSec : MonoBehaviour 
{
	TweenAlpha tween = null ;
	float waitingSec = 4.0f ;
	float waitingTime = 0.0f ;
	bool isForward = true ;
	
	public void OnFinished()
	{
		if( isForward )
		{
			isForward = false ;
			if( null == tween )
			{
				tween = this.GetComponent<TweenAlpha>() ;
			}
			
			tween.PlayReverse() ;
					
			waitingTime = Time.timeSinceLevelLoad + waitingSec ;
		}
		
	}
	
	// Use this for initialization
	void Start () {
		
		if( null == tween )
		{
			tween = this.GetComponent<TweenAlpha>() ;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( false == isForward 
		&& Time.timeSinceLevelLoad > waitingTime )
		{
			tween.PlayForward() ;
			isForward = true ;
		}
	}
}
