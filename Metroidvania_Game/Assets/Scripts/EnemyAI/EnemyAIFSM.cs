using UnityEngine;

public class EnemyAIFSM : MonoBehaviour
{
    IState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(IState newState)
    {
        currentState.OnExit();
        currentState = newState;    
        currentState.OnEnter();
    }
}

public interface IState
{
    public void OnEnter();

    public void UpdateState();

    public void OnHurt();

    public void OnExit();
}
