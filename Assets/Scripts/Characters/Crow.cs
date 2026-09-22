using System;
using Helper;
using UnityEngine;

namespace Characters
{
    public class Crow : MonoBehaviour
    {

        [SerializeField] private GameObject crowBody;
        [SerializeField] private GameObject feather;
        
        [SerializeField] private AudioSource audioSource;

        private bool _hasSquawked;
        
        public async Awaitable Squawk()
        {
            _hasSquawked = true;
            audioSource.Play();
            
            feather?.SetActive(true);
            
            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime;

                crowBody.transform.position += new Vector3(0f, Time.deltaTime * 6, 0f);
                
                await Awaitable.NextFrameAsync();
            }
            
            crowBody?.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_hasSquawked)
            {
                if (other.tag == "Player")
                {
                    Squawk().DiscardAwaitable(nameof(OnTriggerEnter));
                }
            }
        }
    }
}
