using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    [Serializable]
    public struct SpeakLine
    {
        public string lineText;
        public bool isDefaultColor;
        public Color lineColor;

        public UnityEvent onLineFinished;
    }
    
    [Header("Speak Settings")]
    public SpeakLine[] textToSpeak;
    public SpeakLine[] repeatingTextToSpeak;
    public Color textColor;
    public float speakSpeed = 1f;
    [SerializeField] private Vector3 lookOffset;
    [SerializeField] private AudioClip speakBiteAudioClip;
    
    private AudioSource _audioSource;

    public bool hasSpokenTo;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public Vector3 GetLookPosition()
    {
        return transform.position + lookOffset;
    }

    public void HasSpokenTo()
    {
        hasSpokenTo = true;
    }
    
    public void PlaySpeakBite()
    {
        _audioSource?.PlayOneShot(speakBiteAudioClip);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;
        if (other.CompareTag("Player"))
        {
            SpeakController speakController = other.transform.parent.GetComponent<SpeakController>();

            if (speakController != null)
            {
                speakController.SetSpeaker(this);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!enabled) return;
        if (other.CompareTag("Player"))
        {
            SpeakController speakController = other.transform.parent.GetComponent<SpeakController>();

            if (speakController != null)
            {
                speakController.SetSpeaker(null);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = textColor;
        Gizmos.DrawSphere(GetLookPosition(), 0.25f);
    }
}
