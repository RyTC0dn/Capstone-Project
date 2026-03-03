using UnityEngine;

public class DoorOpen : MonoBehaviour
{
   public Animator animator;

    private void Start()
    {
        if (PlayerPrefs.GetInt("DoorOpen", 0) == 1)
        {
            gameObject.SetActive(false);
            Debug.Log("Door is already open");
        }
    }

    public void OpenDoor(Component sender, object data)
    {
        if(data is bool flipped && gameObject.name == "Door")
        {
            if(flipped == true)
            {
                animator.Play("DoorOpen");
            }            
        }
    }
}
