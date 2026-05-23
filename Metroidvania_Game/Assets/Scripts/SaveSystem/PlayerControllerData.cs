using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerControllerData
{
    public float[] position;

    public PlayerControllerData (PrototypePlayerMovementControls controller) 
    {
        position = new float[2];
        position[0] = controller.transform.position.x;
        position[1] = controller.transform.position.y;
    }
}
