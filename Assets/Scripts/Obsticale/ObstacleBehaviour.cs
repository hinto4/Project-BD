using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ObstacleBehaviour : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        GameObject obj = col.gameObject;

        if (obj.tag == "Shredder")
        {
            Destroy(gameObject);
            NetworkServer.UnSpawn(gameObject);
        }
    }

    public virtual void OnBecameInvisible()
    {
        //Destroy(gameObject);
    }
}
