using System;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform trackedTransform;
        [SerializeField] private Camera childCamera;
    
        [Header("Settings")]
        [SerializeField] private Vector3 offset;

        [SerializeField] private float positionLerpSpeed;
        [SerializeField] private float rotationLerpSpeed;
        
        [SerializeField] private float defaultCameraFOV;
        [SerializeField] private float lookingAtFOV;

        private bool _isCurrentlyLookingAt;
        private Vector3 _lookingAtTarget;
        
        private Quaternion _defaultRotation;

        private void Start()
        {
            _defaultRotation = childCamera.transform.localRotation;
        }

        private void FixedUpdate()
        {
            if (_isCurrentlyLookingAt)
            {
                Vector3 direction = _lookingAtTarget - childCamera.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                childCamera.transform.rotation = Quaternion.Lerp(childCamera.transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
                childCamera.fieldOfView = Mathf.Lerp(childCamera.fieldOfView, lookingAtFOV, Time.deltaTime);
            }
            else
            {
                childCamera.transform.localRotation = Quaternion.Lerp(childCamera.transform.localRotation, _defaultRotation, rotationLerpSpeed * Time.deltaTime);
                transform.position = trackedTransform.position;
                transform.rotation = trackedTransform.rotation;
                childCamera.fieldOfView = Mathf.Lerp(childCamera.fieldOfView, defaultCameraFOV, Time.deltaTime);
            }
        }

        public void StartLookingAt(Vector3 target)
        {
            _isCurrentlyLookingAt = true;
            _lookingAtTarget = target;
        }

        public void StopLookingAt()
        {
            _isCurrentlyLookingAt = false;
        }
    }
}
