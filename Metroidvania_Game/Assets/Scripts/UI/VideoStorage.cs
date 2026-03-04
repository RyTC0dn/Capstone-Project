using UnityEngine;
using UnityEngine.Video;

public class VideoStorage : MonoBehaviour
{
    public VideoClip[] clips; //Manually assign clips in array 

    public void PlayVideo(int clipIndex, VideoPlayer player)
    {
        if(clips.Length == 0)
        {
            Debug.LogWarning("No video clips assigned to video storage");
            return; 
        }
        player.clip = clips[clipIndex];
        player.Play();
    }
}
