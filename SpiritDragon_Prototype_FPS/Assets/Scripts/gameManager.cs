
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Data -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpwanPOS;
    public int HP;
    public int level;
    public int playerLevel;
    public int playerHealth;
    public bool loadNxtLvl;

    [Header("----- UI -----")]   
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject playerDamageFlashScreen;
    public GameObject playerHealthPickUpScreen;
    public GameObject playerAmmoPickUpScreen;
    public GameObject playerGrenPickUpScreen;
    public Image playerHpBar;
    public Image activeGunImage;
    public Image CurrentGunImagePistol;
    public Image CurrentGunImageShotgun;
    public Image CurrentGunImageAssaultRifle;
    public TextMeshProUGUI enemiesRemainingText;
    public GameObject interactShield;
    public GameObject interactShiDanger;
    public GameObject activateShi;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI grenDisplay;
    public GameObject gameMenu;
    

    [Header("----- Game Goals -----")]
    public int enemiesRemaining;
    [SerializeField] public bool otherReq;
    public bool bossAlive = false;
    public bool GoalComplete;
    public int played;

    /*---------------------------------------------------------------------------------*/
    public bool isPaused;
    public float Volume;
    public float GameDifficultyValue;
    

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerScript.enemyMultiplyer = playerDataTracker.difficultyMod;
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            otherReq = true;
        }
        else
        {
            otherReq = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = playerScript.weaponAmmo.ToString();
        grenDisplay.text = playerScript.Grenades.ToString();

        updateGameGoal(0);
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
                pause();
            else
                unPause();
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void unPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0");
        if(SceneManager.GetActiveScene().buildIndex == 2 && enemiesRemaining <= 0)
        {
            StartCoroutine(enemiesDead());
        }
        if (enemiesRemaining <= 0 && bossAlive == false && otherReq == false)
        {
            GoalComplete = true;
            StartCoroutine(WinMenu());
            
        }
    }

    IEnumerator WinMenu()
    {
        if (played != 1)
        {
            played++;
            activeMenu = winMenu;
            activeMenu.SetActive(true);
            yield return new WaitForSeconds(3f);
            unPause();
        }
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene("Credits");
        }
    }
    public void playerDead()
    {
        setMenu(loseMenu);
    }

    public void setMenu(GameObject menu)
    {
        pause();
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    IEnumerator enemiesDead()
    {
        activateShi.SetActive(true);
        yield return new WaitForSeconds(2f);
        activateShi.SetActive(false);

    }
    public void SavePlayer (playerController player)
    {
        SavingSystem.SavePlayer(player);
    }

    public void LoadPlayer()
    {
        playerData data = SavingSystem.PlayerLoad();
        level = data._Level ;
        HP = data._Health;

        Vector3 pos;
        pos.x =data.postion[0];
        pos.y = data.postion[1];
        pos.z = data.postion[2];
        transform.position = pos;
    }
    
}
