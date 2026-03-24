using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float[] position;

    public PlayerData (PrototypePlayerMovementControls controls) 
    {
        position = new float[2];
        position[0] = controls.transform.position.x;
        position[1] = controls.transform.position.y;
    }

}
