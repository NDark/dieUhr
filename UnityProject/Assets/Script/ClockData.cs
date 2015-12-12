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
    static bool m_IsDigital = false;

    public static void SetDigital(bool _Set)
    {
        m_IsDigital = _Set;
    }
    public static void SetupLabel(UILabel _Label)
    {
        m_Label = _Label;
    }

    public static void ResetValue()
    {
        m_Minute = 0;
        m_Hour = 0;
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

        if (_Hour == 2)
        {
            hourStr = WordFromTwo_TraditionalChinese() + Localization.Get("Uhr");
        }
        else
        {
            hourStr = WordFromDigital(_Hour) + Localization.Get("Uhr") ;
        }

        if (_Minute <= 10)
        {
            minuteStr = WordFromDigital(_Minute);
        }
        else if (_Minute < 20)
        {
            minuteStr = WordFromDigital(10) + WordFromDigital(_Minute % 10);
        }
        else 
        {
            minuteStr = WordFromDigital(_Minute/10) + WordFromDigital(10) + WordFromDigital(_Minute%10);
        }

        // special case
        if (_Minute == 0 && _Hour == 0)
        {
            m_Label.text = Localization.Get("Mitternacht");
        }
        else if (_Minute == 0)
        {
            
            hourStr = hourStr + "鐘";
            m_Label.text = hourStr;
        }
        else
        {
            
            m_Label.text = hourStr + minuteStr + "分" ;
        }
    }


    public static void CalculateString_English_DigitalMode(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";

        hourStr = WordFromDigital(_Hour);

        if (_Minute < 10)
        {
            minuteStr = "(o) " + WordFromDigital(_Minute);
        }
        else if (_Minute < 20 || _Minute%10 == 0 )
        {
            minuteStr = WordFromDigital(_Minute);
        }
        else
        {
            minuteStr = WordFromDigital(_Minute / 10 * 10 ) + " " + WordFromDigital(_Minute%10);
        }

        // special case
        if (_Minute == 0)
        {
            m_Label.text = hourStr + " " + Localization.Get("Uhr");
        }
        else
        {
            m_Label.text = hourStr + " " + minuteStr;
        }

    }

    public static void CalculateString_English(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";


        hourStr = WordFromDigital(_Hour);



        // special case
        if (_Minute == 0 && _Hour == 0)
        {
            m_Label.text = Localization.Get("Mitternacht")  ;
        }
        else if (m_Minute == 0)
        {
            m_Label.text = hourStr + " " + Localization.Get("Uhr");
        }
        else if (m_Minute == 15)
        {
            // Viertel after (hour)
            minuteStr = Localization.Get("Viertel");
            m_Label.text = minuteStr + " " + Localization.Get("nach") + " " + hourStr ;

        }
        else if (m_Minute == 45)
        {
            minuteStr = Localization.Get("Dreiviertel");
            hourStr = WordFromDigital(_Hour+1);
            m_Label.text = minuteStr + " " + hourStr ;
        }
        else if (m_Minute == 30)
        {
            minuteStr = Localization.Get("Halb") ;
            m_Label.text = minuteStr + " " + hourStr ;
        }
        else
        {
            if (_Minute < 10)
            {
                minuteStr = "(o) " + WordFromDigital(_Minute);
            }
            else if (_Minute < 20 || _Minute % 10 == 0)
            {
                minuteStr = WordFromDigital(_Minute);
            }
            else
            {
                minuteStr = WordFromDigital(_Minute / 10 * 10) + " " + WordFromDigital(_Minute % 10);
            }

            
            m_Label.text = hourStr + " " + minuteStr;
        }

    }


    public static void CalculateString_Deutsch_DigitalMode(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";
        // special case

        hourStr = WordFromDigital(_Hour);

        minuteStr = DeutschMinuteFromDigital(_Minute).ToLower() ;
        
        if (_Minute == 0)
        {
            m_Label.text = hourStr + " " + Localization.Get("Uhr");
        }
        else
        {
            m_Label.text = hourStr + " " + Localization.Get("Uhr") + " "  + minuteStr;
        }

    }

    public static void CalculateString_Deutsch(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";

        if (_Hour > 12)
        {
            _Hour -= 12;
        }

        int _HourPlus1 = _Hour + 1;
        if (_HourPlus1 > 12)
        {
            _HourPlus1 -= 12;
        }
        hourStr = WordFromDigital(_Hour);


        if (_Minute < 10)
        {
            minuteStr = WordFromDigital_LowerCase(_Minute);
        }
        else if (_Minute < 20 || _Minute % 10 == 0)
        {
            minuteStr = WordFromDigital_LowerCase(_Minute);
        }
        else
        {
            minuteStr = WordFromDigital_LowerCase(_Minute % 10) + "und" + WordFromDigital_LowerCase(_Minute / 10 * 10);
        }


        if (_Minute == 0 && _Hour == 0)
        {
            m_Label.text = Localization.Get("Mitternacht");
        }
        else if (_Minute == 0)
        {
            m_Label.text = hourStr + " " + Localization.Get("Uhr");
        }
        else if (_Minute == 15)
        {


            // Viertel after (hour)
            minuteStr = Localization.Get("Viertel");

            m_Label.text = minuteStr + " " + Localization.Get("nach") + " " + hourStr.ToLower() ;
        }
        else if (_Minute == 45)
        {
            
            hourStr = WordFromDigital(_HourPlus1);

            minuteStr = Localization.Get("Viertel");

            m_Label.text = minuteStr + " " + Localization.Get("vor") + " " + hourStr.ToLower();
        }
        else if (_Minute == 30)
        {
            
            hourStr = WordFromDigital(_HourPlus1);
            minuteStr = Localization.Get("Halb") + " ";
            
            m_Label.text = minuteStr + hourStr.ToLower();
        }
        else
        {
            

            if (_Minute >= 20 && _Minute < 30)
            {
                
                minuteStr = WordFromDigital(30 - m_Minute);
                hourStr = "vor halb " + WordFromDigital_LowerCase(_HourPlus1);
            }
            else if (_Minute > 30 && _Minute <= 40)
            {
                minuteStr = WordFromDigital(m_Minute - 30);
                hourStr = "nach halb " + WordFromDigital_LowerCase(_HourPlus1);
            }
            else if (_Minute > 40 && _Minute < 60)
            {
                minuteStr = DeutschMinuteFromDigital(60 - _Minute) ;
                hourStr = "vor " + WordFromDigital_LowerCase(_HourPlus1);
            }
            else if (_Minute > 0 && _Minute < 20)
            {
                minuteStr = DeutschMinuteFromDigital(_Minute);
                hourStr = "nach " + WordFromDigital_LowerCase(_Hour);
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
            if (m_IsDigital)
            {
                CalculateString_English_DigitalMode(m_Hour, m_Minute);
            }
            else
            {
                CalculateString_English(m_Hour, m_Minute);
            }
        }
        else if ("Deutsch" == Localization.language)
        {
            if (m_IsDigital)
            {
                CalculateString_Deutsch_DigitalMode(m_Hour, m_Minute);
            }
            else
            {
                CalculateString_Deutsch(m_Hour, m_Minute);
            }
            
        }

    }

    static string DeutschMinuteFromDigital( int _Digital )
    {
        string ret = "";
        if (_Digital < 10)
        {
            ret = WordFromDigital(_Digital);
        }
        else if (_Digital < 20 || _Digital % 10 == 0)
        {
            ret = WordFromDigital(_Digital);
        }
        else
        {
            ret = WordFromDigital(_Digital % 10) + "und" + WordFromDigital_LowerCase(_Digital / 10 * 10);
        }
        return ret;
    }

    static string WordFromTwo_TraditionalChinese()
    {
        return Localization.Get( "h2" );
    }

    static string WordFromDigital(int _Digital)
    {
        return Localization.Get( _Digital.ToString() );
    }

    static string WordFromDigital_LowerCase(int _Digital)
    {
        return WordFromDigital(_Digital).ToLower(); 
    }
}
