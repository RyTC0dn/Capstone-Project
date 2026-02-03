using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))] //Use kinematic 
public class ItemPickup : MonoBehaviour
{
    public GameEvent abilityPickup;
    private bool hasPickedUpShield = false;
    private bool hasPickedUpWallBreak = false;
    public GameObject itemShield; //Manually assign in inpsector
    public GameObject itemWallBreak; //Manually assign in inpsector

    private SpriteRenderer sp;
    private Collider2D itemCollider;

    public SceneInfo sceneInfo;
    private int id;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();    
        itemCollider = GetComponent<Collider2D>();
        if(sceneInfo.isShieldPickedUp && gameObject.name == "ShieldPickup")
            Destroy(gameObject);
        if (sceneInfo.isWallBreakPickedUp && gameObject.name == "WallBreakPickup")
            Destroy(gameObject);

        //Grab game object id from Unity
        id = gameObject.GetInstanceID();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player" && gameObject.tag == "AbilityPickup" && this.gameObject == itemShield)
        {
            hasPickedUpShield = true;
            sceneInfo.isShieldPickedUp = true;

            abilityPickup.Raise(this, hasPickedUpShield);

            sp.enabled = false;
            itemCollider.enabled = false;
            Destroy(gameObject, 0.1f); //Destroy after a short delay to allow event to process
        }
        if (collision.tag == "Player" && gameObject.tag == "AbilityPickup" && this.gameObject == itemWallBreak)
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
