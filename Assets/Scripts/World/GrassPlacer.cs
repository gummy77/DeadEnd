using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class GrassPlacer : MonoBehaviour
    {
        [SerializeField] private Vector2 placementRange;
        [SerializeField] private float density;

        [SerializeField] private GameObject grassPrefab;
        
        void Start()
        {
            for (float indexX = -placementRange.x; indexX < placementRange.x; indexX += density)
            {
                for (float indexY = -placementRange.y; indexY < placementRange.y; indexY += density)
                {
                    Ray rayPos = new Ray(transform.position + new Vector3(indexX + Random.Range(0, density), 20, indexY + Random.Range(0, density)), Vector3.down * 30);
                    if (Physics.SphereCast(rayPos, 0.5f, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (Vector3.Dot(hit.normal, Vector3.up) > 0.99f)
                        {
                            if (hit.collider.gameObject.CompareTag("SpawnsGrass"))
                            {
                                Instantiate(grassPrefab, hit.point, Quaternion.identity, transform);
                            }
                        }
                    }
                } 
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(transform.position, new Vector3(placementRange.x * 2, 0, placementRange.y * 2));
        }
    }
}
