using UnityEngine;

namespace World
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private GameObject engineFumeParticles;
        [SerializeField] private Transform boot;
        [SerializeField] private GameObject item;
        
        public void OpenBoot()
        {
            engineFumeParticles.SetActive(false);
            item.SetActive(true);
            boot.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
        }
    }
}
