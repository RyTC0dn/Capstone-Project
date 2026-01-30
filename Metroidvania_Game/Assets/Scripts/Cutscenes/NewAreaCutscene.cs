using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class NewAreaCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutsceneTimeline;
    private double timeEnd;
    public GameEvent cutsceneEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cutsceneTimeline != null)
        {
            cutsceneTimeline.time = 0;
        }

        timeEnd = cutsceneTimeline.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEventTriggered(Component sender, object data)
    {
        if(data is bool)
        {

        }
    }

    IEnumerator Cutscene()
    {
        Time.timeScale = 0;

        if(cutsceneTimeline != null)
        {
            cutsceneTimeline.Play();
        }

        yield return new WaitForSeconds();
    }
}
