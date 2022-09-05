using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //Resources
    public static GameManager instance;
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    
    //References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitPointBar;
    public GameObject hud;
    public GameObject menu;
    public Animator deathMenu;
    
    //Logic
    public int pesos;
    public int xp;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    
    public bool TryUpgradeWeapon()
    {
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;

    }

    public void Respawn()
    {
        deathMenu.SetTrigger("Hide");
        SceneManager.LoadScene("Main");
        player.Respawn();
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while(r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(xp >= add)
        {
            add += xpTable[r];
            r++;
            if (r == xpTable.Count)
                return r;
        }

        return r;
    }

    public void GrantXp(int experience)
    {
        int currLevel = GetCurrentLevel();
        xp += experience;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        player.OnLevelp();
        OnHitPointChange();
        ShowText("LEVEL UP", 25, Color.cyan, player.transform.position, Vector3.up * 30, 1.0f);
    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //healthbar
    public void OnHitPointChange()
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitPointBar.localScale = new Vector3(1, ratio, 1);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    public void SaveState()
    {
        string save = "";

        save += "0" + "|";
        save += pesos.ToString() + "|";
        save += xp.ToString() + "|";
        save += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", save);

    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        pesos = int.Parse(data[1]);

        xp = int.Parse(data[2]);
        if(GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        weapon.SetWeaponLevel(int.Parse(data[3]));

        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    
}
