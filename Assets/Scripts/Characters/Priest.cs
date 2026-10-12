using Pickup;
using Player;
using UnityEngine;

namespace Characters
{
    public class Priest : MonoBehaviour
    {
        [SerializeField] private DialogueSpeaker dialogueSpeaker;
        [SerializeField] private int withBeadsSectionIndex;
        [SerializeField] private Item shackKey;
        
        public void GiveBeads()
        {
            SpeakController speakController = FindAnyObjectByType<SpeakController>();
            if (speakController)
            {
                speakController.StartTalkingTo(dialogueSpeaker, withBeadsSectionIndex);
            }
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
