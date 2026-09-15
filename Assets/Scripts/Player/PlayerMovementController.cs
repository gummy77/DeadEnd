using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject playerBody;
    
        [Header("Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 5f;
        [SerializeField] private float jumpStrength = 5f;
        [SerializeField] private LayerMask groundLayerMask;
    
        [Header("Sounds")]
        [SerializeField] private AudioClip jumpAudioClip;
        
        private bool _isLocked;

        public bool IsGrounded { get; private set; }
        public Vector3 Velocity { get; private set; }
        
        private Rigidbody _rigidbody;
        private AudioSource _jumpAudioSource;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        
        public void Setup()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");

            _rigidbody =  GetComponent<Rigidbody>();
            
            _jumpAudioSource = GetComponent<AudioSource>();
        }

        public void RunFixedUpdate()
        {
            if (_isLocked)
            {
                Velocity = Vector3.zero;
                return;
            }
            
            Velocity = transform.rotation * new Vector3(0, 0, _moveAction.ReadValue<Vector2>().y);
            transform.position += Velocity * (moveSpeed * Time.fixedDeltaTime);
        
            float rotationDelta = _moveAction.ReadValue<Vector2>().x * rotateSpeed * Time.fixedDeltaTime;
            transform.Rotate(Vector3.up, rotationDelta);

            if (!IsGrounded)
            {
                Vector3 rayStart = transform.position + new Vector3(0, 0.1f, 0);
                IsGrounded = Physics.Raycast(rayStart, Vector3.down, 0.125f, groundLayerMask);
            }

            if (_jumpAction.IsPressed() && IsGrounded)
            {
                _rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                PlayJumpSoundRpc();

                IsGrounded = false;
            }
        }

        // [Rpc(SendTo.Everyone)]
        private void PlayJumpSoundRpc()
        {
            _jumpAudioSource.PlayOneShot(jumpAudioClip);
        }

        public void LockMovement()
        {
            _isLocked = true;
        }
        public void UnlockMovement()
        {
            _isLocked = false;
        }
    }
}
