using System;
using Pickup;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    public class PlayerDrownController : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryController playerInventoryController;
        [SerializeField] private Item swimmingItem;

        [SerializeField] private GameObject swimmingMask;
        [SerializeField] private GameObject particles;
        
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Volume drownVolume;
        
        [SerializeField] private float waterLevel;
        [SerializeField] private float drownTime;
        
        private float _drownTimer;
        private bool _hasItem;

        private void Update()
        {
            if (playerTransform.position.y < waterLevel)
            {
                particles.SetActive(true);
                
                if (!_hasItem)
                {
                    _drownTimer += Time.deltaTime;
                    
                    if (playerInventoryController.CheckItem(swimmingItem))
                    {
                        _hasItem = true;
                    }
                }
                else
                {
                    swimmingMask.SetActive(true);
                }
            }
            else
            {
                particles.SetActive(false);
                swimmingMask.SetActive(false);
                if (_drownTimer > 0)
                {
                    _drownTimer -= Time.deltaTime * 2f;
                }
            }
            
            drownVolume.weight = _drownTimer / drownTime;
            
            if (_drownTimer > drownTime)
            {
                GameObject respawn = GameObject.Find("Respawn");
                if (respawn)
                {
                    playerTransform.position = respawn.transform.position;
                }
            }
        }
    }
}
