using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class NewAreaCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneTimeline;
    [SerializeField] private PlayableAsset[] timeline;
    private float timeEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cutsceneTimeline != null)
        {
            //Restart cutscene on start
            cutsceneTimeline.time = 0;
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
        if(data is int signal)
        {
            //Play the specific cutscene based on the signal recieved 
            //from a specific lever, 0 being the first 
            StartCoroutine(Cutscene(signal));
        }
    }

    IEnumerator Cutscene(int number)
    {
        //Set the time scale to 0,
        //pausing the game to let the cutscene play
        Time.timeScale = 0;

        if(cutsceneTimeline != null)
        {
            cutsceneTimeline.playableAsset = timeline[number];
            cutsceneTimeline.Play();
        }

        yield return new WaitForSeconds(timeEnd);

        //After the cutscene finishes playing,
        //have time move again by setting time scale to 1
        Time.timeScale = 1;
    }
}
