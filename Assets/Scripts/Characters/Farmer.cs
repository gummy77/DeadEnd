using System;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    [SerializeField] private Transform planksTransform;
    
    [SerializeField] private int plankDialogueSectionIndex;
    [SerializeField] private Vector3 plankMovePosition; 
    
    public void MovePlanks()
    {
        planksTransform.position = plankMovePosition;
        DialogueSpeaker dialogueSpeaker = GetComponent<DialogueSpeaker>();
        if (dialogueSpeaker)
        {
            dialogueSpeaker.MoveDialogueSection(plankDialogueSectionIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(plankMovePosition, 0.5f);
    }
}
