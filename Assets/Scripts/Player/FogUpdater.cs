using UnityEngine;

namespace Player
{
    [ExecuteInEditMode]
    public class FogUpdater : MonoBehaviour
    {
        [SerializeField] private Material material;
        
        void Update()
        {
            if (material)
            {
                material.SetVector("_Player_Position", transform.position);
            }
        }
    }
}
