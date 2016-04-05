using UnityEngine;
using System.Collections;

public class WhereRandomLoadScreen : MonoBehaviour 
{
	private string [] m_TextureNames = 
	{
		"ChristinaSturmer" ,
		"Kaptnpengdittentakel" ,
		"LadyGaga"
	} ;

	private float m_NextReloadTime = 0.0f ;
	private float m_ReloadSec = 5.0f ;
	private int m_PreviousIndex = 0 ;

	private Renderer m_Render = null ;
	
	// Use this for initialization
	void Start () 
	{
		m_NextReloadTime = Time.timeSinceLevelLoad + m_ReloadSec ;
		m_Render = this.GetComponent<Renderer>() ;
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
		int index = 0 ;
		index = Random.Range( 0 , m_TextureNames.Length ) ; ;
		while( index == m_PreviousIndex )
		{
			index = Random.Range( 0 , m_TextureNames.Length ) ; ;
		}
		m_PreviousIndex = index ;
		string filename = m_TextureNames[ m_PreviousIndex ] ;
		Texture2D tex = Resources.Load( "Where/TelevisionScreen/" + filename ) as Texture2D ;
		if( null == tex )
		{
			Debug.LogError("null == tex filename" + filename);
			return ;
		
		}

		if( null != m_Render && null != m_Render.material )
		{
			m_Render.material.mainTexture = tex ;
		}
		
		
	}
}


