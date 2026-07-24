using UnityEngine;

namespace Characters
{
    public class MimicOpener : MonoBehaviour
    {
        [SerializeField] private Transform mimicHeadTransform;
    
        [SerializeField] private float openDistance;
        private Camera _mainCamera;
    
        private void Awake()
        {
            GameManager.LobbyStarted += SetupCamera;
        }

        private void SetupCamera()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_mainCamera) return;
            
            if (Vector3.Distance(_mainCamera.transform.position, transform.position) < openDistance)
            {
                mimicHeadTransform.localRotation = Quaternion.Lerp(mimicHeadTransform.localRotation, Quaternion.Euler(0, 90, -75), Time.deltaTime);
            }
            else
            {
                mimicHeadTransform.localRotation = Quaternion.Lerp(mimicHeadTransform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime);
            }
        }
    }
}
