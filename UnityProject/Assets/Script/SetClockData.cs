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

    public void ResetValue()
    {
        ClockData.ResetValue();
        ClockData.CalculateString();
    }

    public void DoUpdate()
    {
        ClockData.CalculateString();
		ClockData.UpdateExampleSentence();
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
