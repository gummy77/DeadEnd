using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");

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
            playerAnimator.SetFloat(Speed, _playerMovementController.IsGrounded.Value ? _playerMovementController.Velocity.Value.magnitude : 0);
        }
    }
}
