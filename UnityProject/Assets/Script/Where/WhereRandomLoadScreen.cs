using UnityEngine;
using System.Collections;

public class WhereRandomLoadScreen : MonoBehaviour 
{
	public string [] m_TextureNames = 
	{
		"ChristinaStürmer" ,
		"KÄPTNPENGDIETENTAKEL" ,
		"LadyGaga"
	} ;

	private float m_NextReloadTime = 0.0f ;
	private float m_ReloadSec = 20.0f ;

	// Use this for initialization
	void Start () 
	{
		m_NextReloadTime = Time.timeSinceLevelLoad + m_ReloadSec ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Time.timeSinceLevelLoad > m_NextReloadTime )
		{
			ReloadTexture() ;
			m_NextReloadTime = Time.timeSinceLevelLoad + m_ReloadSec ;
		}
	
	}
	
	private void ReloadTexture()
	{
		int index = Random.Range( 0 , m_TextureNames.Length ) ;
		string filename = m_TextureNames[ index ] ;
		Texture2D tex = Resources.Load( "Where/TelevisionScreen/" + filename ) as Texture2D ;
		if( null != tex )
		{
			Renderer render = this.GetComponent<Renderer>() ;
			if( null != render && null != render.material )
			{
				
				render.material.mainTexture = tex ;
			}
		}
	}
}


