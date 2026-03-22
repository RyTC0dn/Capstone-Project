using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips; //Manually assignable array of audio clips in the inspector
    private static float lastAudioPlayed;

    public void PlayAudio(int clipIndex, AudioSource source)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioPlayer.");
            return;
        }

        if (!source.isPlaying) //Prevent overlapping audio clips
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

    public void PlayAudioCycle(AudioSource source, int minValue, int maxValue)
    {
        // Ensure that the clips array is not empty and that the provided range is valid
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioPlayer.");
            return;
        }

        if (!source.isPlaying)
        {
            //Cycle through audio clips
            minValue = (minValue + 1) % clips.Length;
            source.PlayOneShot(clips[minValue]);
            if (minValue >= maxValue)
            {
                //Reset
                minValue = 0;
            }
        }
    }
}