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
            _playerMovementController = _playerController.MovementController;
        }

        private void Update()
        {
            playerAnimator.SetFloat(Speed, _playerMovementController.Velocity.Value.magnitude);
        }
    }
}
