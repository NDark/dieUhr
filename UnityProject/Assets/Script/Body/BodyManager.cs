using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
	public UILabel m_AnswerLabel = null;
	public UILabel m_ExampleContent = null;

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

}
