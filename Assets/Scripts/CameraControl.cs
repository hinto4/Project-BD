using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraControl : MonoBehaviour
{
    private GameObject Target;

    void FixedUpdate()
    {
        if(Target != null)
        {
            CameraOffset();
        }   
    }

    public void StartCamera()
    {
        Camera cam = GetComponent<Camera>();
        cam.enabled = true;

        AudioListener cameraAudioListener = GetComponent<AudioListener>();
        cameraAudioListener.enabled = true;
    }

    public void FollowTarget(GameObject target)
    {
        Target = target;
    }

    void CameraOffset()
    {
        // TODO Make camera offset more dynamic.
        transform.position = new Vector3(Target.transform.position.x,
            Target.transform.position.y + 4,
            Target.transform.position.z - 7);
    }
}
