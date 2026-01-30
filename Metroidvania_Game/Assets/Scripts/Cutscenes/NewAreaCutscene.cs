using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class NewAreaCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneTimeline;
    private float timeEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cutsceneTimeline != null)
        {
            //Restart cutscene on start
            cutsceneTimeline.time = 0;
            cutsceneTimeline.Stop();
        }

        //Tie a variable to the duration of the assigned cutscene
        timeEnd = (float)cutsceneTimeline.duration;
        //Ensure that the cutscene timeline ignores time scale 
        cutsceneTimeline.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEventTriggered(Component sender, object data)
    {
        if(data is bool)
        {
            StartCoroutine(Cutscene());
        }
    }

    IEnumerator Cutscene()
    {
        //Set the time scale to 0,
        //pausing the game to let the cutscene play
        Time.timeScale = 0;

        if(cutsceneTimeline != null)
        {
            cutsceneTimeline.Play();
        }

        yield return new WaitForSeconds(timeEnd);

        //After the cutscene finishes playing,
        //have time move again by setting time scale to 1
        Time.timeScale = 1;
    }
}
