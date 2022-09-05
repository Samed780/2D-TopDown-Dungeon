using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //damage
    public int damage = 1;
    public float pushForce = 5;


    protected override void OnCollide(Collider2D collider)
    {
        if(collider.tag == "Fighter" && collider.name == "Player")
        {
            Damage dmg = new Damage { damageAmount = damage, origin = transform.position, pushForce = pushForce };
            collider.SendMessage("RecieveDamage", dmg);
        }

    }
}
