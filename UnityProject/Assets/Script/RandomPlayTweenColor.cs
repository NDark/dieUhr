using UnityEngine;
using System.Collections;

public class RandomPlayTweenColor : MonoBehaviour
{
    float nextPlay = 0.0f;
    TweenColor color = null;
    bool isReadyToStop = false;
    // Use this for initialization
    void Start ()
    {
        RandomNextPlay();
        color = this.GetComponent<TweenColor>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (null == color)
        {
            return;
        }

        if (Time.timeSinceLevelLoad > nextPlay )
        {
            if (true == isReadyToStop)
            {
                this.enabled = false;
                return;
            }

            color.Play();
            RandomNextPlay();
        }
	
	}

    void OnPress(bool _Active)
    {
        isReadyToStop = true;

    }

    void RandomNextPlay()
    {
        nextPlay = Time.timeSinceLevelLoad + Random.Range(10, 30);
    }
}
