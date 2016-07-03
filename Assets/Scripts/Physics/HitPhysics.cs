using UnityEngine;
using System.Collections;

public class HitPhysics : MonoBehaviour
{
    public RagdollSystem ragdollSystem;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<ObstacleBehaviour>())
        {
            Debug.Log(this.transform.gameObject);
            
            ragdollSystem.ragdoll = true;

            this.gameObject.GetComponent<Rigidbody>().AddForce
                (col.gameObject.transform.position, ForceMode.Force);
        }       
    }
}
