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

    public SceneInfo sceneInfo;

    private void Start()
    {
        if (sceneInfo.isShieldPickedUp && gameObject.name == "ShieldPickup")
            Destroy(gameObject);
        if (sceneInfo.isWallBreakPickedUp && gameObject.name == "WallBreakPickup")
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && gameObject.tag == "AbilityPickup" && this.gameObject == itemShield)
        {
            hasPickedUpShield = true;
            abilityPickup.Raise(this, hasPickedUpShield);
            Destroy(gameObject);
        }
        if (collision.tag == "Player" && gameObject.tag == "AbilityPickup" && this.gameObject == itemWallBreak)
        {
            hasPickedUpWallBreak = true;
            abilityPickup.Raise(this, hasPickedUpWallBreak);
            Destroy(gameObject);
        }
    }
}