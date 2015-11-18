using UnityEngine;
using System.Collections;

public static class ClockData
{
    static UILabel m_Label = null;
    static int m_Minute = 0;
    static int m_Hour = 0;
    public static void SetupLabel(UILabel _Label)
    {
        m_Label = _Label;
    }
    public static void DoCalculateString(string _Key, int value)
    {
        if ("Minute" == _Key)
        {
            m_Minute = value/6;
        }
        else if ("Hour" == _Key)
        {
            m_Hour = value/30;
        }
        CalculateString();
    }

    static void CalculateString()
    {
        Debug.Log("m_Minute" + m_Minute);
        Debug.Log("m_Hour"+ m_Hour);
        if (null == m_Label)
        {
            return;
        }

        m_Label.text = "CalculateString()";
    }
}
