using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    //damage
    public int[] damagePoint = {1, 2, 3, 5, 7, 10, 13, 15};
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f, 3.2f, 3.5f};

    //upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //swing
    private float cooldown = 0.5f;
    private float lastSwing;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
        
    }

    protected override void OnCollide(Collider2D collider)
    {
        if(collider.tag == "Fighter")
        {
            if(collider.name == "Player")
                return;
            Damage dmg = new Damage { damageAmount = damagePoint[weaponLevel], origin = transform.position, pushForce = pushForce[weaponLevel] };
            collider.SendMessage("RecieveDamage", dmg);
        }
    }

    private void Swing()
    {
        animator.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
