using System;
using Helper;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using World;

namespace Player
{
    public class InteractController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text interactTextObject;
        
        [SerializeField] string interactText;
        
        private Interactor _activeInteractor;
        private InputAction _interactAction;
        
        private void Start()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
        }

        public void Update()
        {
            interactTextObject.gameObject.SetActive(_activeInteractor);
            
            if (_activeInteractor)
            {
                interactTextObject.text = interactText;
                if (_interactAction.WasPressedThisFrame())
                {
                    _activeInteractor.OnInteract();
                }
            }
        }
        
        public void SetInteractor(Interactor interactor)
        {
            _activeInteractor = interactor;
        }
    }
}
