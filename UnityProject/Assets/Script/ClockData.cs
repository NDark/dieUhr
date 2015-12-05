/**

https://www.ego4u.com/en/cram-up/vocabulary/time

*/
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

    public static void DoSetValue(string _Key, int value)
    {
        if ("Minute" == _Key)
        {
            m_Minute = value / 6;
        }
        else if ("Hour" == _Key)
        {
            m_Hour = value / 30;
        }
        
    }

    public static void DoCalculateString(string _Key, int value)
    {
        DoSetValue(_Key, value);
        CalculateString();
    }

    public static void CalculateString_TraditionalChinese( int _Hour , int _Minute )
    {
        string minuteStr = "";
        string hourStr = "";

        // special case
        if (_Minute == 0 && _Hour == 0)
        {
            m_Label.text = Localization.Get("Mitternacht");
        }
        else if (_Minute == 0)
        {
            hourStr = (_Hour).ToString() + " " + Localization.Get("Uhr") + "鐘";
            m_Label.text = hourStr;
        }
        else
        {
            hourStr = (_Hour).ToString();
            minuteStr = (_Minute).ToString();
            m_Label.text = hourStr + " " + Localization.Get("Uhr") + " " + minuteStr + " 分" ;
        }
    }


    public static void CalculateString_English(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";
        // special case
        if (_Minute == 0 && _Hour == 0)
        {
            m_Label.text = Localization.Get("Mitternacht")  ;
        }
        else if (m_Minute == 0)
        {
            hourStr = (_Hour).ToString() + " " + Localization.Get("Uhr");
            m_Label.text = hourStr ;
        }
        else if (m_Minute == 15)
        {
            // Viertel after (hour)
            minuteStr = Localization.Get("Viertel");
            hourStr = " " + Localization.Get("nach") + " " + (_Hour).ToString();
            m_Label.text = minuteStr + " " + hourStr ;

        }
        else if (m_Minute == 45)
        {
            minuteStr = Localization.Get("Dreiviertel");
            hourStr = " " + (_Hour + 1).ToString();
            m_Label.text = minuteStr + " " + hourStr ;
        }
        else if (m_Minute == 30)
        {
            minuteStr = Localization.Get("Halb") + " ";
            hourStr = (_Hour).ToString();
            m_Label.text = minuteStr + " " + hourStr ;
        }
        else
        {
            hourStr = (_Hour).ToString();
            if (m_Minute < 10)
            {
                minuteStr = "(o) " + (m_Minute).ToString();
            }
            else
            {
                minuteStr = (m_Minute).ToString();
            }
            m_Label.text = hourStr + " " + minuteStr;
        }

    }


    public static void CalculateString_Deutsch(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";
        // special case

        if (m_Minute == 0 && _Hour == 0)
        {
            m_Label.text = Localization.Get("Mitternacht");
        }
        else if (m_Minute == 0)
        {
            hourStr = (_Hour).ToString() + " " + Localization.Get("Uhr");
            m_Label.text = minuteStr + " " + hourStr;
        }
        else if (m_Minute == 15)
        {
            if (_Hour == 0)
            {
                _Hour = 12;
            }

            int _HourPlus1 = _Hour + 1;
            if (_HourPlus1 > 12)
            {
                _HourPlus1 -= 12;
            }

            // Viertel after (hour)
            minuteStr = Localization.Get("Viertel");
            hourStr = " " + Localization.Get("nach") + " " + (_Hour).ToString();

            m_Label.text = minuteStr + " " + hourStr;
        }
        else if (m_Minute == 45)
        {
            minuteStr = Localization.Get("Viertel");
            hourStr = " " + Localization.Get("vor") + " " + (_Hour + 1).ToString();

            m_Label.text = minuteStr + " " + hourStr;
        }
        else if (m_Minute == 30)
        {
            minuteStr = Localization.Get("Halb") + " ";
            hourStr = (_Hour + 1).ToString();
            m_Label.text = minuteStr + " " + hourStr;
        }
        else
        {

            if (m_Minute >= 20 && m_Minute < 30)
            {
                minuteStr = (30 - m_Minute).ToString();
                hourStr = " vor halb " + (_Hour + 1).ToString();
            }
            else if (m_Minute > 30 && m_Minute <= 40)
            {
                minuteStr = (m_Minute - 30).ToString();
                hourStr = " nach halb " + (_Hour + 1).ToString();
            }
            else if (m_Minute > 40 && m_Minute < 60)
            {
                minuteStr = (60 - m_Minute).ToString();
                hourStr = " vor " + (_Hour + 1).ToString();
            }
            else if (m_Minute > 0 && m_Minute < 20)
            {
                if (_Hour == 0)
                {
                    _Hour = 12;
                }

                minuteStr = (m_Minute).ToString();
                hourStr = " nach " + (_Hour).ToString();
            }
            m_Label.text = minuteStr + " " + hourStr;
        }

    }


    public static void CalculateString()
    {
        // Debug.Log("m_Minute" + m_Minute);
        // Debug.Log("m_Hour"+ m_Hour);
        if (null == m_Label)
        {
            return;
        }

        if ("TraditionalChinese" == Localization.language)
        {
            CalculateString_TraditionalChinese(m_Hour, m_Minute);
        }
        else if ("English" == Localization.language)
        {
            CalculateString_English(m_Hour, m_Minute);
        }
        else if ("Deutsch" == Localization.language)
        {
            CalculateString_Deutsch(m_Hour, m_Minute);
        }

    }
}
