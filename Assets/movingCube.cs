using UnityEngine;
using System.Collections;

public class movingCube : MonoBehaviour
{
    public Transform Distance;

    public float Speed;

    public float DistanceCovered;

    float startTime;
    float journeyLength;

    void Start()
    {
        startTime = Time.time;
        Debug.Log(startTime);
        journeyLength = Vector3.Distance(this.transform.position, Distance.transform.position);
    }

	void Update ()
    {
        Debug.Log(startTime);

        float distCovered = (Time.time - startTime) * Speed;
        DistanceCovered = distCovered;

        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(this.transform.position, Distance.transform.position, fracJourney);
	}
}
