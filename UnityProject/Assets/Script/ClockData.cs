﻿/**

https://www.ego4u.com/en/cram-up/vocabulary/time

@date 20151212 by NDark . add class method WordFromDigital_ConsiderOne()

@date 20160213 by NDark 
. add checking minute 1 at CalculateString_Deutsch_DigitalMode()
. add checking minute 1 at CalculateString_Deutsch()

@date 20160229 by NDark
. add class member m_ExampleButton
. add class member m_ExampleContent
. add class method SetupExampleButton()


. add class method ShowNGUIObj()
. add class method UpdateNGUILabel()

*/
using UnityEngine;
using System.Collections;

public static class ClockData
{

	static UILabel m_Label = null;
	static GameObject m_ExampleButton = null;
	static UILabel m_ExampleContent = null;

	static int m_Minute = 0;
	static int m_Hour { 
		get { return (m_HourFromInput + additionalHourFromMinute); }
		set { m_HourFromInput = value; } 
	}
	static int m_HourFromInput = 0;
	static bool m_IsDigital = false;

	static int m_RandomIndex = 0 ;
	
    public static void SetDigital(bool _Set)
    {
        m_IsDigital = _Set;
    }
    public static void SetupLabel(UILabel _Label)
    {
        m_Label = _Label;
    }
    
	public static void SetupExampleButton(GameObject _Obj , UILabel _Content )
	{
		m_ExampleButton = _Obj;
		m_ExampleContent = _Content ;
	}
	
    public static void ResetValue()
    {
        m_Minute = 0;
        m_Hour = 0;
    }

    public static void DoSetValue(string _Key, int value)
    {
		// Debug.Log("value" + value);

		if ("Minute" == _Key)
        {
			int aroundMinute = (value + 3) / 6;
			if (aroundMinute >= 60)
			{
				additionalHourFromMinute = 1;
			}
			else
			{
				additionalHourFromMinute = 0;
			}
			m_Minute = aroundMinute%60;

		}
        else if ("Hour" == _Key)
        {
            m_Hour = value / 30 ;
			additionalHourFromMinute = 0;

		}
        
    }

	static int additionalHourFromMinute = 0;
    public static void DoCalculateString(string _Key, int value)
    {
        DoSetValue(_Key, value);
        CalculateString();
        
        // update content
        
		m_RandomIndex = Random.Range( 0 , 10 ) ;
		UpdateExampleSentence() ;
    }
    
	public static void UpdateExampleSentence()
	{
		if ("Polish" == Localization.language)
		{ 
			UpdateExampleSentence_Polish();
			return ;
		}

		string key = "ClockExampleKey_" + m_RandomIndex ;
		string exampleSentence = Localization.Get( key ) ;
		string replaceTimeKey = "<time>" ;
		exampleSentence = exampleSentence.Replace( replaceTimeKey , m_Label.text ) ;
		UpdateNGUILabel( m_ExampleContent , exampleSentence ) ;
		
		// show button
		ShowNGUIObj( m_ExampleButton , true ) ;	
	}	

