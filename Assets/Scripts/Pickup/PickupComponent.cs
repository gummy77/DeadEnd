using System;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace Pickup
{
    public class PickupComponent : MonoBehaviour
    {
        public Item item;
        [SerializeField] private GameObject pickupParticlePrefab;
        [SerializeField] private bool startsPickupable;
        
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
