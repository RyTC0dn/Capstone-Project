using UnityEngine;

public class PlayerInputHub : MonoBehaviour
{
    public static Player_Controller controls;

    private void Awake()
    {
        if (controls == null)
        {
            controls = new Player_Controller();
            controls.Enable();
        }
    }

    private void OnDestroy() {
        if (controls != null)
        {
            controls.Disable();
        }
    
    }
}