    public static void CalculateString_TraditionalChinese( int _Hour , int _Minute )
    {
		// Debug.Log("_Minute"+ _Minute);
		// Debug.Log("_Hour"+ _Hour);

		string minuteStr = "";
        string hourStr = "";

        if (_Hour == 2)
        {
            hourStr = WordFromTwo_TraditionalChinese() + Localization.Get("Uhr");
        }
        else 
        {
			if( _Hour > 12 )
			{
				_Hour -= 12 ;
			}
            hourStr = WordFromDigital(_Hour) + Localization.Get("Uhr") ;
			// Debug.Log("_Hour=" + _Hour);
			// Debug.Log("hourStr=" + hourStr);
        }
        
        if (_Minute <= 10)
        {
            minuteStr = WordFromDigital(_Minute);
        }
        else if (_Minute < 20)
        {
            minuteStr = WordFromDigital(10) + WordFromDigital(_Minute % 10);
        }
		else if( 0 == _Minute%10 )
		{
			minuteStr = WordFromDigital(_Minute/10) + WordFromDigital(10) ;
		}
        else 
        {
            minuteStr = WordFromDigital(_Minute/10) + WordFromDigital(10) + WordFromDigital(_Minute%10);
        }

        // special case
        if (_Minute == 0 && (_Hour == 0 || _Hour == 12))
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

		if( 1 == _Minute )
		{
			minuteStr = WordFromDigital_ConsiderOne(_Minute) ;
		}
		else
		{
        	minuteStr = DeutschMinuteFromDigital(_Minute) ;
        }
        
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
        string hourStrWiths = WordFromDigital_ConsiderOne(_Hour);

        if (_Minute == 0 && (_Hour == 0|| 12== _Hour))
        {
            m_Label.text = Localization.Get("Mitternacht");
        }
        else if (_Minute == 0)
        {
            string additionText = "";
            if (1 == _Hour)
            {
                additionText = " ( " + hourStr + "s" + " )" ;
            }
            m_Label.text = hourStr + " " + Localization.Get("Uhr") + additionText ;
        }
		
        else if (_Minute == 15)
        {


            // Viertel after (hour)
            minuteStr = Localization.Get("Viertel");

            m_Label.text = minuteStr + " " + Localization.Get("nach") + " " + hourStrWiths;
        }
        else if (_Minute == 45)
        {
            
            hourStr = WordFromDigital_ConsiderOne(_HourPlus1);

            minuteStr = Localization.Get("Viertel");

            m_Label.text = minuteStr + " " + Localization.Get("vor") + " " + hourStr;
        }
        else if (_Minute == 30)
        {
            
            hourStr = WordFromDigital_ConsiderOne(_HourPlus1);
            minuteStr = Localization.Get("Halb") + " ";
            
            m_Label.text = minuteStr + hourStr;
        }
        else
        {

            if (_Minute >= 20 && _Minute < 30)
            {
				minuteStr = WordFromDigital_ConsiderOne(30 - m_Minute);
                hourStr = "vor halb " + WordFromDigital_ConsiderOne(_HourPlus1);
            }
            else if (_Minute > 30 && _Minute <= 40)
            {
				minuteStr = WordFromDigital_ConsiderOne(m_Minute - 30);
                hourStr = "nach halb " + WordFromDigital_ConsiderOne(_HourPlus1);
            }
            else if (_Minute > 40 && _Minute < 60)
            {
				minuteStr = WordFromDigital_ConsiderOne(60 - _Minute) ;
                hourStr = "vor " + WordFromDigital_ConsiderOne(_HourPlus1);
            }
            else if (_Minute >= 1 && _Minute < 20)
            {
				minuteStr = WordFromDigital_ConsiderOne(_Minute);
                hourStr = "nach " + WordFromDigital_ConsiderOne(_Hour);
            }
            m_Label.text = minuteStr + " " + hourStr;
        }

    }


