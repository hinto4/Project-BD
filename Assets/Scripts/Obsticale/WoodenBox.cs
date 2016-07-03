using UnityEngine;
using System.Collections;

public class WoodenBox : ObstacleBehaviour
{
	void FixedUpdate()
    {
       // GetComponent<Rigidbody>().AddForce(Vector3.back * 3000f * Time.deltaTime);
    }

    public override void OnBecameInvisible()
    {
        // Do nothing atm.
    }
}
