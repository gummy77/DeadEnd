using UnityEngine;

namespace World
{
    public class MeatPlant : MonoBehaviour
    {
        
        [SerializeField] private GameObject plant;
        [SerializeField] private ParticleSystem cutParticles;
        [SerializeField] private Collider plantCollider;
        [SerializeField] private AudioSource audioSource;
        
        public void Cut()
        {
            plant.SetActive(false);
            cutParticles.Play();
            plantCollider.enabled = false;
            audioSource.Play();
        }
    }
}
