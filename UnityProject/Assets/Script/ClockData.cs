using UnityEngine;
using System.Collections;

public static class ClockData
{

    static UILabel m_Label = null;
    static int m_Minute = 0;
    static int m_Hour = 0;
    static int m_AMPM = 0;
    public static void SetupAMPM( bool _PM )
    {
        m_AMPM = (true == _PM) ? 12 : 0;
    }
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
        Debug.Log("24 hour" + (m_Hour + m_AMPM) );
        if (null == m_Label)
        {
            return;
        }
        int hour24 = (m_Hour + m_AMPM);
        string minuteStr = "";
        string hourStr = "";
        

        // special case
        if (m_Minute == 15)
        {
            minuteStr = "Viertel";
            hourStr = " nach " + (hour24).ToString();
        }
        else if (m_Minute == 45)
        {
            minuteStr = "Viertel";
            hourStr = " vor " + (hour24 + 1).ToString();
        }
        else if (m_Minute == 30)
        {
            minuteStr = "Halb ";
            hourStr = (hour24 + 1).ToString();
        }
        else if (m_Minute >= 20 && m_Minute < 30)
        {
            minuteStr = (30-m_Minute).ToString() ;
            hourStr = " vor halb " + (hour24 + 1).ToString();
        }
        else if (m_Minute > 30 && m_Minute <= 40)
        {
            minuteStr = (m_Minute-30).ToString();
            hourStr = " nach halb " + (hour24 + 1).ToString();
        }
        else if (m_Minute > 40 && m_Minute < 60)
        {
            minuteStr = (60 - m_Minute).ToString();
            hourStr = " vor " + (hour24 + 1).ToString();
        }
        else if (m_Minute > 0 && m_Minute < 20)
        {
            minuteStr = (m_Minute).ToString();
            hourStr = " nach " + (hour24).ToString();
        }

        m_Label.text = minuteStr + " " + hourStr ;
    }
}
