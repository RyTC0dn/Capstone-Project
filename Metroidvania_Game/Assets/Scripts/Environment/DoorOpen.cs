using UnityEngine;

public class DoorOpen : MonoBehaviour
{
   public Animator animator;

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
