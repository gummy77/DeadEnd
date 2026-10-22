using System;
using Pickup;
using Player;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    [SerializeField] private Transform planksTransform;
    
    [SerializeField] private int plankDialogueSectionIndex;
    [SerializeField] private Vector3 plankMovePosition; 
    
    [SerializeField] private DialogueSpeaker dialogueSpeaker;
    [SerializeField] private int giveFlowersDialogueSectionIndex;
    
    [SerializeField] private Item strangeSeedItem;
    
    [SerializeField] private int recieveFiveDollarsDialogueSectionIndex;
    
    public void MovePlanks()
    {
        planksTransform.position = plankMovePosition;
        dialogueSpeaker = GetComponent<DialogueSpeaker>();
        if (dialogueSpeaker)
        {
            dialogueSpeaker.MoveDialogueSection(plankDialogueSectionIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(plankMovePosition, 0.5f);
    }

    public void TryGiveFlowers()
    {
        SpeakController speakController = FindAnyObjectByType<SpeakController>();
        if (speakController)
        {
            speakController.StartTalkingTo(dialogueSpeaker, giveFlowersDialogueSectionIndex, false);
        }
    }

    public void ReceiveFiveDollars()
    {
        SpeakController speakController = FindAnyObjectByType<SpeakController>();
        if (speakController)
        {
            speakController.StartTalkingTo(dialogueSpeaker, recieveFiveDollarsDialogueSectionIndex);
        }
    }
    
    public void GivePlayerSeed()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        if (playerController)
        {
            playerController.inventoryController.AddItem(strangeSeedItem);
        }
    }
}
