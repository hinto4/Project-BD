using UnityEngine;
using System.Collections;

public class ObstacleFallTrap : MonoBehaviour
{
    bool trapActivated = false;
    private AudioSource audioSource;

    public GameObject ObstacleObjects;
    public Transform SpawnLocation;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

	void OnTriggerEnter(Collider col)
    {
        if(col.GetComponent<Player>())
        {
            if(!trapActivated)
            {
                Instantiate(ObstacleObjects, SpawnLocation.transform.position, Quaternion.identity);
                audioSource.Play();
            }
            trapActivated = true;

        }
    }
}
