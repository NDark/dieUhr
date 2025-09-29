using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
	public UILabel m_AnswerLabel = null;
	public UILabel m_ExampleContent = null;
	public GameObject m_AvatarGameObj = null;

	public GameObject m_ShowBodyPartModeButton = null;
	public GameObject m_QuestionModeButton = null;
	public GameObject m_ExampleButton = null;

	public GameObject m_InstructionObj = null;
	public TweenAlpha m_CorrectAlpha = null;
	public AudioSource m_CorrectAudio = null;

	public Camera m_2DCamera = null;
	public Camera m_3DCamera = null;

	public BodyInputHelper m_MoveModeTouchRegion = null;

	public enum BodyType
	{
		Head,
		Neck,
		Chest,
		Belly,
		Shoulder,
		Arm,
		Hand,
		Leg,
		Foot,
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

	string m_CurrentBodyKey = string.Empty;
	public BodyState m_State = BodyState.None;

	float m_ShowExampleWaitTime = 3.0f;
	float m_ShowExampleCheckTime = 0.0f;

	public void TrySwitchToQuestionMode()
	{
		/*if (WhereState.WhereState_WaitInAnswerMode != m_State
		   && WhereState.WhereState_WaitInTeacherMode != m_State)
		{
			Debug.LogWarning("TrySwitchToMoveMode() invalid state.");
			return;
		}

		ReleaveScene(m_CurrentScene, m_Fussball);

		SwitchGUI(false);
		//*/
		SetupGUI(isShowBodyPartMode: false);
		m_State = BodyState.QuestionMode_Init;

	}

	public void TrySwitchToShowPartMode()
	{
		/*
		if (WhereState.WhereState_WaitInMoveMode != m_State
		   && WhereState.WhereState_WaitInTeacherMode != m_State)
		{
			Debug.LogWarning("TrySwitchToAnswerMode() invalid state.");
			return;
		}


		ReleaveScene(m_CurrentScene, m_Fussball);


		this.transform.rotation = Quaternion.identity;
		SwitchGUI(true);
		//*/
		SetupGUI(isShowBodyPartMode: true);
		m_State = BodyState.ShowPartMode_Init ;
	}



	public void OnUserClick()
	{
		Debug.LogWarning("OnUserClick");
		/*
		if (m_State != WhereState.WhereState_WaitInMoveMode)
		{
			Debug.LogError("DetectUserMouse() invalid state.");
			return;
		}

		if (m_CurrentWhereKey == m_CurrentSelectKey)
		{
			PlayCorrectAnimation(true);
			m_CorrectAnswerWaitTime = Time.timeSinceLevelLoad + m_CorrectAnswerWaitSec;
			m_State = WhereState.WhereState_WaitCorrectAnimation;
		}
		//*/
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
				m_State = BodyState.ShowPartMode;
				break;
			case BodyState.ShowPartMode:
				CheckExampleTimer();
				break;

			case BodyState.QuestionMode_Init:
				m_State = BodyState.QuestionMode;
				break;
			case BodyState.QuestionMode:
				CheckExampleTimer();
				break;
			case BodyState.WaitCorrectAnimation:
				WaitCorrectAnimation();
				break;

		}
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
		Debug.LogError("ResetAnswerContent");
		/*
		if (m_State == WhereState.WhereState_WaitInAnswerMode
		   || m_State == WhereState.WhereState_EnterAnswerMode
		   || m_State == WhereState.WhereState_EnterTeacherMode
		   || m_State == WhereState.WhereState_WaitInTeacherMode
		   )
		{
			m_AnswerLabel.text = CreateAnswer(m_TargetKey[0], m_CurrentSceneKey, m_CurrentWhereKey, this.m_CurrentReferenceKey);
		}
		else if (m_State == WhereState.WhereState_WaitInMoveMode
				|| m_State == WhereState.WhereState_EnterMoveMode)
		{
			m_AnswerLabel.text = CreateInstruction(m_TargetKey[0], m_CurrentSceneKey, m_CurrentWhereKey, this.m_CurrentReferenceKey);
		}
		//*/
	}

	public void ResetExampleContent()
	{
		Debug.LogError("ResetExampleContent");
		/*
		string exampleKey = GetExampleKey(m_CurrentWhereKey);
		string exampleSentence = Localization.Get(exampleKey);
		UpdateExampleContent(exampleSentence);
		//*/
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

	void RandonmizeTopic(GameObject _CurrentScene)
	{
		/*
		List<string> validWhereKey
		= CollectValidWhereFromSceneObject(_CurrentScene, true);

		if (0 == validWhereKey.Count)
		{
			Debug.LogError("0 == validWhereKey");
			return;
		}

		int randomIndex = Random.Range(0, validWhereKey.Count);
		m_CurrentWhereKey = validWhereKey[randomIndex];
		// Debug.LogWarning ("RandonmizeWhere() m_CurrentWhereKey=" + m_CurrentWhereKey);

		CheckWhereIsReference(_CurrentScene);

		//*/
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
		
		if (Time.timeSinceLevelLoad > m_AnswerWaitCheckTime)
		{
			// ReleaveScene(m_CurrentScene, m_Fussball);
			PlayCorrectAnimation(false);
			m_State = BodyState.QuestionMode_Init;
		}
		
	}

	private float m_CorrectAnswerWaitSec = 1.0f;
	private float m_AnswerWaitCheckTime = 0.0f;
}
