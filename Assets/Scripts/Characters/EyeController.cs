using Helper;
using Player;
using UnityEngine;

namespace Characters
{
    public class EyeController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float lockDistance;
        [SerializeField] private float rotationSpeed;
    
        
        private Camera _mainCamera;
        private float _wanderingEyeTimer;
        private Vector3 _lookingAtTarget;
        private Vector3 _defaultPosition;

        private float _bobOffset;
        private bool _isFlying;
    
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
                _defaultPosition = transform.localPosition;
                _bobOffset = Random.Range(-5, 5);
            }
        }

        private void Update()
        {
            if (!_mainCamera)
            {
                SetupCamera();
                return;
            }

            if (!_isFlying)
            {
                transform.localPosition =
                    _defaultPosition + new Vector3(0, Mathf.Sin(Time.time + _bobOffset) * 0.25f, 0);
            }

            Vector3 direction = _lookingAtTarget - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        
            if (Vector3.Distance(_mainCamera.transform.position, transform.position) < lockDistance)
            {
                _lookingAtTarget = _mainCamera.transform.position;
            }
            else
            {
                _wanderingEyeTimer -= Time.deltaTime;
                if (_wanderingEyeTimer < 0f)
                {
                    _lookingAtTarget = transform.position + (transform.forward * Random.Range(0, 2)) + (transform.right * Random.Range(-2, 2)) + (transform.up * Random.Range(0, 2));
                    _wanderingEyeTimer = Random.Range(0, 3);
                }
            }
        }

        public void Found()
        {
            Fly().DiscardAwaitable(nameof(Found));
        }

        private async Awaitable Fly()
        {
            _isFlying = true;
            while (transform.position.y < 10f)
            {
                transform.position += Vector3.up * Time.deltaTime * 3f;
                await Awaitable.EndOfFrameAsync();
            }
            
            Destroy(gameObject);
        }
    }
}
