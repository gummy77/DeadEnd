using UnityEngine;

namespace Player
{
    public class FogUpdater : MonoBehaviour
    {
        [SerializeField] private Material material;
        
        void Update()
        {
            material.SetVector("_Player_Position", transform.position);
        }
    }
}
