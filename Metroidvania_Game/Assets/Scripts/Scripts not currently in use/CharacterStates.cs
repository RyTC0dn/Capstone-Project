using UnityEngine;

enum CharacterClass
{
    Knight,
    Huntsmen
}

//This script is to help break down the different classes of
//the player into different states, which can be separate scripts
//but called on during a switch state function, but this is currently not in use
public class CharacterStates : MonoBehaviour
{
    private CharacterClass currentClass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
