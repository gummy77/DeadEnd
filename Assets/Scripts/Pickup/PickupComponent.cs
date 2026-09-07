using System;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace Pickup
{
    public class PickupComponent : NetworkBehaviour
    {
        public Item item;
        [SerializeField] private GameObject pickupParticlePrefab;
        [SerializeField] private bool startsPickupable;

        private bool _hasBeenSetup = false;
        private bool _hasBeenPickedUp;
        [SerializeField]
        protected readonly NetworkVariable<bool> CanPickup = new NetworkVariable<bool>(writePerm:NetworkVariableWritePermission.Server);

        private void Start()
        {
            SetupVariablesRpc();
        }

        [Rpc(SendTo.Server)]
        private void SetupVariablesRpc()
        {
            if (!_hasBeenSetup)
            {
                CanPickup.Value = startsPickupable;
                _hasBeenSetup = true;
            }
        }

        public bool DoPickup()
        {
            if (!_hasBeenPickedUp && CanPickup.Value)
            {
                DoServerPickupRpc();
                
                _hasBeenPickedUp = true;
                return true;
            }

            return false;
        }

        [Rpc(SendTo.Server)]
        private void DoServerPickupRpc()
        {
            DoClientPickupRpc();
            Destroy(gameObject);

        }
        
        [Rpc(SendTo.Everyone)]
        private void DoClientPickupRpc()
        {
            if (pickupParticlePrefab)
            {
                Instantiate(pickupParticlePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
