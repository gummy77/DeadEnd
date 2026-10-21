using Helper;
using Player;
using UnityEngine;

namespace Game
{
    public class Phone : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerController  playerController;
        [SerializeField] private AudioSource phoneAudioSource;

        [SerializeField] private AudioClip callAudio;

        [SerializeField] private Transform phoneTransform;
        
        private Vector3 phoneStartPosition;
        private Quaternion phoneStartRotation;
        
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
            phoneAudioSource.Stop();
            phoneAudioSource.PlayOneShot(callAudio);
            playerController.movementController.LockMovement();

            phoneStartPosition = phoneTransform.position;
            phoneStartRotation = phoneTransform.rotation;
            
            float timer = 0;

            while (timer < 1f)
            {
                timer += Time.deltaTime;

                phoneTransform.position = Vector3.Lerp(phoneStartPosition, playerTransform.position + (playerTransform.rotation * new Vector3(0.5f, 1f, 0f)), timer);
                phoneTransform.rotation = Quaternion.Lerp(phoneStartRotation, Quaternion.Euler(90, 0, 0) , timer);
                
                await Awaitable.EndOfFrameAsync();
            }
            
            await Awaitable.WaitForSecondsAsync(10);
            
            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                phoneTransform.position = Vector3.Lerp(phoneStartPosition, playerTransform.position + (playerTransform.rotation * new Vector3(0.5f, 1f, 0f)), timer);
                phoneTransform.rotation = Quaternion.Lerp(phoneStartRotation, Quaternion.Euler(90, 0, 0) , timer);
                
                await Awaitable.EndOfFrameAsync();
            }
            
            await Awaitable.WaitForSecondsAsync(1.5f);
            
            playerTransform.position +=  Vector3.up * 50;
            
            playerController.movementController.UnlockMovement();
        }
        
        private async Awaitable ReturnCall()
        {
            playerTransform.position += transform.rotation * new Vector3(0, -50, -77.5f);
            playerTransform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}
