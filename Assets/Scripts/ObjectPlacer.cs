using UnityEngine;


public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private bool doesRandomlyRotate = false;
    
    void Awake()
    {
        TerrainGenerator.GenerationFinished += Place;
    }

    private void Place()
    {
        Vector3 startVector = transform.position + new Vector3(0, 1, 0);

        if (Physics.Raycast(startVector, Vector3.down * 5, out RaycastHit hit, layerMask))
        {
            transform.position = hit.point;
        }

        if (doesRandomlyRotate)
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
}
