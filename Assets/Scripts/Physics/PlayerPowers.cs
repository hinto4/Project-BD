using UnityEngine;
using System.Collections;

public class PlayerPowers : MonoBehaviour
{
    Camera cam;

    Animator animator;

    GameObject DraggingItem;

    public float draggingItemDistance;

    private bool isDragging = false;

    float mouseY;

    float moveSpeed = 0.01f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(cam == null)
        {
            cam = FindObjectOfType<Camera>();
            Debug.Log("Found camera");
        }       

        if (Input.GetMouseButtonDown(0) && DraggingItem == null)            // Pick up dragable item and call RayFindGameObjectWithTag moveable.
        {   
            RayFindGameObjectWithTag("Moveable");
            isDragging = true;   
        }
        else if(Input.GetMouseButtonDown(0) && DraggingItem != null)        // Drop the item after second time mouse 0 input.
        {
            DraggingItem.GetComponent<Rigidbody>().useGravity = true;
            DraggingItem = null;
            draggingItemDistance = 0f;                                      // Reset the draggintItemDistance.
            isDragging = false;
        }

        if(DraggingItem != null)                                            // Handle item dragging, update transforms.
        {
            Vector3 MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);         

            // Try to play with RAYS, once object is in focus, we gotta start mouse positioning from 0 depending on that object.
            // Gotta get this direction shoot from camera.
            DraggingItem.transform.position = Vector3.Lerp(DraggingItem.transform.position,
                new Vector3(MousePosition.x + ray.direction.x * 10f, MousePosition.y + ray.direction.y * 10f, 
                DraggingItem.transform.position.z + draggingItemDistance), moveSpeed);
            
            Cursor.visible = false;

            /*
            DraggingItem.transform.position = Vector3.Lerp(DraggingItem.transform.position,
                new Vector3((ray.origin.x - MousePosition.x) * 55f, (ray.origin.y - MousePosition.y) * 55f,
                ray.origin.z + draggingItemDistance), moveSpeed);*/
        }
        else
        {
            Cursor.visible = true;
        }

        if (isDragging)
        {
            draggingItemDistance += Input.GetAxis("Mouse ScrollWheel") * 2f;
        }
    }

    /// <summary>
    ///  returns GameObject that ray returns with a tag.
    ///</summary>
    public GameObject RayFindGameObjectWithTag(string tag) 
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.transform.gameObject.tag == tag)
            {
                DraggingItem = hit.transform.gameObject;
                DraggingItem.GetComponent<Rigidbody>().useGravity = false;               
                return DraggingItem;
            }
            else
            {
                return null;
            }
        }     
        else
        {
            return null;
        }
    }
    
    void OnAnimatorIK()
    {
        if(animator)
        {
            if(DraggingItem != null)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(DraggingItem.transform.position);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                animator.SetIKPosition(AvatarIKGoal.RightHand,
                    DraggingItem.transform.position);

                animator.SetIKRotation(AvatarIKGoal.RightHand,
                    DraggingItem.transform.rotation);
            }
        }
    }
}
