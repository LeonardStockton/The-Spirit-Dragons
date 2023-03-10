
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Animations;

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
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI grenDisplay;
    public GameObject gameMenu;
    

    [Header("----- Game Resource -----")]
    public GameObject sceneLoader;
    public levelLoader loaderScript;

    [Header("----- Game Goals -----")]
    public int enemiesRemaining;

    /*---------------------------------------------------------------------------------*/
    public bool isPaused;
    public float Volume;
    public float GameDifficultyValue;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        sceneLoader = GameObject.FindGameObjectWithTag("LvlLoad");
        loaderScript = sceneLoader.GetComponent<levelLoader>();

    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = player.GetComponent<playerController>().weaponAmmo.ToString();
        grenDisplay.text = player.GetComponent<playerController>().Grenades.ToString();
       

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
        if (enemiesRemaining <= 0)
        {
            setMenu(winMenu);
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
