using UnityEngine;
using System.Collections;

public class PlatformLever : MonoBehaviour
{

	void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.gameObject;

        if(obj.tag == "Wall")
        {
            Debug.Log("Collision with a wall");
        }
    }
}
