using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(AudioSource))]
    public class AnimationEventAudioController : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private Vector2 volumeRange;
        [SerializeField] private Vector2 pitchRange;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] audioClips;
    
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudio()
        {
            float volume = Random.Range(volumeRange.x, volumeRange.y);
            float pitch = Random.Range(pitchRange.x, pitchRange.y);
            
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
            
            _audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], volume);
        }
    }
}
