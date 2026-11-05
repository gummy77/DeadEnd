using Pickup;
using Player;
using UnityEngine;

public class Infected : MonoBehaviour
{
    [SerializeField] private int cutWithShearsSectionIndex;
    
    public void UseShearsOn()
    {
        SpeakController speakController = FindAnyObjectByType<SpeakController>();
        if (speakController)
        {
            speakController.StartTalkingTo(GetComponent<DialogueSpeaker>(), cutWithShearsSectionIndex);
        }
    }
}
