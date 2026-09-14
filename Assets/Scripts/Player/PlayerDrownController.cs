using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    public class PlayerDrownController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Volume drownVolume;
        
        [SerializeField] private float waterLevel;
        [SerializeField] private float drownTime;
        
        private float _drownTimer;

        private void Update()
        {
            if (playerTransform.position.y < waterLevel)
            {
                _drownTimer += Time.deltaTime;
            }
            else
            {
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
