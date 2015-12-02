using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragArrow : MonoBehaviour
{
    public DragArrow hourSprite = null;
    public DragArrow minuteSprite = null;
    public UILabel uiLabel = null;
    public UILabel m_IntroductionLabel = null;
    public string key = "";
    bool m_IsPress = false;
    Vector3 centerVec = new Vector3(0.5f, 0.5f, 0);
    public float halfScreenWidth = 320;
    public float halfScreenHeight = 568;
    public float halfClockWidth = 320;
    public Camera UICamera = null;
    float m_Angle = 0;
    UISprite m_Sprite = null;
    // Use this for initialization
    void Start ()
    {
        halfScreenWidth = Screen.width / 2;
        halfScreenHeight = Screen.height / 2;
        if (null != uiLabel)
        {
            ClockData.SetupLabel(uiLabel);
        }
        m_Sprite = this.GetComponent<UISprite>();
    }



    public void SetupAM()
    {
        ClockData.SetupAMPM(false);
    }

    public void SetupPM()
    {
        ClockData.SetupAMPM(true);
    }

    public void DoUpdate()
    {
        ClockData.CalculateString();
    }

    // Update is called once per frame
    void Update ()
    {
        if (true == m_IsPress)
        {


            // Debug.Log("Input.mousePosition=" + Input.mousePosition);
            float x = (Input.mousePosition.x - halfScreenWidth) ;
            float y = (Input.mousePosition.y - halfScreenHeight) ;
            x = Mathf.Clamp(x, -1* halfClockWidth, halfClockWidth) / halfClockWidth;
            y = Mathf.Clamp(y, -1* halfClockWidth, halfClockWidth) / halfClockWidth;
            // Debug.Log("x=" + x);
            // Debug.Log("y=" + y);

            Vector3 viewPointPosition = new Vector3(x, y, 0);
            // Vector3 viewPointPosition = UICamera.ScreenToViewportPoint(Input.mousePosition);
            // Debug.Log("viewPointPosition=" + viewPointPosition);
            viewPointPosition.Normalize();
            float angle = Vector3.Angle(Vector3.up, viewPointPosition);
            // Debug.Log("angle=" + angle);

            if (x < 0)
            {
                angle = 360 - angle;
            }
            m_Angle = angle;
            UpdateRotationByAngle(m_Angle);
        }
    }

    Queue<float> lastUpdateMin = new Queue<float>();

    public void UpdateRotationByMinuteAngle(float _Angle)
    {

        if (lastUpdateMin.Count > 3)
        {
            lastUpdateMin.Dequeue();
        }
        float avgLastValue = avgLastList();

        // 360 -> 30 (360/12)
        lastUpdateMin.Enqueue(_Angle);

        int hourInt = (int)(m_Angle / 30.0f);
        
        int hourMod30 = (int)(m_Angle - hourInt * 30);
        /*
        Debug.Log("m_Angle=" + m_Angle);
        Debug.Log("hourInt=" + hourInt);
        Debug.Log("hourMod30=" + hourMod30);
        Debug.Log("_Angle=" + _Angle);
        Debug.Log("avgLastValue=" + avgLastValue);
        */
        if ( _Angle < 360 && _Angle > 270 && avgLastValue >= 0 && avgLastValue < 90)
        {
            Debug.LogWarning("avgLastValue=" + avgLastValue);
            // counter clock wise
            if (0 == hourInt)
            {
                hourInt = 12;
            }
            m_Angle = (hourInt - 1) * 30;
            ClockData.DoSetValue(this.key, (int)(m_Angle));
            return;
        }
        else if (_Angle >0 && _Angle < 90 && avgLastValue > 270 && avgLastValue < 360 )
        {
            // clock wise
            Debug.LogWarning("avgLastValue=" + avgLastValue);
            m_Angle = (hourInt + 1) * 30;
            ClockData.DoSetValue(this.key, (int)(m_Angle));
            return;
        }
        //*/
        float minAngle = _Angle / 12.0f;
        m_Angle = hourInt * 30.0f + minAngle;
        DoUpdateRotationByAngle(m_Angle);
    }

    public float avgLastList()
    {
        float sum = 0;
        if (lastUpdateMin.Count > 0)
        {
            foreach (float tmp in lastUpdateMin)
            {
                sum += tmp;
            }
            sum /= lastUpdateMin.Count;
        }
        return sum;
    }
    public void UpdateRotationByHourAngle(float _Angle)
    {
    }

    void DoUpdateRotationByAngle(float _Angle)
    {
        this.transform.localRotation = Quaternion.AngleAxis(_Angle, -Vector3.forward);
    }
    void UpdateRotationByAngle( float _Angle )
    {
        DoUpdateRotationByAngle(_Angle);
        if (this.hourSprite)
        {
            // minute to controll hour
            hourSprite.UpdateRotationByMinuteAngle(_Angle);
        }
        else if (this.minuteSprite)
        {
            hourSprite.UpdateRotationByHourAngle(_Angle);
        }
    }

    void OnPress( bool _Press )
    {
        m_IsPress = _Press;
        if (false == _Press)
        {
            ClockData.DoCalculateString(this.key, (int)(m_Angle));
        }
        else
        {
            HideIntroduction();
        }
    }

    void HideIntroduction()
    {
        if( null != m_IntroductionLabel )
        {
            m_IntroductionLabel.enabled = false;
            m_IntroductionLabel = null;
        }
        
    }
}
