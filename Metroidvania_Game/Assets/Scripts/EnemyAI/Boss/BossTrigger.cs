using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class BossTrigger : MonoBehaviour
{
    public PlayableDirector cutsceneTimeline;
    private bool hasTriggered = false;
    public SceneInfo sceneInfo;

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

    private void Update()
    {

        if (cutsceneTimeline.time >= cutsceneTimeline.duration)
        {
            // Disable the trigger after the cutscene has been played
            sceneInfo.isBossCutscenePlayed = false;
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cutsceneTimeline != null)
            {
                StartCoroutine(Cutscene());
                sceneInfo.isBossCutscenePlayed = true;
            }
        }
    }

    IEnumerator Cutscene()
    {
        Time.timeScale = 0;

        try
        {

            if (cutsceneTimeline != null)
            {
                cutsceneTimeline.Play();

                float timeEnd = (float)cutsceneTimeline.duration;

                yield return new WaitForSecondsRealtime(timeEnd);
            }
            else
            {
                yield return null;
            }

        }
        finally
        {
            // Ensure that time scale is reset even if something goes wrong
            Time.timeScale = 1;
        }
    }


}
