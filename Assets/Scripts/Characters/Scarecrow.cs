using Helper;
using Pickup;
using Player;
using UnityEngine;

namespace Characters
{
    public class Scarecrow : MonoBehaviour
    {
        [SerializeField] private Speaker speaker;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip screamClip;
        [SerializeField] private AudioClip snipClip;

        [SerializeField] private GameObject closedBody;
        [SerializeField] private GameObject openedBody;

        [SerializeField] private Item item;
        
        public void CutOpen()
        {
            speaker.enabled = false;
            audioSource.PlayOneShot(screamClip);
            
            DoCut().DiscardAwaitable(nameof(CutOpen));
        }

        public async Awaitable DoCut()
        {
            await Awaitable.WaitForSecondsAsync(0.5f);
            
            audioSource.PlayOneShot(snipClip);
            closedBody.SetActive(false);
            openedBody.SetActive(true);
            
            await Awaitable.WaitForSecondsAsync(0.5f);
            
            PlayerController playerController = FindAnyObjectByType<PlayerController>();
            if (playerController)
            {
                playerController.inventoryController.AddItem(item);
            }
        }
    }
}
