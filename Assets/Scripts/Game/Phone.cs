using Helper;
using Player;
using UnityEngine;

namespace Game
{
    public class Phone : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerController  playerController;
        
        public void Answer()
        {
            AnswerCall().DiscardAwaitable(nameof(AnswerCall));
        }
        
        public void Return()
        {
            ReturnCall().DiscardAwaitable(nameof(ReturnCall));
        }

        private async Awaitable AnswerCall()
        {
            playerTransform.position +=  Vector3.up * 50;
            playerController.movementController.LockMovement();

            await Awaitable.WaitForSecondsAsync(6);
            
            playerController.movementController.UnlockMovement();
        }
        
        private async Awaitable ReturnCall()
        {
            playerTransform.position += transform.rotation * new Vector3(0, -50, -77.5f);
            playerTransform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}
