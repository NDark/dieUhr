using UnityEngine;
using System.Collections;

public class SetClockData : MonoBehaviour
{

    public void SetDigital()
    {
        ClockData.SetDigital(true);
    }

    public void SetClock()
    {
        ClockData.SetDigital(false);

    }


    public void DoUpdate()
    {
        ClockData.CalculateString();
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
