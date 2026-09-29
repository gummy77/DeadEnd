using System;
using Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup
{
    public class PickupComponent : MonoBehaviour
    {
        public Item item;
        [SerializeField] private GameObject pickupParticlePrefab;
        [SerializeField] private bool startsPickupable;
        
        [SerializeField] private UnityEvent onPickup;
        
        private bool _hasBeenSetup = false;
        private bool _hasBeenPickedUp;
        protected bool canPickup;

        private void Start()
        {
            SetupVariablesRpc();
        }

        // [Rpc(SendTo.Server)]
        private void SetupVariablesRpc()
        {
            if (!_hasBeenSetup)
            {
                canPickup = startsPickupable;
                _hasBeenSetup = true;
            }
        }

        public bool DoPickup()
        {
            if (!_hasBeenPickedUp && canPickup)
            {
                DoServerPickupRpc();
                
                _hasBeenPickedUp = true;
                onPickup.Invoke();
                return true;
            }

            return false;
        }

        // [Rpc(SendTo.Server)]
        private void DoServerPickupRpc()
        {
            DoClientPickupRpc();
            Destroy(gameObject);
        }
        
        // [Rpc(SendTo.Everyone)]
        private void DoClientPickupRpc()
        {
            if (pickupParticlePrefab)
            {
                Instantiate(pickupParticlePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
