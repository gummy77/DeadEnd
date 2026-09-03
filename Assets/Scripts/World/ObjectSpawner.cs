using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSpawn;

        [SerializeField] private Vector2 bounds;

        [SerializeField] private int numberOfObjectsToSpawn;
        
        [SerializeField] private Vector2 scaleRange;

        [SerializeField] private bool doesRandomlyRotate;
        
        public void Start()
        {
            Generate();
        }

        private void Generate()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int index = 0; index < numberOfObjectsToSpawn; index++)
            {
                Vector3 position = transform.position + new Vector3(Random.Range(-bounds.x, bounds.x), 0, Random.Range(-bounds.y, bounds.y));
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                GameObject obj = Instantiate(objectToSpawn, position, doesRandomlyRotate ? rotation : Quaternion.identity, transform);
                obj.transform.localScale = Vector3.one * Random.Range(scaleRange.x, scaleRange.y);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(bounds.x * 2, 0.1f, bounds.y * 2));
        }
    }
}
