using Pickup;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMouseController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource pickupAudioSource;
        [SerializeField] private AudioClip pickupSound;
        
        private InputAction _clickAction;
        private InputAction _rightClickAction;
        private InputAction _pointAction;
        private InputAction _mouse;
    
        private PlayerInventoryController _playerInventoryController;
        
        private void Start()
        {
            _clickAction = InputSystem.actions.FindAction("Click");
            _rightClickAction = InputSystem.actions.FindAction("RightClick");
            _pointAction = InputSystem.actions.FindAction("Point");
            
            _playerInventoryController = GetComponent<PlayerInventoryController>();
            
            // cursorTransform.gameObject.SetActive(true);
            // Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = false;
        }
        
        private void Update()
        {
            // if (!Cursor.visible)
            // {
            //     cursorTransform.position = _pointAction.ReadValue<Vector2>();
            // }
            
            if (_clickAction.IsPressed())
            {
                // cursorRenderer.sprite = cursorClosed;
                // Cursor.visible = false;

                if (_clickAction.WasPerformedThisFrame())
                {
                    if (Camera.main)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(_pointAction.ReadValue<Vector2>());

                        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
                        {
                            if (hit.transform.CompareTag("Pickup"))
                            {
                                PickupComponent[] pickupComponents = hit.transform.gameObject.GetComponents<PickupComponent>();

                                foreach (PickupComponent pickupComponent in pickupComponents)
                                {
                                    if (pickupComponent)
                                    {
                                        bool success = pickupComponent.DoPickup();
                                        if (pickupComponent.item && _playerInventoryController && success)
                                        {
                                            _playerInventoryController.AddItem(pickupComponent.item,
                                                spawnPoint: _pointAction.ReadValue<Vector2>());
                                        }

                                        if (pickupSound)
                                        {
                                            pickupAudioSource?.PlayOneShot(pickupSound);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (_rightClickAction.IsPressed())
            {
                // cursorRenderer.sprite = cursorLooking;
                // Cursor.visible = false;
            }
            else
            {
                // cursorRenderer.sprite = cursorOpen;
            }
        }
    }
}
