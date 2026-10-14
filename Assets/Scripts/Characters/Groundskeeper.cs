using Pickup;
using Player;
using UnityEngine;

public class Groundskeeper : MonoBehaviour
{
    [SerializeField] private Item keys;
    
    [SerializeField] private int withPlanksSectionIndex;
    [SerializeField] private int withNailsSectionIndex;
    [SerializeField] private int withBothSectionIndex;

    private bool _hasPlanks;
    private bool _hasNails;
    
    public void GiveKeys()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController)
        {
            playerController.inventoryController.AddItem(keys);
        }
    }

    public void ReceivePlanks()
    {
        
        SpeakController speakController = FindAnyObjectByType<SpeakController>();
        if (speakController)
        {
            _hasPlanks = true;
            if (!_hasNails)
            {
                speakController.StartTalkingTo(GetComponent<DialogueSpeaker>(), withPlanksSectionIndex);
            }
            else
            {
                speakController.StartTalkingTo(GetComponent<DialogueSpeaker>(), withBothSectionIndex);
            }
        }
    }
    public void ReceiveNails()
    {
        SpeakController speakController = FindAnyObjectByType<SpeakController>();
        if (speakController)
        {
            _hasNails = true;
            if (!_hasPlanks)
            {
                speakController.StartTalkingTo(GetComponent<DialogueSpeaker>(), withNailsSectionIndex);
            }
            else
            {
                speakController.StartTalkingTo(GetComponent<DialogueSpeaker>(), withBothSectionIndex);
            }
        }
    }
}
