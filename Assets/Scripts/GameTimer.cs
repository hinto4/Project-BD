using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    public float CountDownTime = 2f;

    public bool StartMatch = false;

    void Update()
    {
        if(StartMatch)
        {
            Debug.Log(CountDownTimer());
        }
    }

    public int CountDownTimer()
    {
        CountDownTime -= Time.deltaTime;
        return Mathf.CeilToInt(CountDownTime);
    }
}
