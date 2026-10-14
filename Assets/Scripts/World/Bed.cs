using System;
using Helper;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class Bed : MonoBehaviour
    {
        [SerializeField] private float sleepTime = 2f;
        [SerializeField] private float jumpHeight = 0.75f;
        
        [SerializeField] private TMP_Text dayCounterText;
        [SerializeField] private ParticleSystem sleepingParticles;
        [SerializeField] private Transform sleepingGooberTransform;
        [SerializeField] private Transform gooberModelTransform;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Vector3 lookAtPosition;

        [SerializeField] private UnityEvent onDayEnded;

        private int _dayCounter = 1;
        private Vector3 _bodyStartPosition;
        private Quaternion _bodyStartRotation;
        
        public void StartSleep()
        {
            GetComponent<Interactor>().SetInteractable(false);
            DoSleep().DiscardAwaitable(nameof(DoSleep));
        }

        public async Awaitable DoSleep()
        {
            _bodyStartPosition = gooberModelTransform.position;
            _bodyStartRotation = gooberModelTransform.rotation;

            playerController.movementController.LockMovement();
            playerController.cameraController.StartLookingAt(transform.position + lookAtPosition);
            
            float timer = 0;

            while (timer < 1f)
            {
                timer += Time.deltaTime;
                gooberModelTransform.position = Vector3.Lerp(_bodyStartPosition, sleepingGooberTransform.position, timer);
                gooberModelTransform.position += new Vector3(0, (1 - (Mathf.Abs(timer - 0.5f) * 2)) * jumpHeight, 0);
                
                gooberModelTransform.rotation = Quaternion.Lerp(_bodyStartRotation, sleepingGooberTransform.rotation, timer);
                await Awaitable.NextFrameAsync();
            }
            
            sleepingParticles.Play();
            _dayCounter += 1;
            dayCounterText.text = "Day " + _dayCounter;
            dayCounterText.gameObject.SetActive(true);
            
            await Awaitable.WaitForSecondsAsync(sleepTime);
            
            sleepingParticles.Stop();
            onDayEnded.Invoke();
            dayCounterText.gameObject.SetActive(false);

            
            playerController.cameraController.StopLookingAt();
            
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                gooberModelTransform.position = Vector3.Lerp(_bodyStartPosition, sleepingGooberTransform.position, timer);
                gooberModelTransform.position += new Vector3(0, (1 - (Mathf.Abs(timer - 0.5f) * 2)) * jumpHeight, 0);
                gooberModelTransform.rotation = Quaternion.Lerp(_bodyStartRotation, sleepingGooberTransform.rotation, timer);
                await Awaitable.NextFrameAsync();
            }
            
            playerController.movementController.UnlockMovement();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + lookAtPosition, 0.25f);
        }
    }
}
