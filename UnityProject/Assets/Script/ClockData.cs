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
        CalculateString();
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

    public static void CalculateString()
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
        string additionalStr = "";

        if ("TraditionalChinese" == Localization.language)
        {
            // special case
            if (m_Minute == 0)
            {
                hourStr = (hour24).ToString() + " " + Localization.Get("Uhr") + "鐘";
                m_Label.text = hourStr;
            }
            else
            {
                hourStr = (hour24).ToString();
                minuteStr = (m_Minute).ToString();
                m_Label.text = hourStr + " " + Localization.Get("Uhr") + " " + minuteStr + " 分" + additionalStr;
            }
        }
        else if ("English" == Localization.language || 
            "Deutsch" == Localization.language)
        {
            // special case
            if (m_Minute == 0)
            {
                if (hour24 == 0)
                {
                    if (Localization.language == "Deutsch")
                    {

                    }
                    else if (Localization.language == "English")
                    {
                    }

                }
                else
                {
                    hourStr = (hour24).ToString() + " " + Localization.Get("Uhr");
                    m_Label.text = minuteStr + " " + hourStr + additionalStr;
                }


            }
            else if (m_Minute == 15)
            {
                if (Localization.language == "Deutsch")
                {
                    // Viertel after (hour)
                    minuteStr = Localization.Get("Viertel");
                    hourStr = " " + Localization.Get("nach") + " " + (hour24).ToString();

                    additionalStr = " (" + minuteStr + " " + Localization.Get("to") + " " + (hour24 + 1).ToString() + ")";
                }
                else if (Localization.language == "English")
                {
                    // Viertel after (hour)
                    minuteStr = Localization.Get("Viertel");
                    hourStr = " " + Localization.Get("nach") + " " + (hour24).ToString();
                }
                m_Label.text = minuteStr + " " + hourStr + additionalStr;
            }
            else if (m_Minute == 45)
            {
                if (Localization.language == "Deutsch")
                {
                    minuteStr = Localization.Get("Viertel");
                    hourStr = " " + Localization.Get("vor") + " " + (hour24 + 1).ToString();

                    additionalStr = " (" + Localization.Get("Dreiviertel") + " " + (hour24 + 1).ToString() + ")";
                }
                else if (Localization.language == "English")
                {
                    minuteStr = Localization.Get("Dreiviertel");
                    hourStr = " " + (hour24 + 1).ToString();
                }
                m_Label.text = minuteStr + " " + hourStr + additionalStr;
            }
            else if (m_Minute == 30)
            {
                minuteStr = Localization.Get("Halb") + " ";
                if (Localization.language == "Deutsch")
                {
                    hourStr = (hour24 + 1).ToString();
                }
                else if (Localization.language == "English")
                {
                    hourStr = (hour24).ToString();
                }
                m_Label.text = minuteStr + " " + hourStr + additionalStr;
            }
            else
            {
                if (Localization.language == "Deutsch")
                {
                    if (m_Minute >= 20 && m_Minute < 30)
                    {
                        minuteStr = (30 - m_Minute).ToString();
                        hourStr = " vor halb " + (hour24 + 1).ToString();
                    }
                    else if (m_Minute > 30 && m_Minute <= 40)
                    {
                        minuteStr = (m_Minute - 30).ToString();
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
                else if (Localization.language == "English")
                {
                    hourStr = (hour24).ToString();
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


            
        }

    }
}
