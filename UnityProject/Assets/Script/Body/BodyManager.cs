using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
	public UILabel m_AnswerLabel = null;
	public UILabel m_ExampleContent = null;
	public GameObject m_AvatarGameObj = null;
	public GameObject m_NailObj = null;

	public GameObject m_ShowBodyPartModeButton = null;
	public GameObject m_QuestionModeButton = null;
	public GameObject m_ExampleButton = null;

	public UILabel m_InstructionText = null;
	public TweenAlpha m_CorrectAlpha = null;
	public AudioSource m_CorrectAudio = null;

	public Camera m_2DCamera = null;
	public Camera m_3DCamera = null;

	public BodyInputHelper m_MoveModeTouchRegion = null;

	public TweenPosition m_NejiTweenPos = null ;
	public TweenAlpha m_NejiTweenAlpha = null;
	public AudioSource m_NejiAudio = null;

	public enum BodyType : int
	{
		Head = 0 ,
		Neck,
		Chest,
		Belly,
		Shoulder,
		Arm,
		Hand,
		Leg,
		Foot,
		Max,
	} ;

	public enum BodyState
	{
		None = 0,
		Initialize,
		ShowPartMode_Init,
		ShowPartMode,
		QuestionMode_Init,
		QuestionMode,
		WaitCorrectAnimation,
	}

	string m_CurrentSelectPart = string.Empty;
	string m_CurrentAnswerKey = string.Empty;
	public BodyState m_State = BodyState.None;

	float m_ShowExampleWaitTime = 3.0f;
	float m_ShowExampleCheckTime = 0.0f;

	public void TrySwitchToQuestionMode()
	{
		if ( m_State == BodyState.ShowPartMode )
		{
			SetupGUI(isShowBodyPartMode: false);
			m_State = BodyState.QuestionMode_Init;
		}

	}

	public void TrySwitchToShowPartMode()
	{
		if (m_State == BodyState.QuestionMode)
		{
			SetupGUI(isShowBodyPartMode: true);
			m_State = BodyState.ShowPartMode_Init;
			return;
		}
	}

	void DetectUserClick_ShowPartMode()
	{
		// detect user part
		m_CurrentAnswerKey = string.Empty;

		var ray = m_3DCamera.ScreenPointToRay(Input.mousePosition);
		var hits = Physics.RaycastAll(ray);
		float minDist = float.MaxValue ;
		Collider minDistCollider = null ;
		BodyVibrationHelper vibrationTarget = null ;
		Vector3 minDistPos = Vector3.zero;
		foreach(var hit in hits )
		{ 
			
			var tempdist = hit.distance ;
			if( tempdist < minDist)
			{ 
				minDist = tempdist;
				m_CurrentAnswerKey = hit.collider.name ;
				minDistPos = hit.collider.transform.position;
				minDistCollider = hit.collider;
			}
		}

		// Debug.Log("m_CurrentAnswerKey=" + m_CurrentAnswerKey);
		// show part name from m_CurrentSelectPart
		// m_CurrentAnswerKey
		bool hasClickOnValidPart = string.Empty != m_CurrentAnswerKey;
		this.ResetAnswerContent();
		if (hasClickOnValidPart)
		{
			m_CorrectAudio.Play();
			BodyVibrationHelper helper = minDistCollider.GetComponent<BodyVibrationHelper>();
			if(null!= helper)
			{
				vibrationTarget = helper ;
			}
			StartCoroutine(ActiveNeji());
		}
		else
		{
		}
		
		ShowNailObj(hasClickOnValidPart , minDistPos , vibrationTarget );

		this.ResetAnswerContent();
		this.ResetExampleContent();

		NGUITools.SetActive(m_InstructionText.gameObject, !hasClickOnValidPart);

	}

	void DetectUserClick_QuestionMode()
	{
		// detect user part
		m_CurrentSelectPart = string.Empty;

		var ray = m_3DCamera.ScreenPointToRay(Input.mousePosition);
		var hits = Physics.RaycastAll(ray);
		float minDist = float.MaxValue;
		Vector3 minDistPos = Vector3.zero;
		Collider minDistCollider = null;
		BodyVibrationHelper vibrationTarget = null;
		foreach (var hit in hits)
		{

			var tempdist = hit.distance;
			if (tempdist < minDist)
			{
				minDist = tempdist;
				m_CurrentSelectPart = hit.collider.name;
				minDistPos = hit.collider.transform.position;
				minDistCollider = hit.collider;
			}
		}

		// Debug.Log("m_CurrentAnswerKey=" + m_CurrentAnswerKey);
		// show part name from m_CurrentSelectPart
		// m_CurrentAnswerKey
		bool hasClickOnValidPart = m_CurrentAnswerKey == m_CurrentSelectPart;
		if (hasClickOnValidPart)
		{
			PlayCorrectAnimation(true);
			m_CorrectAnswerWaitCheckTime = Time.timeSinceLevelLoad + m_CorrectAnswerWaitSec;
			m_State = BodyState.WaitCorrectAnimation;
			BodyVibrationHelper helper = minDistCollider.GetComponent<BodyVibrationHelper>();
			if (null != helper)
			{
				vibrationTarget = helper;
			}
			StartCoroutine(ActiveNeji());
		}
		else
		{
		}
		
		ShowNailObj(hasClickOnValidPart, minDistPos, vibrationTarget);
		

	}

	public void OnUserClick()
	{
		NGUITools.SetActive(this.m_InstructionText.gameObject, false);

		if (BodyState.ShowPartMode != m_State
			&& BodyState.QuestionMode != m_State
			)
		{
			Debug.LogError("OnUserClick() invalid state.");
			return;
		}

		switch( m_State)
		{ 
			case BodyState.ShowPartMode :
				{ 
					this.DetectUserClick_ShowPartMode();
				}
				break;
			case BodyState.QuestionMode:
				this.DetectUserClick_QuestionMode();

				break;
		}

	}



	// Start is called before the first frame update
	void Start()
    {
        
    }

	void Awake()
	{
		BodyInputHelper.s_System = this; 
	}

    // Update is called once per frame
    void Update()
    {
		switch (m_State)
		{
			case BodyState.None:
				m_State = BodyState.Initialize;
				break;
			case BodyState.Initialize:
				Initialize();
				m_State = BodyState.ShowPartMode_Init;
				break;

			case BodyState.ShowPartMode_Init:
				m_CurrentAnswerKey = string.Empty ;
				NGUITools.SetActive(m_InstructionText.gameObject, true);
				ResetAnswerContent();
				ShowExampleButton(false);// no example until key is click
				m_State = BodyState.ShowPartMode;
				break;
			case BodyState.ShowPartMode:
				if(!string.IsNullOrEmpty(m_CurrentAnswerKey))
				{
					CheckExampleTimer();
				}

				break;

			case BodyState.QuestionMode_Init:

				this.RandomizeTopic();
				this.ResetExampleContent();
				this.ShowExampleButton(true);
				this.ResetAnswerContent();
				m_State = BodyState.QuestionMode;
				break;
			case BodyState.QuestionMode:
				CheckExampleTimer();
				break;
			case BodyState.WaitCorrectAnimation:
				WaitCorrectAnimation();
				break;
		}

		UpdateNailObj();
	}

	void Initialize()
	{
		SetupGUI( isShowBodyPartMode : true);
	}

	void SetupGUI( bool isShowBodyPartMode )
	{
		NGUITools.SetActive(m_ShowBodyPartModeButton, !isShowBodyPartMode);
		NGUITools.SetActive(m_QuestionModeButton, isShowBodyPartMode);
	}

	public void ResetAnswerContent()
	{
		switch(m_State)
		{
			case BodyState.ShowPartMode_Init:
			case BodyState.ShowPartMode:
				if(string.IsNullOrEmpty(m_CurrentAnswerKey))
				{
					m_AnswerLabel.text = string.Empty;
				}
				else
				{
					m_AnswerLabel.text = Localization.Get("Body." + m_CurrentAnswerKey);
				}
				break ;
			case BodyState.QuestionMode_Init:
			case BodyState.QuestionMode:
				string fmt = Localization.Get("Body.QuestionOnAnswerFmt");
				string answerText = string.Format(fmt , Localization.Get("Body." + m_CurrentAnswerKey) );
				m_AnswerLabel.text = answerText;
				break;

		}
	}


	public void ResetInstructionText()
	{
		m_InstructionText.text = Localization.Get("Introduction.Body");
	}

	public void ResetExampleContent()
	{
		if(!string.IsNullOrEmpty(m_CurrentAnswerKey))
		{
			string exampleKey = GetExampleKey(m_CurrentAnswerKey);
			string exampleSentence = Localization.Get(exampleKey);
			UpdateExampleContent(exampleSentence);
			ShowExampleButton(true);
		}
		else
		{
			ShowExampleButton(false);
		}
		

	}

	private string GetExampleKey(string _WhereKey)
	{
		return "BodyExample." + _WhereKey;
	}


	private void UpdateExampleContent(string _Content)
	{
		if (null == m_ExampleContent)
		{
			return;
		}

		m_ExampleContent.text = _Content;
	}

	private void ResetExampleTimer()
	{
		float waitTime = Random.Range(m_ShowExampleWaitTime / 2.0f, m_ShowExampleWaitTime);
		m_ShowExampleCheckTime = Time.timeSinceLevelLoad + waitTime;
	}

	private void CheckExampleTimer()
	{
		if (Time.timeSinceLevelLoad > m_ShowExampleCheckTime)
		{
			ShowExampleButton(true);
		}
	}

	private void ShowExampleButton(bool _Show)
	{
		if (null == this.m_ExampleButton)
		{
			return;
		}

		NGUITools.SetActive(m_ExampleButton, _Show);
	}

	void RandomizeTopic()
	{
		int randomIndex = Random.Range(0, (int) BodyType.Max );
		m_CurrentAnswerKey = ((BodyType) randomIndex ).ToString();
		// Debug.LogWarning ("RandomizeTopic() m_CurrentAnswerKey=" + m_CurrentAnswerKey);
		ShowExampleButton(false);
	}


	public string CreateInstruction(string _TargetKey
							   , string _SceneKey
							   , string _WhereKey
							   , string _ReferenceKey)
	{

		//		Debug.Log("CreateInstruction()" + _TargetKey );
		//		Debug.Log("CreateInstruction()" + _SceneKey );
		//		Debug.Log("CreateInstruction()" + _WhereKey );
		//		
		string localizationWhereKey = "WhereInstruction_" + _WhereKey;
		string localWhereString = Localization.Get(localizationWhereKey);
		/*
		string targetString = Localization.Get("WhereTarget_" + _TargetKey);
		targetString = AkkusativTheNoun(targetString);
		string sceneString = Localization.Get("WhereScene_" + _SceneKey);
		sceneString = AkkusativTheNoun(sceneString);

		string referenceString = string.Empty;
		if (string.Empty != _ReferenceKey)
		{
			referenceString = Localization.Get("WhereScene_" + _ReferenceKey);
			referenceString = AkkusativTheNoun(referenceString);
		}


		localWhereString = localWhereString.Replace("<target>", targetString);
		localWhereString = localWhereString.Replace("<scene>", sceneString);
		localWhereString = localWhereString.Replace("<reference>", referenceString);
		localWhereString = ReplaceDativShort(localWhereString);
		localWhereString = ReplaceFirstUpperCase(localWhereString);
		//*/
		return localWhereString;
	}

	private void PlayCorrectAnimation(bool _Forward)
	{
		if (null == m_CorrectAlpha)
		{
			return;
		}

		if (true == _Forward)
		{
			m_CorrectAlpha.PlayForward();
			m_CorrectAudio.Play();
		}
		else
		{
			m_CorrectAlpha.PlayReverse();
		}
	}

	private void WaitCorrectAnimation()
	{
		
		if (Time.timeSinceLevelLoad > m_CorrectAnswerWaitCheckTime)
		{
			// ReleaveScene(m_CurrentScene, m_Fussball);
			PlayCorrectAnimation(false);
			m_State = BodyState.QuestionMode_Init;// go back to question mode
		}
		
	}

	void ShowNailObj( bool show , Vector3 pos , BodyVibrationHelper helper )
	{
		m_NailObj.SetActive(show);
		m_NailTargetPos = pos ;
		m_NailVibrationHelper = helper;
		pos.z += 3;
		m_NailObj.transform.position = pos;

		if(show)
		{
			m_NailIsMoving = true;
		}
		
	}

	void UpdateNailObj()
	{ 
		if(m_NailIsMoving)
		{
			m_NailObj.transform.position = Vector3.MoveTowards(m_NailObj.transform.position, m_NailTargetPos, Time.deltaTime * m_NailSpeed );

			if( Vector3.Distance(m_NailObj.transform.position , m_NailTargetPos) < 0.1f )
			{ 
				m_NailIsMoving = false ;
				m_NailVibrationHelper.ActiveVibration();
			}
		}
	}

	IEnumerator ActiveNeji()
	{
		m_NejiAudio.Play();
		m_NejiTweenPos.ResetToBeginning() ;
		m_NejiTweenAlpha.ResetToBeginning();
		yield return new WaitForSeconds(1);
		m_NejiTweenPos.PlayForward();
		m_NejiTweenAlpha.PlayForward();

	}
float m_NailSpeed = 10f ;
	bool m_NailIsMoving = false ;
	Vector3 m_NailTargetPos = Vector3.zero;
	BodyVibrationHelper m_NailVibrationHelper = null ;

	private float m_CorrectAnswerWaitSec = 1.0f;
	private float m_CorrectAnswerWaitCheckTime = 0.0f;
}
