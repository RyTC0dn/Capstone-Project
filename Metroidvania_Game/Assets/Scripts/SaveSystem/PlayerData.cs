using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int health;

    public float[] position;

    public PlayerData (PrototypePlayerMovementControls controller) 
    {
        position = new float[2];
        position[0] = controller.transform.position.x;
        position[1] = controller.transform.position.y;
    }

}
