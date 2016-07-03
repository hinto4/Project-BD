using UnityEngine;
using System.Collections;

public class ObstacleItemAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision col)
    {
        audioSource.Play();
    }
}
