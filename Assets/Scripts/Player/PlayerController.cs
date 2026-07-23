using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerCamera;
    
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    private NetworkTransform _networkTransform;

    private void Start()
    {
        _networkTransform = playerBody.GetComponent<NetworkTransform>();

        if (!IsOwner)
        {
            playerCamera.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        Vector3 movementDelta = _networkTransform.transform.rotation * new Vector3(0, 0, Input.GetAxis("Vertical"));
        _networkTransform.transform.position += movementDelta * (moveSpeed * Time.fixedDeltaTime);
        
        float rotationDelta = Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime;
        _networkTransform.transform.Rotate(Vector3.up, rotationDelta);
    }
}
