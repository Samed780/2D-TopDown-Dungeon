using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterMenu : MonoBehaviour
{
    //text fields
    public Text levelText, hitPointText, pesosText, xpText, upgradeCostText;

    //logic
    public int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;


    //character selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;
            OnSelectionChange();
        }
        else
        {
            currentCharacterSelection--;
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count -1;
            OnSelectionChange();
        }
           
    }

    private void OnSelectionChange()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    //weapon upgrade
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    //update character information
    public void UpdateMenu()
    {
        //weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        //meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitPointText.text = GameManager.instance.player.hitPoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        //xpbar
        int currLevel = GameManager.instance.GetCurrentLevel();

        if(currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.xp.ToString() + "total expeience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLeveLXP = GameManager.instance.GetXpToLevel(currLevel -1);
            int currLeveLXP = GameManager.instance.GetXpToLevel(currLevel);
            int diff = currLeveLXP - prevLeveLXP;
            int currXPIntoLevel = GameManager.instance.xp - prevLeveLXP;
            float completionRatio = (float)currXPIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXPIntoLevel.ToString() + "/" + diff;

        }

    }
}
