using System;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform trackedTransform;
        [SerializeField] private Camera childCamera;
        [SerializeField] private Camera fogCamera;
    
        [Header("Settings")]
        [SerializeField] private Vector3 offset;

        [SerializeField] private float positionLerpSpeed;
        [SerializeField] private float rotationLerpSpeed;
        
        [SerializeField] private float defaultCameraFOV;
        [SerializeField] private float lookingAtFOV;

        private bool _isCurrentlyLookingAt;
        private Vector3 _lookingAtTarget;
        private int _lockCount;

        private float _activeFOV;
        private Quaternion _defaultRotation;

        private void Start()
        {
            _defaultRotation = childCamera.transform.localRotation;
            _activeFOV = defaultCameraFOV;
        }

        private void FixedUpdate()
        {
            if (_isCurrentlyLookingAt)
            {
                Vector3 direction = _lookingAtTarget - childCamera.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                childCamera.transform.rotation = Quaternion.Lerp(childCamera.transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            }
            else
            {
                childCamera.transform.localRotation = Quaternion.Lerp(childCamera.transform.localRotation, _defaultRotation, rotationLerpSpeed * Time.deltaTime);
                transform.position = trackedTransform.position;
                transform.rotation = trackedTransform.rotation;
            }
            childCamera.fieldOfView = Mathf.Lerp(childCamera.fieldOfView, _activeFOV, Time.deltaTime);
            fogCamera.fieldOfView = Mathf.Lerp(childCamera.fieldOfView, _activeFOV, Time.deltaTime);
        }

        public void StartLookingAt(Vector3 target, int newFOV = -1)
        {
            if (newFOV != -1)
            {
                _activeFOV = newFOV;
            }
            else
            {
                _activeFOV = lookingAtFOV;
            }
            _lockCount++;
            _isCurrentlyLookingAt = true;
            _lookingAtTarget = target;
        }

        public void StopLookingAt()
        {
            _lockCount--;
            if (_lockCount <= 0)
            {
                _activeFOV = defaultCameraFOV;
                _isCurrentlyLookingAt = false;
            }
        }
    }
}
