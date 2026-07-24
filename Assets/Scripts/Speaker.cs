using System;
using Player;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    [Header("Speak Settings")]
    public string[] textToSpeak;
    public Color textColor;
    public float speakSpeed = 1f;
    [SerializeField] private Vector3 lookOffset;
    [SerializeField] private AudioClip speakBiteAudioClip;
    
    private AudioSource _audioSource;

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
    
    private void OnTriggerStay(Collider other)
    {
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
