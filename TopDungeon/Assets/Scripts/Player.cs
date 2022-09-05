using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Start();
    }

    protected override void RecieveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.RecieveDamage(dmg);
        GameManager.instance.OnHitPointChange();
    }

    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenu.SetTrigger("Show");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinID)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }

    public void OnLevelp()
    {
        maxHitPoint++;
        hitPoint = maxHitPoint;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelp();
    }

    public void Heal(int healingAmout)
    {
        if (hitPoint == maxHitPoint)
            return;

        hitPoint += healingAmout;

        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;

        GameManager.instance.ShowText("+" + healingAmout.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitPointChange();
    }

    public void Respawn()
    {
        isAlive = true;
        Heal(maxHitPoint);
        immuneTime = Time.time;
        pushDirection = Vector3.zero;
    }
}
