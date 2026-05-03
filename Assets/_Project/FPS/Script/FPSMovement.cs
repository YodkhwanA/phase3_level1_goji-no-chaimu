using System.Net.NetworkInformation;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;

    [Header("Fall")]
    [SerializeField] private float gravity = -12f;
    [SerializeField] private float initialFallVelocity = -2f;

    [Header("Reference")]
    [SerializeField] private Transform camereTransform;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference gravityAction;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private float _verticalVelocity;


    //public AudioSource AudioFootsteps;
    //public AudioSource LandingAudio;
    //public AudioSource AudioFoley;
    //public AudioClip LandingAudioClip;
    //public AudioClip[] FootstepAudioClips;
    //[Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    //private float _footstepTimer;
    //[SerializeField] private float footstepInterval = 0.5f;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {

        if (moveAction != null)
        {
            moveAction.action.Enable();
            moveAction.action.performed += StoreMovementInput;
            moveAction.action.canceled += StoreMovementInput;
        }
        if (gravityAction != null)
        {
            gravityAction.action.Enable();
            gravityAction.action.performed += Gravity;
        }
    }
    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.performed -= StoreMovementInput;
            moveAction.action.canceled -= StoreMovementInput;
        }
        if (gravityAction != null)
        {
            gravityAction.action.performed -= Gravity;
        }
    }

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;
        HandleMovement();
        HandleGravity();
        //HandleFootsteps();
    }

    private void StoreMovementInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Gravity(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _verticalVelocity = gravity;
        }
    }

    private void HandleGravity()
    {
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = initialFallVelocity;
        }
        _verticalVelocity += gravity * Time.deltaTime;
    }
    
    private void HandleMovement()
    {
        var move = camereTransform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)).normalized;
        var currentSpeed = walkSpeed;
        var finalMove = move * currentSpeed;
        finalMove.y = _verticalVelocity;

        _characterController.Move(finalMove * Time.deltaTime);
    }

    //private void HandleFootsteps()
    //{
    //    float speed = _characterController.velocity.magnitude;

    //    if (_isGrounded && speed > 0.1f)
    //    {
    //        _footstepTimer -= Time.deltaTime;

    //        if (_footstepTimer <= 0f)
    //        {
    //            PlayFootstep();
    //            _footstepTimer = footstepInterval;
    //        }
    //    }
    //}

    //#1 From TPS Code
    //private void OnFootstep(AnimationEvent animationEvent)
    //{
    //    if (animationEvent.animatorClipInfo.weight > 0.5f)
    //    {

    //        if (AudioFootsteps != null)
    //            AudioFootsteps.Play();
    //        if (AudioFoley != null)
    //            AudioFoley.Play();
    //    }
    //}

    //#2
    //private void PlayFootstep()
    //{
    //    Debug.Log("PLAY FOOTSTEP");

    //    if (AudioFootsteps == null)
    //    {
    //        Debug.Log("NO AUDIO SOURCE");
    //        return;
    //    }

    //    if (FootstepAudioClips.Length == 0)
    //    {
    //        Debug.Log("NO CLIP");
    //        return;
    //    }

    //    int index = Random.Range(0, FootstepAudioClips.Length);
    //    AudioFootsteps.PlayOneShot(FootstepAudioClips[index], 1f);
    //}
}
