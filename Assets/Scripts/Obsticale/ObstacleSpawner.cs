using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject Obstacle;

    private float lastSpawn = 0;
    private bool spawned = false;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
    }

    public void Spawn()
    {
        if (!spawned && Time.time >= lastSpawn)
        {
            spawned = true;
            lastSpawn = Time.time + Random.Range(1f, 5f);
            GameObject cube = (GameObject)Instantiate(Obstacle, transform.position, Quaternion.identity);
            NetworkServer.Spawn(cube);
        }
        spawned = false;
        /*
        int FrameCount = Time.frameCount % 100;

        if(FrameCount == Random.Range(10,20))
        {
            Debug.Log("Spawning cube ");
            GameObject cube = (GameObject)Instantiate(Obstacle, transform.position, Quaternion.identity);
            NetworkServer.Spawn(cube);
        }*/
    }
}
