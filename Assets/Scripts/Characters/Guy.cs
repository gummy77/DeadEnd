using System;
using Player;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Characters
{
    public class Guy : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float lockDistance;
        [SerializeField] private float rotationSpeed;

        [SerializeField] private Transform neckTransform;
        
        private Camera _mainCamera;
        private Quaternion _defaultRotation;
        
        private void Awake()
        {
            PlayerController.PlayerSpawned += (PlayerController playerController) =>
            {
                if (playerController.IsOwner)
                {
                    SetupCamera();
                }
            };
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
            
            if (Vector3.Distance(_mainCamera.transform.position, neckTransform.position) < lockDistance)
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
