using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a component that enables player characters to pick up ability items, such as shields or wall-breaking
/// tools, within a scene.
/// </summary>
/// <remarks>Attach this component to ability item GameObjects to handle pickup interactions and trigger related
/// game events. The component manages item visibility and state based on scene information, ensuring that items already
/// picked up are not available again. Requires a Rigidbody2D component for collision detection. Designed for use in
/// Unity projects where ability pickups are tracked across scenes.</remarks>

[RequireComponent(typeof(Rigidbody2D))] //Use kinematic 
public class ItemPickup : MonoBehaviour
{
    public GameEvent abilityPickup;
    private bool hasPickedUpShield = false;
    private bool hasPickedUpWallBreak = false;

    private SpriteRenderer sp;
    private Collider2D itemCollider;
    AudioPlayer audioPlayer;
    public Object shield;
    public GameObject wallBreak;

    public SceneInfo sceneInfo;
    private int id;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();    
        itemCollider = GetComponent<Collider2D>();
        audioPlayer = FindFirstObjectByType<AudioPlayer>();

        if(sceneInfo.isShieldPickedUp)
            Destroy(shield);
        if (sceneInfo.isWallBreakPickedUp)
            Destroy(wallBreak);

        //Grab game object id from Unity
        id = gameObject.GetInstanceID();
    }

    private void Update()
    {
        //Debug.Log($"Item ID: {id}, Shield Picked Up: {hasPickedUpShield}, Wall Break Picked Up: {hasPickedUpWallBreak}");
        if (sceneInfo.isShieldPickedUp)
        {
            Destroy(shield);
        }
        else if(sceneInfo.isWallBreakPickedUp)
        {
            Destroy(wallBreak);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player" && gameObject.tag == "AbilityPickup" && this.gameObject == shield)
        {
            hasPickedUpShield = true;
            sceneInfo.isShieldPickedUp = true;

            abilityPickup.Raise(this, hasPickedUpShield);

            sp.enabled = false;
            itemCollider.enabled = false;
            Destroy(gameObject, 0.1f); //Destroy after a short delay to allow event to process
        }
        if (collision.tag == "Player" && gameObject.tag == "AbilityPickup" && this.gameObject == wallBreak)
        {
            hasPickedUpWallBreak = true;
            sceneInfo.isWallBreakPickedUp = true;

            //Game events
            abilityPickup.Raise(this, hasPickedUpWallBreak);

            sp.enabled = false;
            itemCollider.enabled = false;
            Destroy(gameObject, 0.1f);
        }
    }
}
