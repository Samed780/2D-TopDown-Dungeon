using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collidable
{
    public string message;
    private float cooldown = 4.0f;
    private float lastshout;

    protected override void Start()
    {
        base.Start();
        lastshout = -cooldown;
    }

    protected override void OnCollide(Collider2D collider)
    {
        if(collider.name == "Player")
        {
            if (Time.time - lastshout > cooldown)
            {
                lastshout = Time.time;
                GameManager.instance.ShowText(message, 25, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown);
            }
        }
        
    }
}
