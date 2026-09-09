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
        // [SerializeField] private Transform cursorTransform;
        // [SerializeField] private Image cursorRenderer;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject clickParticlesPrefab;
        
        // [Header("Textures")]
        // [SerializeField] private Sprite cursorOpen;
        // [SerializeField] private Sprite cursorClosed;
        // [SerializeField] private Sprite cursorLooking;
        
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

                        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
                        {
                            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                            {
                                SpawnClickParticlesRpc(hit.point, hit.transform.rotation);
                            }
                            if (hit.transform.CompareTag("Pickup"))
                            {
                                PickupComponent pickupComponent = hit.transform.gameObject.GetComponent<PickupComponent>();

                                if (pickupComponent)
                                {
                                    bool success = pickupComponent.DoPickup();
                                    if (pickupComponent.item && _playerInventoryController && success)
                                    {
                                        _playerInventoryController.AddItem(pickupComponent.item);
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
        
        [Rpc(SendTo.Everyone)]
        private void SpawnClickParticlesRpc(Vector3 hitPosition, Quaternion hitRotation)
        {
            Instantiate(clickParticlesPrefab, hitPosition, hitRotation);
        }
    }
}
