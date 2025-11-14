using UnityEngine;
using System.Collections.Generic;

public class ItemPickup : MonoBehaviour
{
    public GameEvent abilityPickup;
    private bool hasPickedUpShield = false;
    private bool hasPickedUpWallBreak = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && gameObject.tag == "AbilityPickup" && gameObject.name == "ShieldPickup")
        {
            hasPickedUpShield = true;
            abilityPickup.Raise(this, hasPickedUpShield);
            Destroy(gameObject);
        }
        if (collision.tag == "Player" && gameObject.tag == "AbilityPickup" && gameObject.name == "WallBreakPickup")
        {
            hasPickedUpWallBreak = true;
            abilityPickup.Raise(this, hasPickedUpWallBreak);
            Destroy(gameObject);
        }
    }
}
