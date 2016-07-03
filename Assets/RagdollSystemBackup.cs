using UnityEngine;
using System.Collections;

public class RagdollSystemBackup : MonoBehaviour
{
    protected Animator animator;

    private Rigidbody boneRB;
    private Collider boneCol;

    private Collider[] playerCol;
    private Rigidbody playerRB;

    public bool enableRagDoll = false;
    public GameObject[] bones;

    private GameObject rightHandJoint;
    private GameObject leftHandJoint;
    private GameObject rightFootJoint;
    private GameObject leftFootJoint;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponents<Collider>();

        Debug.Log(playerCol.Length);

        animator = GetComponent<Animator>();       
    }

    void Update()
    {

        if(rightHandJoint == null && leftHandJoint == null 
            && rightFootJoint == null && leftFootJoint == null)
        {
            rightHandJoint = GameObject.FindGameObjectWithTag("RightHandJoint");
            leftHandJoint = GameObject.FindGameObjectWithTag("LeftHandJoint");

            rightFootJoint = GameObject.FindGameObjectWithTag("RightFootJoint");
            leftFootJoint = GameObject.FindGameObjectWithTag("LeftFootJoint");
        }
        else
        {
            Debug.Log("Right hand joint found " + rightHandJoint);
            Debug.Log("Left hand joint found" + leftHandJoint);

            Debug.Log("Right foot joint found " + rightFootJoint);
            Debug.Log("Left foot joint found" + leftFootJoint);
        }

        if (!enableRagDoll)
        {
            bones[0].transform.position = rightHandJoint.transform.position;
            bones[1].transform.position = leftHandJoint.transform.position;

            bones[2].transform.position = rightFootJoint.transform.position;
            bones[3].transform.position = leftFootJoint.transform.position;
        }
    }

    void EnableRagdoll()
    {
        playerRB.freezeRotation = false;
        

        foreach (GameObject bone in bones)
        {
            boneRB = bone.GetComponent<Rigidbody>();
            boneCol = bone.GetComponent<Collider>();
            
            boneRB.isKinematic = false;
            boneRB.useGravity = true;

            boneCol.isTrigger = false;
        }      
    }

    // Temporary
    void OnAnimatorIK()
    {
        if(enableRagDoll)
        {
            EnableRagdoll();

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, bones[0].transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, bones[0].transform.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, bones[1].transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, bones[1].transform.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, bones[2].transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, bones[2].transform.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, bones[3].transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, bones[3].transform.rotation);           
        }
    }
}
