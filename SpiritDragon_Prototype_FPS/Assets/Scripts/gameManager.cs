using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Data -----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpwanPOS;

    [Header("----- UI -----")]
   
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject playerDamageFlashScreen;
    public GameObject playerHealthPickUpScreen;
    public GameObject playerAmmoPickUpScreen;
    public Image playerHpBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI timer;
    public float time;
    bool timebool = false;
    public TextMeshProUGUI ammoDisplay;

    [Header("----- Game Goals -----")]
    public int enemiesRemaining;


    public bool isPaused;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = player.GetComponent<playerController>().weaponAmmo.ToString();
        time -= Time.deltaTime;
        timer.text = time.ToString("F0");
        if (time <= 0 && timebool == false)
        {
            timebool = true;
            playerDead();
        }

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
}
