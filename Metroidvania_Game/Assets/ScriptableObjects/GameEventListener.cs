using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    public string eventDescription; //Just to help explain what each game listener is listening for

    public GameEvent gameEvent; //The game event that you want the specific object to be looking out for

    public CustomGameEvent response; //The function that is called when game event is recieved

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListeneer(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
