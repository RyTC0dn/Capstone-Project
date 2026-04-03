using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class NewAreaCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneTimeline;
    [SerializeField] private PlayableAsset[] timeline;
    private float timeEnd;
    public Camera cutsceneCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        cutsceneCamera = GameObject.Find("CutsceneCamera").GetComponent<Camera>();
        cutsceneCamera.gameObject.SetActive(false);

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
    private void Update()
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

    private IEnumerator Cutscene(int number)
    {
        cutsceneCamera.gameObject.SetActive(true);

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
            cutsceneCamera.gameObject.SetActive(false);
        }
    }
}