using UnityEngine;
using System.Collections;

public class DragArrow : MonoBehaviour
{
    public UILabel uiLabel = null;
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
	
	// Update is called once per frame
	void Update ()
    {
        if (true == m_IsPress)
        {


            Debug.Log("Input.mousePosition=" + Input.mousePosition);
            float x = (Input.mousePosition.x - halfScreenWidth) ;
            float y = (Input.mousePosition.y - halfScreenHeight) ;
            x = Mathf.Clamp(x, -1* halfClockWidth, halfClockWidth) / halfClockWidth;
            y = Mathf.Clamp(y, -1* halfClockWidth, halfClockWidth) / halfClockWidth;
            Debug.Log("x=" + x);
            Debug.Log("y=" + y);

            Vector3 viewPointPosition = new Vector3(x, y, 0);
            // Vector3 viewPointPosition = UICamera.ScreenToViewportPoint(Input.mousePosition);
            Debug.Log("viewPointPosition=" + viewPointPosition);
            viewPointPosition.Normalize();
            float angle = Vector3.Angle(Vector3.up, viewPointPosition);
            Debug.Log("angle=" + angle);
            
            if (x < 0)
            {
                angle = 360 - angle;
            }
            m_Angle = angle;
            this.transform.localRotation = Quaternion.AngleAxis(angle, -Vector3.forward);

        }
    }

    void OnPress( bool _Press )
    {
        m_IsPress = _Press;
        if (false == _Press)
        {
            ClockData.DoCalculateString(this.key, (int)(m_Angle));
        }
    }
}
