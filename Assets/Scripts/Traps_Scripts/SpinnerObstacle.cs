using UnityEngine;
using System.Collections;

public class SpinnerObstacle : ObstacleBehaviour
{
    public void SpinObstacles()
    {
        if(this.transform.tag == "Spinner")     
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 100, Space.World);
        }        
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.gameObject;
        Debug.Log("Col with " + obj.GetComponent<Rigidbody>());
        if(obj.GetComponent<Player>())
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(5000f, transform.position, 3.0f);
            }
        }
    }

    public override void OnBecameInvisible()
    {
        // Do not destroy this item when out of sight.
    }
}
