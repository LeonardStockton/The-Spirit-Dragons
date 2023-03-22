using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDataTracker : MonoBehaviour
{
    public static List<gunStats> plyrWepList;
    public static int rifleAmmo;
    public static int shotgunAmmo;
    public static int sniperAmmo;
    public static int submachineAmmo;
    public static int pistolAmmo;
    public static int grenAmount;
    public static int selectedWeapon;
    public static float difficultyMod;

    public void Start()
    { 
        if(plyrWepList != null)
        {
            gameManager.instance.playerScript.weaponList = plyrWepList;
            gameManager.instance.playerScript.rifleAmmo = rifleAmmo;
            gameManager.instance.playerScript.shotgunAmmo = shotgunAmmo;
            gameManager.instance.playerScript.sniperAmmo = sniperAmmo;
            gameManager.instance.playerScript.submachineAmmo = submachineAmmo;
            gameManager.instance.playerScript.pistolAmmo = pistolAmmo;
            gameManager.instance.playerScript.Grenades = grenAmount;
            gameManager.instance.playerScript.selectedWeapon = selectedWeapon;
        }
    }

    private void Update()
    {
        if(gameManager.instance.loadNxtLvl == true)
        {
            Debug.Log("Ran data Store");
            plyrWepList = gameManager.instance.playerScript.weaponList;
            rifleAmmo = gameManager.instance.playerScript.rifleAmmo;
            shotgunAmmo = gameManager.instance.playerScript.shotgunAmmo;
            sniperAmmo = gameManager.instance.playerScript.sniperAmmo;
            submachineAmmo = gameManager.instance.playerScript.submachineAmmo;
            pistolAmmo = gameManager.instance.playerScript.pistolAmmo;
            grenAmount = gameManager.instance.playerScript.Grenades;
            selectedWeapon = gameManager.instance.playerScript.selectedWeapon;
        }
    }
}
