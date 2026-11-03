using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        [Header("References")]
        [SerializeField] private Animator playerAnimator;
    
        private PlayerController _playerController;

        private PlayerMovementController _playerMovementController;
        
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _playerMovementController = _playerController.movementController;
        }

        private void Update()
        {
            float horizontal = (new Vector3(_playerMovementController.Velocity.x, 0, _playerMovementController.Velocity.z)).magnitude;
            float vertical = _playerMovementController.rb.linearVelocity.y;
            
            playerAnimator.SetBool(IsWalking, horizontal >= 0.1f);
            playerAnimator.SetFloat(VerticalSpeed, vertical);
            playerAnimator.SetBool(IsGrounded, _playerMovementController.IsGrounded);
        }
    }
}
