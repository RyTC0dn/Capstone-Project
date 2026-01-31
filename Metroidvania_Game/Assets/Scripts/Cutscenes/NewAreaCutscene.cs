using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class NewAreaCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneTimeline;
    [SerializeField] private PlayableAsset[] timeline;
    private float timeEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cutsceneTimeline != null)
        {
            // Restart cutscene on start
            cutsceneTimeline.time = 0;
            // Ensure that the cutscene timeline ignores time scale 
            cutsceneTimeline.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            cutsceneTimeline.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEventTriggered(Component sender, object data)
    {
        if (data is int signal)
        {
            // Play the specific cutscene based on the signal received 
            // from a specific lever, 0 being the first 
            StartCoroutine(Cutscene(signal));
        }
    }

    IEnumerator Cutscene(int number)
    {
        // Pause game time while the cutscene plays (the PlayableDirector runs on unscaled time)
        Time.timeScale = 0;

        try
        {
            if (cutsceneTimeline != null && timeline != null && number >= 0 && number < timeline.Length)
            {
                cutsceneTimeline.playableAsset = timeline[number];
                cutsceneTimeline.Play();

                // Get duration after assigning the playable asset
                timeEnd = (float)cutsceneTimeline.duration;

                // Use unscaled wait so the coroutine advances while timeScale == 0
                yield return new WaitForSecondsRealtime(timeEnd);
            }
            else
            {
                // If invalid, just yield one frame to avoid locking callers
                yield return null;
            }
        }
        finally
        {
            // Always restore time scale
            Time.timeScale = 1;
        }
    }
}
