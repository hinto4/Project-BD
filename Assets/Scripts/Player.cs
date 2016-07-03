using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private Vector3 _inputValue;
    private myThirdPersonCharacter _characterController;
    private CameraControl _cameraControl;

    private bool Jump;

    private RagdollSystem _ragdollSystem;
    private GameManager _gameManager;

    void Start()
    {
        _ragdollSystem = GetComponent<RagdollSystem>();
        _characterController = GetComponent<myThirdPersonCharacter>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (this.transform.position.y <= -50) // Temporary reset player position when player falls from platvorm.
        {
            transform.position = new Vector3(0, 0, -24);
        }

        if(!Jump)
        {
            Jump = CrossPlatformInputManager.GetButtonDown("Jump");

            // If player is on the ground let him change ragdoll state and use jump.
            if (_ragdollSystem.ragdoll && Jump)
            {
                _ragdollSystem.ragdoll = false;
            }
        }
    }

    void FixedUpdate ()
    {
        if (!isLocalPlayer)
            return;

        // If player is in standing position, match started, allow movement.
        if (!_ragdollSystem.ragdoll && _gameManager.GameState() 
            == GameManager.GameStates.Match_Started)       
        {
            _inputValue.x = CrossPlatformInputManager.GetAxis("Horizontal");
            _inputValue.z = CrossPlatformInputManager.GetAxis("Vertical");           
        }

        Vector3 characterMovement = _inputValue;

        _characterController.Move(characterMovement, false, Jump);

        Jump = false;
    }

    public override void OnStartLocalPlayer()
    {
        _cameraControl = FindObjectOfType<CameraControl>();
        _cameraControl.StartCamera();
        _cameraControl.FollowTarget(this.transform.gameObject);
    }
}
