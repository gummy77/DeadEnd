using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(PlayerAnimationController))]
    [RequireComponent(typeof(SpeakController))]
    // [RequireComponent(typeof())]
    public class PlayerController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject playerBody;
        [SerializeField] private GameObject playerCamera;
        
        [Header("Player Settings")]
        [SerializeField] private Vector3 spawnBoundPosition;
        [SerializeField] private Vector3 spawnBoundsSize;

        public PlayerMovementController movementController;
        public PlayerAnimationController animationController;
        public PlayerInventoryController inventoryController;
        public CameraController cameraController;

        public static Action<PlayerController> PlayerSpawned;
        
        private void Awake()
        {
            animationController = GetComponent<PlayerAnimationController>();
            cameraController = playerCamera.GetComponent<CameraController>();
            inventoryController = GetComponent<PlayerInventoryController>();
        }

        private void Start()
        {
            movementController.Setup();
            
            if (!IsOwner)
            {
                playerCamera.SetActive(false);
            }
            else
            {
                playerBody.transform.position = spawnBoundPosition + new Vector3(Random.Range(0,  spawnBoundsSize.x), 0, Random.Range(0, spawnBoundsSize.y));
                PlayerSpawned?.Invoke(this);
            }
        }

        private void FixedUpdate()
        {
            movementController.RunFixedUpdate();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(spawnBoundPosition + (spawnBoundsSize/2), spawnBoundsSize);
        }
    }
}
