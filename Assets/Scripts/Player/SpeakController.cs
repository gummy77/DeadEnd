using System;
using Helper;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class SpeakController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text speakText;
        [SerializeField] private GameObject speakInteractObject;
        
        [Header("Speak Settings")]
        [SerializeField] private float characterSpeed;
        [SerializeField] private float lineWaitTime;
        
        private bool _isSpeaking;
        private Speaker _activeSpeaker;

        private PlayerController _playerController;
        
        private InputAction _interactAction;
        
        private void Start()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
                
            _playerController = GetComponent<PlayerController>();
        }

        public void Update()
        {
            if (!_playerController.IsOwner) return;
            
            speakInteractObject.SetActive(!_isSpeaking && _activeSpeaker);
            
            if (!_isSpeaking)
            {
                if (_activeSpeaker)
                {
                    if (_interactAction.IsPressed())
                    {
                        Speak().DiscardAwaitable(nameof(Speak));
                    }
                }
            }
        }

        private async Awaitable Speak()
        {
            _playerController.movementController.LockMovement();
            _playerController.cameraController.StartLookingAt(_activeSpeaker.GetLookPosition());
            speakText.color = _activeSpeaker.textColor;
            _isSpeaking = true;
            
            Speaker.SpeakLine[] textToSpeak;
            
            if (!_activeSpeaker.hasSpokenTo)
            {
                textToSpeak = _activeSpeaker.textToSpeak;
            }
            else
            {
                textToSpeak = _activeSpeaker.repeatingTextToSpeak;
            }
            
            _activeSpeaker.HasSpokenTo();
            
            for (int lineIndex = 0; lineIndex < textToSpeak.Length; lineIndex++)
            {
                if (!textToSpeak[lineIndex].isDefaultColor)
                {
                    speakText.color = textToSpeak[lineIndex].lineColor;
                }
                for (int characterIndex = 0;
                     characterIndex < textToSpeak[lineIndex].lineText.Length;
                     characterIndex++)
                {
                    speakText.text = textToSpeak[lineIndex].lineText.Substring(0, characterIndex + 1);
                    if (textToSpeak[lineIndex].lineText[characterIndex] != ' ')
                    {
                        _activeSpeaker.PlaySpeakBite();
                    }
                    await Awaitable.WaitForSecondsAsync(characterSpeed * _activeSpeaker.speakSpeed);
                }
                textToSpeak[lineIndex].onLineFinished?.Invoke();
                await Awaitable.WaitForSecondsAsync(lineWaitTime * _activeSpeaker.speakSpeed);
            }

            speakText.text = "";
            _playerController.cameraController.StopLookingAt();
            _playerController.movementController.UnlockMovement();
            _isSpeaking = false;
        }
        
        public void SetSpeaker(Speaker speaker)
        {
            _activeSpeaker = speaker;
        }
    }
}
