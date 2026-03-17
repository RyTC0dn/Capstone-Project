using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomAudioPlayer : UnityEvent<Component, AudioClip> { }

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips; //Manually assignable array of audio clips in the inspector
    public AudioSource audioSource;
    private static float lastAudioPlayed;
    [Space(20)]

    [Header("Play settings")]
    public bool playOnAwake;

    [Tooltip("Select one of these options to play single, random, or sequence of clips")]
    public bool playAudio, playRandom, playCycle;
    [Space(10)]

    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;
    public CustomAudioPlayer player;

    private void Update()
    {
        if(playOnAwake)
        {
            if (playAudio) { PlayAudio(minValue, audioSource); }
            else if(playRandom) { PlayRandomClip(audioSource, minValue, maxValue); }
            else if(playCycle) { CycleAudioClips(audioSource); }
        }
    }

    public void PlayAudio(int clipIndex, AudioSource source)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioPlayer.");
            return;
        }


        if (!source.isPlaying) 
        {
            if (Time.time - AudioPlayer.lastAudioPlayed > 0.5f)
            {
                source.clip = clips[clipIndex];
                source.PlayOneShot(source.clip);
            }
        }
        // Move to the next clip index, looping back to the start if necessary
        //currentClipIndex = (currentClipIndex + 1) % clips.Length;
    }

    public void PlayRandomClip(AudioSource source, int minValue, int maxValue)
    {
        // Ensure that the clips array is not empty and that the provided range is valid
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioPlayer.");
            return;
        }

        if (!source.isPlaying)
        {
            if (Time.time - AudioPlayer.lastAudioPlayed > 0.5f)
            {
                source.clip = clips[Random.Range(minValue, maxValue)];
                source.PlayOneShot(source.clip);
            }
        }
    }

    public void CycleAudioClips(AudioSource source)
    {
        // Ensure that the clips array is not empty and that the provided range is valid
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioPlayer.");
            return;
        }

        if (!source.isPlaying)
        {
            if(Time.time - AudioPlayer.lastAudioPlayed > 0.5f)
            {
                minValue = (minValue + 1) % clips.Length;
                source.PlayOneShot(clips[minValue]);
            }
        }
    }

    public void OnAudioEvent(Component sender, AudioClip clip)
    {
        player.Invoke(sender, clip);
    }
}