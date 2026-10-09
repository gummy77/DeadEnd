using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class DialogueSpeaker : MonoBehaviour
{
    [Serializable]
    public struct DialogueSection
    {
        public string sectionName;
        public DialogueLine[] textToSpeak;
    }
    
    [Serializable]
    public struct DialogueLine
    {
        public string lineText;
        public UnityEvent onLineFinished;
    }
    
    [Header("NPC Settings")]
    public Color dialogueColor = Color.white;
    public float dialogueSpeed = 1f;
    [SerializeField] private Vector3 lookOffset;
    [SerializeField] private AudioClip speakBiteAudioClip;
    
    [Header("Dialogue Settings")]
    public DialogueSection[] dialogueSections;
    public int dialogueSectionIndex = 0;

    [SerializeField] private bool dialogueEnabled = true;
    
    private AudioSource _audioSource;
    private SpeakController _speakController;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public Vector3 GetLookPosition()
    {
        return transform.position + lookOffset;
    }

    public void PlaySpeakBite()
    {
        _audioSource?.PlayOneShot(speakBiteAudioClip);
    }

    public void MoveDialogueSection(int newIndex)
    {
        dialogueSectionIndex = newIndex;
    }

    public void SetEnabled(bool newEnabled)
    {
        dialogueEnabled = newEnabled;
        if (newEnabled == false)
        {
            _speakController.SetSpeaker(null);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!dialogueEnabled) return;
        if (other.CompareTag("Player"))
        {
            _speakController = other.transform.parent.GetComponent<SpeakController>();
        
            if (_speakController != null)
            {
                _speakController.SetSpeaker(this);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!dialogueEnabled) return;
        if (other.CompareTag("Player"))
        {
            _speakController = other.transform.parent.GetComponent<SpeakController>();
        
            if (_speakController != null)
            {
                _speakController.SetSpeaker(null);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = dialogueColor;
        Gizmos.DrawSphere(GetLookPosition(), 0.25f);
    }
}
