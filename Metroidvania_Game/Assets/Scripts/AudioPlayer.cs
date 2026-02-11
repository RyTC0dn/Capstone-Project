using UnityEngine;
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips; //Manually assignable array of audio clips in the inspector

    public void PlayAudio(int clipIndex, AudioSource source){
        if (clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioPlayer.");
            return;
        }
        source.clip = clips[clipIndex];
        source.PlayOneShot(source.clip);
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

        source.clip = clips[Random.Range(minValue, maxValue)];
        source.PlayOneShot(source.clip);
    }
}
