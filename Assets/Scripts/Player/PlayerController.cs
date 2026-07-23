using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerAnimationController))]
    // [RequireComponent(typeof())]
    public class PlayerController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject playerBody;
        [SerializeField] private GameObject playerCamera;
    
        [Header("Player Settings")]

        public PlayerMovementController MovementController { get; private set; }
        public PlayerAnimationController AnimationController { get; private set; }

        private void Awake()
        {
            MovementController = GetComponent<PlayerMovementController>();
            AnimationController = GetComponent<PlayerAnimationController>();
        }

        private void Start()
        {
            if (!IsOwner)
            {
                playerCamera.SetActive(false);
            }
        }
    }
}
