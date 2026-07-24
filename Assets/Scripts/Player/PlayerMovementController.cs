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
        
        private bool _isLocked = false;

        public NetworkVariable<bool> IsGrounded { get; private set; } = new NetworkVariable<bool>();
        public NetworkVariable<Vector3> Velocity { get; private set; } = new NetworkVariable<Vector3>();
    
        
        private NetworkTransform _networkTransform;
        private NetworkRigidbody _networkRigidbody;
        private AudioSource _jumpAudioSource;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        
        private void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            
            _networkTransform = playerBody.GetComponent<NetworkTransform>();
            _networkRigidbody =  playerBody.GetComponent<NetworkRigidbody>();
                
            _jumpAudioSource = playerBody.GetComponent<AudioSource>();
        }

        void FixedUpdate()
        {
            if (!IsOwner) return;
            if (_isLocked)
            {
                Velocity.Value = Vector3.zero;
                return;
            }
            Velocity.Value = _networkTransform.transform.rotation * new Vector3(0, 0, _moveAction.ReadValue<Vector2>().y);
            _networkTransform.transform.position += Velocity.Value * (moveSpeed * Time.fixedDeltaTime);
        
            float rotationDelta = _moveAction.ReadValue<Vector2>().x * rotateSpeed * Time.fixedDeltaTime;
            _networkTransform.transform.Rotate(Vector3.up, rotationDelta);

            if (!IsGrounded.Value)
            {
                Vector3 rayStart = _networkTransform.transform.position + new Vector3(0, 0.1f, 0);
                IsGrounded.Value = Physics.Raycast(rayStart, Vector3.down, 0.125f, groundLayerMask);
            }

            if (_jumpAction.IsPressed() && IsGrounded.Value)
            {
                _networkRigidbody.Rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
                PlayJumpSoundRpc();

                IsGrounded.Value = false;
            }
        }

        [Rpc(SendTo.Everyone)]
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
