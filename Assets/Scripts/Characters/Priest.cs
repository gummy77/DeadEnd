using Pickup;
using Player;
using UnityEngine;

namespace Characters
{
    public class Priest : MonoBehaviour
    {
        [SerializeField] private Speaker preBeadSpeak;
        [SerializeField] private Speaker postBeadSpeak;

        [SerializeField] private Item shackKey;
        
        public void GiveBeads()
        {
            preBeadSpeak.enabled = false;
            postBeadSpeak.enabled = true;
        }

        public void GivePlayerKey()
        {
            PlayerController playerController = FindAnyObjectByType<PlayerController>();
            if (playerController)
            {
                playerController.inventoryController.AddItem(shackKey);
            }
        }
    }
}
