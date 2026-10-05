using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace World
{
    public class MeatPlant : MonoBehaviour
    {
        [SerializeField] private GameObject[] plants;
        [SerializeField] private ParticleSystem cutParticles;
        [SerializeField] private Collider plantCollider;
        [SerializeField] private AudioSource audioSource;

        private int _chosenPlant;
        
        private void Start()
        {
           Grow();
           MeatPlantManager.MeatPlantInit(this);
        }

        public void Grow()
        {
            _chosenPlant = Random.Range(0, plants.Length);
            for (int index = 0; index < plants.Length; index++)
            {
                plants[index].SetActive(index == _chosenPlant);
            }
            transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            plantCollider.enabled = false;
        }
        
        public void Cut()
        {
            MeatPlantManager.MeatPlantCut(this);
            plants[_chosenPlant].SetActive(false);
            cutParticles.Play();
            plantCollider.enabled = false;
            audioSource.Play();
        }
    }
}
