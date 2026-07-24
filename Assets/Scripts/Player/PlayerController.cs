using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(PlayerMovementController))]
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
        
        public PlayerMovementController MovementController { get; private set; }
        public PlayerAnimationController AnimationController { get; private set; }
        public CameraController CameraController { get; private set; }

        public static Action<PlayerController> PlayerSpawned;
        
        private void Awake()
        {
            MovementController = GetComponent<PlayerMovementController>();
            AnimationController = GetComponent<PlayerAnimationController>();
            CameraController = playerCamera.GetComponent<CameraController>();
        }

        private void Start()
        {
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(spawnBoundPosition + (spawnBoundsSize/2), spawnBoundsSize);
        }
    }
}
