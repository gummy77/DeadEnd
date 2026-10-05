using System;
using Pickup;
using Player;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Characters
{
    public class Guy : MonoBehaviour
    {
        [SerializeField] private GameObject meatCube;
        [SerializeField] private GameObject meatCubePartial;
        [SerializeField] private DialogueSpeaker speaker;
        [SerializeField] private Transform neckTransform;

        [SerializeField] private Renderer faceRenderer;
        [SerializeField] private Material faceMatEaten;
        [SerializeField] private int meatEatenDialogueSection;
        
        
        [SerializeField] private Item beads;
        
        [Header("Settings")]
        [SerializeField] private float lockDistance;
        [SerializeField] private float rotationSpeed;

        
        private Camera _mainCamera;
        private Quaternion _defaultRotation;
        private bool _hasDinner;
        private DialogueSpeaker _speaker;
        
        private void Awake()
        {
            _speaker= GetComponent<DialogueSpeaker>();
            PlayerController.PlayerSpawned += (PlayerController playerController) =>
            {
                SetupCamera();
            };
        }

        public void EatMeat()
        {
            meatCubePartial.SetActive(true);
            meatCube.SetActive(false);
            faceRenderer.material = faceMatEaten;
            _speaker.MoveDialogueSection(meatEatenDialogueSection);
        }

        public void GiveDinner()
        {
            meatCube.SetActive(true);
            speaker.SetEnabled(true);
            _hasDinner = true;
        }

        public void GiveBeads()
        {
            PlayerController playerController = FindAnyObjectByType<PlayerController>();
            if (playerController)
            {
                playerController.inventoryController.AddItem(beads);
            }
        }
        
        private void SetupCamera()
        {
            if (Camera.main != null)
            {
                _mainCamera = Camera.main;
                _defaultRotation = transform.localRotation;
            }
        }
        
        private void Update()
        {
            if (!_mainCamera)
            {
                SetupCamera();
                return;
            }
            
            if (Vector3.Distance(_mainCamera.transform.position, neckTransform.position) < lockDistance && !_hasDinner)
            {
                Vector3 direction = _mainCamera.transform.position - neckTransform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                neckTransform.rotation = Quaternion.Lerp(neckTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                neckTransform.localRotation = Quaternion.Lerp(neckTransform.localRotation, _defaultRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