    public static void CalculateString()
    {
		/*
        Debug.Log("m_Minute" + m_Minute);
        Debug.Log("m_Hour"+ m_Hour);
		Debug.Log("additionalHourFromMinute" + additionalHourFromMinute);
		//*/

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
		else if ("Polish" == Localization.language)
        {
            if (m_IsDigital)
            {
                CalculateString_Polish_DigitalMode(m_Hour, m_Minute);
            }
            else
            {
                CalculateString_Polish_Tradition(m_Hour, m_Minute);
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

	
    static string WordFromDigital_ConsiderPolishMinuty(int _Digital)
    {
        if (1 == _Digital)
        {
            return Localization.Get(_Digital.ToString()) + " " + Localization.Get("minute");
        }
        else
        {
            return Localization.Get(_Digital.ToString()) + " " + Localization.Get("minutes");
        }

    }
	
    static string WordFromDigital_ConsiderOne(int _Digital)
    {
        if (1 == _Digital)
        {
            return Localization.Get(_Digital.ToString()) + "s";
        }
        else
        {
            return Localization.Get(_Digital.ToString()) ;
        }

    }
	
    static string WordFromNumberRd(int _Digital)
    {
        return Localization.Get( _Digital.ToString() + "rd" );
    }

    static string WordFromDigital(int _Digital)
    {
        return Localization.Get( _Digital.ToString() );
    }

    static string WordFromDigital_LowerCase(int _Digital)
    {
        return WordFromDigital(_Digital).ToLower(); 
    }
    
	
	
	static void ShowNGUIObj( GameObject _Obj , bool _Show )
	{
		if( null == _Obj )
		{
			return ;
		}
		
		NGUITools.SetActive( _Obj , _Show ) ;
	}
	
	static void UpdateNGUILabel( UILabel _Label ,  string _Content )
	{
		if( null == _Label )
		{
			return ;
		}
		
		_Label.text = _Content ; 
	}
	

    public static void CalculateString_Polish_DigitalMode(int _Hour, int _Minute)
    {
        string minuteStr = "";
        string hourStr = "";
        // special case

        hourStr = WordFromNumberRd(_Hour);

		if (_Minute < 20 || _Minute%10 == 0 )
        {
            minuteStr = WordFromDigital(_Minute);
        }
        else
        {
            minuteStr = WordFromDigital(_Minute / 10 * 10 ) + " " + WordFromDigital(_Minute%10);
        }
        
        if (_Minute == 0)
        {
			string format = Localization.Get("Uhr");
			string onOclockStr = format.Replace("(HR)" ,hourStr );
            m_Label.text = onOclockStr ;
        }
        else
        {
            m_Label.text = hourStr + " " + minuteStr;
        }

    }

	
    public static void CalculateString_Polish_Tradition(int _Hour, int _Minute)
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

        hourStr = WordFromNumberRd(_Hour);

        if (_Minute == 0 && (_Hour == 0|| 12== _Hour))
        {
            m_Label.text = Localization.Get("Mitternacht");
        }
        else if (_Minute == 0)
        {
			string format = Localization.Get("Uhr");
			string onOclockStr = format.Replace("(HR)" ,hourStr );
            m_Label.text = onOclockStr ;
        }
		
        else if (_Minute == 15)
        {
            // kwartał po (hour)
            string str2 = Localization.Get("Viertel") + " " + Localization.Get("nach") + " " + hourStr;
			minuteStr = WordFromDigital(m_Minute);
			string str1 = minuteStr + " " + Localization.Get("nach") + " " + hourStr;
			m_Label.text = str2 + " (albo " + str1 + ")";
        }
        else if (_Minute == 45)
        {
            // trzy kwartały po (hour)
            
			string str1 = Localization.Get("Dreiviertel") + " " + Localization.Get("nach") + " " + hourStr;
			
			string hourPlus1Str = WordFromNumberRd(_HourPlus1);

			//  Za kwartał (hour+1)
			string format = Localization.Get("to") ;
			string str2 = format ;
			str2 = str2.Replace("(MN)",Localization.Get("Viertel"));
			str2 = str2.Replace("(HR)",hourPlus1Str);
            m_Label.text = str2 + " (albo " + str1 + ")";
        }
        else if (_Minute == 30)
        {
			// w pół do czwartej
            string format = Localization.Get("Halb") ;
            hourStr = WordFromNumberRd(_HourPlus1);
            string str = format.Replace("(HR)",hourStr) ;
            m_Label.text = str;
        }
        else
        {

            if (_Minute >= 20 && _Minute < 30)
            {
				minuteStr = WordFromDigital_ConsiderPolishMinuty(30 - m_Minute);
				string format = Localization.Get("Halb");
				hourStr = format.Replace("(HR)" , PolishTaToTEj( WordFromNumberRd(_HourPlus1) ) );
                hourStr = Localization.Get("vor") + " " + hourStr;
            }
            else if (_Minute > 30 && _Minute <= 40)
            {
				minuteStr = WordFromDigital_ConsiderPolishMinuty(m_Minute - 30);
				string format = Localization.Get("Halb");
				hourStr = format.Replace("(HR)" , PolishTaToTEj( WordFromNumberRd(_HourPlus1) ) );
                hourStr = Localization.Get("nach") + " " + hourStr;

            }
            else if (_Minute > 40 && _Minute < 60)
            {
				minuteStr = WordFromDigital_ConsiderPolishMinuty(60 - _Minute);
                hourStr = Localization.Get("vor") +" " + PolishTaToTEj( WordFromNumberRd(_HourPlus1) );
            }
            else if (_Minute >= 1 && _Minute < 20)
            {
				minuteStr = WordFromDigital_ConsiderPolishMinuty(_Minute);
                hourStr = Localization.Get("nach") + " " + PolishTaToTEj( WordFromNumberRd(_Hour) );
            }
            m_Label.text = minuteStr + " " + hourStr;
        }

    }

	public static string PolishTaToTEj( string input )
	{ 
		if( input.EndsWith("a"))
		{ 
			var lastIndex = input.LastIndexOf("a") ;
			return input.Substring(0,lastIndex) + "ej";
		}
		return input ;
		
		
	}
	
	public static void UpdateExampleSentence_Polish()
	{
		string key = "ClockExampleKey_" + m_RandomIndex ;
		string exampleSentence = Localization.Get( key ) ;
		string replaceTimeKey = "<otime>" ;
		var timeStr = string.Empty ;
		var orgStr  =m_Label.text;
		// Debug.Log("timeStr" + timeStr);

		char [] splitor = { ' '} ;
		var strvec = orgStr.Split(splitor);
		// pierwsza godzina -> o pierwszej godzinie
		
		if( !orgStr.StartsWith("o ") && strvec.Length>0
			&& strvec[0].EndsWith("a") )
		{ 
			if( orgStr.EndsWith("godzina"))
			{ 
				timeStr = "o " + PolishTaToTEj( strvec[0] ) + " godzinie" ;
			}
			else 
			{ 
				timeStr = "o " + PolishTaToTEj( strvec[0] ) ;
				for( var i = 1 ; i <strvec.Length ; ++i )
				{ 
					timeStr+= " " + strvec[i];
				}
			}
			
		}

		/*
		Debug.Log("orgStr=" + orgStr);
		Debug.Log("timeStr=" + timeStr);
		//*/
		if( string.IsNullOrEmpty(timeStr ))
		{ 
			if( orgStr.StartsWith("o"))
			{
				timeStr = orgStr ;
			}
			else 
			{
				timeStr = "o " + orgStr ;
			}
			
		}
		

		exampleSentence = exampleSentence.Replace( replaceTimeKey , timeStr ) ;

		UpdateNGUILabel( m_ExampleContent , exampleSentence ) ;
		
		// show button
		ShowNGUIObj( m_ExampleButton , true ) ;	
	}	



}
