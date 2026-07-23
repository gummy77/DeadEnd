using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject playerBody;
    
        [Header("Settings")]
        [SerializeField] private float spawnDistance = 5;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 5f;
    
    
        public NetworkVariable<Vector3> Velocity { get; private set; } = new NetworkVariable<Vector3>();
    
        private NetworkTransform _networkTransform;

        private void Start()
        {
            _networkTransform = playerBody.GetComponent<NetworkTransform>();
            
            float angle = Random.Range(0, 360);
            float distance = Random.Range(0, spawnDistance);
            _networkTransform.transform.position = new Vector3(Mathf.Sin(angle) * distance, 3, Mathf.Cos(angle) * distance);
        }

        void FixedUpdate()
        {
            if (!IsOwner) return;
            Velocity.Value = _networkTransform.transform.rotation * new Vector3(0, 0, Input.GetAxis("Vertical"));
            _networkTransform.transform.position += Velocity.Value * (moveSpeed * Time.fixedDeltaTime);
        
            float rotationDelta = Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime;
            _networkTransform.transform.Rotate(Vector3.up, rotationDelta);
        }
    }
}
