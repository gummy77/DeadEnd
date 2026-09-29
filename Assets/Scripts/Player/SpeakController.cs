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
        private DialogueSpeaker _activeSpeaker;

        private PlayerController _playerController;
        
        private InputAction _interactAction;
        
        private void Start()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
            _playerController = GetComponent<PlayerController>();
        }

        public void Update()
        {
            speakInteractObject.SetActive(!_isSpeaking && _activeSpeaker);
            
            if (!_isSpeaking)
            {
                if (_activeSpeaker)
                {
                    if (_interactAction.WasPressedThisFrame())
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
            speakText.color = _activeSpeaker.dialogueColor;
            _isSpeaking = true;

            await Awaitable.WaitForSecondsAsync(0.1f);
            
            DialogueSpeaker.DialogueSection dialogueSection = _activeSpeaker.dialogueSections[_activeSpeaker.dialogueSectionIndex];
            
            for (int lineIndex = 0; lineIndex < dialogueSection.textToSpeak.Length; lineIndex++)
            {
                DialogueSpeaker.DialogueLine dialogueLine = dialogueSection.textToSpeak[lineIndex];
                speakText.color = _activeSpeaker.dialogueColor;

                for (int characterIndex = 0;
                     characterIndex < dialogueLine.lineText.Length;
                     characterIndex++)
                {
                    speakText.text = dialogueLine.lineText.Substring(0, characterIndex + 1);
                    if (dialogueLine.lineText[characterIndex] != ' ')
                    {
                        _activeSpeaker.PlaySpeakBite();
                    }


                    await Awaitable.WaitForSecondsAsync(characterSpeed * _activeSpeaker.dialogueSpeed * (_interactAction.IsPressed() ? 0.25f : 1f));

                }
                dialogueLine.onLineFinished?.Invoke();
                await Awaitable.WaitForSecondsAsync(lineWaitTime * (_interactAction.IsPressed() ? 0.5f : 1f));
            }

            speakText.text = "";
            _playerController.cameraController.StopLookingAt();
            _playerController.movementController.UnlockMovement();
            _isSpeaking = false;
        }
        
        public void SetSpeaker(DialogueSpeaker speaker)
        {
            _activeSpeaker = speaker;
        }
    }
}
