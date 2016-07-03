using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RagdollSystem : MonoBehaviour
{
    float ragdollEndTime = -100;



    // How long do we blend when transitioning from ragdolled to animated.
    public float ragdollToMecanimBlendTime = 0.5f;
    float mecanimToGetUpTransitionTime = 0.05f;

    public bool ragdoll
    {
        get { return state != RagdollState.animated; }

        set
        {
            if(value == true)
            {
                RootKinematics(true);                   // Kinematics enabled, collision disabled as we're on ragdolled state.  
                ragdollKinematics(false);
                animator.enabled = false;
                state = RagdollState.ragdollEnabled;
            }
            else
            {
                if(state == RagdollState.ragdollEnabled)
                {
                    RootKinematics(false);              // Kinematics disabled, collision enabled, as we're back to animated state.    
                    ragdollKinematics(true);
                    ragdollEndTime = Time.time;
                    animator.enabled = true;
                    state = RagdollState.blendToAnim;

                    // Store the ragdoll position for blending.
                    foreach(BodyPart bodyPart in bodyParts)
                    {
                        bodyPart.storedPosition = bodyPart.transform.position;
                        bodyPart.storedRotation = bodyPart.transform.rotation;
                    }

                    // remember some key positions
                    ragdollFeetPos = 0.5f * (animator.GetBoneTransform(HumanBodyBones.LeftToes).position 
                        + animator.GetBoneTransform(HumanBodyBones.RightToes).position);
                    ragdollHeadPos = animator.GetBoneTransform(HumanBodyBones.Head).position;
                    ragdollHipPos = animator.GetBoneTransform(HumanBodyBones.Hips).position;

                    // Initiate the get up animation
                    // Hips forward vector pointing upwards. 
                    if (animator.GetBoneTransform(HumanBodyBones.Hips).forward.y > 0)   
                    {
                        animator.SetBool("StandUpFromBack", true);                        
                    }
                    else
                    {
                        animator.SetBool("StandUpFromBelly", true);
                    }
                }              
            }
        }
    }

    public class BodyPart
    {
        public Transform transform;
        public Vector3 storedPosition;
        public Quaternion storedRotation;
    }

    // Declares list of bodyParts, initialized in Start();
    List<BodyPart> bodyParts = new List<BodyPart>();

    // stores poses of ragdoll that ended up with.
    Vector3 ragdollHipPos, ragdollHeadPos, ragdollFeetPos;

    private Animator animator;

    enum RagdollState
    {
        animated,               // Mecanim is enabled / player animator enabled.
        ragdollEnabled,         // Physics affect the ragdoll, animator disabled.
        blendToAnim             // Enabling the animator and restoring last ragdolled position.
    }

    // Current start state of ragdoll
    RagdollState state = RagdollState.animated;

    void Start()
    {
        animator = GetComponent<Animator>();

        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rigidBodies)
        {
            HitPhysics hitPhysics = rb.gameObject.AddComponent<HitPhysics>();
            hitPhysics.ragdollSystem = this;
        }

        foreach(Component c in GetComponentsInChildren(typeof(Collider)))
        {
            if(c != this.GetComponent<Collider>())
            {
                (c as Collider).isTrigger = true;
            }           
        }

        // Stores transform for each of objects with transform.
        foreach(Component c in GetComponentsInChildren(typeof(Transform)))
        {
            BodyPart bodyPart = new BodyPart();
            bodyPart.transform = c as Transform;
            bodyParts.Add(bodyPart);            
        }
    }

    void LateUpdate()
    {
        animator.SetBool("StandUpFromBack", false);
        animator.SetBool("StandUpFromBelly", false);

        if(state == RagdollState.blendToAnim)
        {
            if(Time.time <= ragdollEndTime + mecanimToGetUpTransitionTime)
            {
                // Get the best root position that matches with ragdoll.
                // Player position will be set by hips position, playing standup animation from.
                Vector3 animatedToRagdolled = ragdollHipPos - animator.GetBoneTransform(HumanBodyBones.Hips).position;
                Vector3 newRootPosition = transform.position + animatedToRagdolled;

                // Cast a ray from the found root position downwards and find the highest hit that does not belong to character.
                RaycastHit[] hits = Physics.RaycastAll(new Ray(newRootPosition, Vector3.down));
                newRootPosition.y = 0;
                foreach (RaycastHit hit in hits)
                {
                    if (!hit.transform.IsChildOf(transform))
                    {
                        newRootPosition.y = Mathf.Max(newRootPosition.y, hit.point.y);
                    }
                }
                transform.position = newRootPosition;


                Vector3 ragdolledDirection = ragdollHeadPos - ragdollFeetPos;
                ragdolledDirection.y = 0;

                Vector3 meanFeetPosition = 0.5f * (animator.GetBoneTransform(HumanBodyBones.LeftFoot).position 
                    + animator.GetBoneTransform(HumanBodyBones.RightFoot).position);

                Vector3 animatedDirection = animator.GetBoneTransform(HumanBodyBones.Head).position - meanFeetPosition;
                animatedDirection.y = 0;

                transform.rotation *= Quaternion.FromToRotation(animatedDirection.normalized, ragdolledDirection.normalized);
            }

            float ragdollBlendAmount = 1.0f - (Time.time - ragdollEndTime - mecanimToGetUpTransitionTime) / ragdollToMecanimBlendTime;
            ragdollBlendAmount = Mathf.Clamp01(ragdollBlendAmount);

            foreach (BodyPart b in bodyParts)
            {
                if (b.transform != transform)
                { 
                    if (b.transform == animator.GetBoneTransform(HumanBodyBones.Hips))
                        b.transform.position = Vector3.Lerp(b.transform.position, b.storedPosition, ragdollBlendAmount);

                    b.transform.rotation = Quaternion.Slerp(b.transform.rotation, b.storedRotation, ragdollBlendAmount);
                }
            }

            if (ragdollBlendAmount == 0)
            {
                state = RagdollState.animated;
                return;
            }
        }
    }

    // Controlling root kinematics and collision separately as player movement requires it's own physic components.
    void RootKinematics(bool value)
    {
        if(value)
        {
            this.transform.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            this.transform.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.GetComponent<Collider>().isTrigger = false;
        }
    }

    void ragdollKinematics(bool value)
    {
        Component[] components = GetComponentsInChildren(typeof(Rigidbody));

        foreach (Component c in components)
        {
            if (c != this.GetComponent<Rigidbody>()) 
            {
                (c as Rigidbody).isKinematic = value;
            }
                    
        }
        // Temporary
        foreach (Component c in GetComponentsInChildren(typeof(Collider)))
        {
            if (c != this.GetComponent<Collider>())
            {
                (c as Collider).isTrigger = value;
            }
        }
    }
}
