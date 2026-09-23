using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class FishSwimmer : MonoBehaviour
    {
        [SerializeField] private Transform fish;

        [SerializeField] private Vector2 swimRadius;
        
        [SerializeField] private float fishSpeed;
        [SerializeField] private float fishTurnSpeed;
        
        private Vector3 _targetPosition;
        private float _targetTimer;
        
        private void Update()
        {
            if (fish)
            {
                if (_targetTimer <= 0f)
                {
                    _targetTimer = Random.Range(1f, 2f);
                    
                    Vector3 newTarget = Random.insideUnitCircle * Random.Range(swimRadius.x, swimRadius.y);
                    newTarget.z = newTarget.y;
                    newTarget.y = 0;
                    _targetPosition = newTarget;
                }
                _targetTimer -= Time.deltaTime;
                
                fish.rotation = Quaternion.Lerp(fish.rotation, Quaternion.LookRotation(_targetPosition - fish.localPosition), Time.deltaTime * fishTurnSpeed);
                fish.transform.position += fish.forward * (fishSpeed * Time.deltaTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, swimRadius.x);
            Gizmos.DrawWireSphere(transform.position, swimRadius.y);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + _targetPosition, 0.25f);
        }
    }
}
