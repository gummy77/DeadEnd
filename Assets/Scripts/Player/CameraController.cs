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

        private void FixedUpdate()
        {
            // TODO lerp these
            transform.position = trackedTransform.position;
            transform.rotation = trackedTransform.rotation;
        }
    }
}
