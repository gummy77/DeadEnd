using Pickup;
using Player;
using UnityEngine;

namespace Characters
{
    public class Scarecrow : MonoBehaviour
    {
        [SerializeField] private Speaker speaker;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        [SerializeField] private GameObject closedBody;
        [SerializeField] private GameObject openedBody;

        [SerializeField] private Item item;
        
        public void CutOpen()
        {
            speaker.enabled = false;
            audioSource.PlayOneShot(audioClip);
            
            closedBody.SetActive(false);
            openedBody.SetActive(true);
            
            PlayerController[] playerControllers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
            foreach (PlayerController playerController in playerControllers)
            {
                if (playerController.IsOwner)
                {
                    playerController.inventoryController.AddItem(item);
                }
            }
        }
    }
}
