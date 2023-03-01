using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponBehaviorcontrol : MonoBehaviour
{
    [Header("~~~~~~~Gun Stats~~~~~~~~")]
    [SerializeField] List<gunStats> weaponList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] float weaponZoomMax;
    [SerializeField] GameObject weaponModel;

    [SerializeField] public int weaponAmmo;
    [SerializeField] public int Grenades;

    [Header("~~~~~~~~Gun Audio~~~~~~~")]
    [SerializeField] AudioClip[] audDmg;
    [Range(0, 1)][SerializeField] float audDmgVol;
    [SerializeField] AudioClip[] audGunShot;
    [Range(0, 1)][SerializeField] float audGunVol;

    float wepZoomOrg;
    int selectedWeapon;
    int firstGunPickup = 0;


    // Update is called once per frame
    void Update()
    {

    }


    public void gunPick(gunStats gunStats, string gunName)
    {
        weaponList.Add(gunStats);
        shootRate = gunStats.shootRate;
        shootDist = gunStats.shootDist;
        shootDamage = gunStats.shootDamage;
        weaponAmmo = gunStats.weaponAmmo;

        weaponModel.GetComponentInChildren<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponSkin.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponentInChildren<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponSkin.GetComponent<MeshRenderer>().sharedMaterial;
        selectedWeapon = weaponList.Count - 1;


        if (weaponModel.GetComponent<MeshFilter>().mesh.name == "shotgun2 Instance")
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
        }
        if (weaponModel.GetComponent<MeshFilter>().mesh.name == "pistol3 Instance")
        {
            Vector3 newPos = new Vector3(0.57f, -0.95f, 1.83f);
            weaponModel.transform.localPosition = newPos;
        }
        if (weaponModel.GetComponent<MeshFilter>().mesh.name == "assault4 Instance")
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
        }
        UpdateGunUI(selectedWeapon, firstGunPickup);
        ++firstGunPickup;
    }

    public void UpdateGunUI(int GunPos, int condition = 0)
    {
        if (weaponList[GunPos].shootDamage == 5 && condition == 0)
        {
            gameManager.instance.CurrentGunImagePistol.enabled = true;
            gameManager.instance.CurrentGunImageShotgun.enabled = false;
            gameManager.instance.CurrentGunImageAssaultRifle.enabled = false;
        }
        else if (weaponList[GunPos].shootDamage == 25 && condition == 0)
        {
            gameManager.instance.CurrentGunImagePistol.enabled = false;
            gameManager.instance.CurrentGunImageShotgun.enabled = true;
            gameManager.instance.CurrentGunImageAssaultRifle.enabled = false;
        }
        else if (weaponList[GunPos].shootDamage == 10 && condition == 0)
        {
            gameManager.instance.CurrentGunImagePistol.enabled = false;
            gameManager.instance.CurrentGunImageShotgun.enabled = false;
            gameManager.instance.CurrentGunImageAssaultRifle.enabled = true;
        }


    }

    void selectFirearm()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weaponList.Count - 1)
        {
            selectedWeapon++;
            changeFirearm();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
        {
            selectedWeapon--;
            changeFirearm();
        }
    }

    void changeFirearm()
    {
        shootRate = weaponList[selectedWeapon].shootRate;
        shootDist = weaponList[selectedWeapon].shootDist;
        shootDamage = weaponList[selectedWeapon].shootDamage;

        UpdateGunUI(selectedWeapon);


        weaponModel.GetComponentInChildren<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponSkin.GetComponentInChildren<MeshFilter>().sharedMesh;
        weaponModel.GetComponentInChildren<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponSkin.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        if (weaponModel.GetComponent<MeshFilter>().mesh.name == "shotgun2 Instance")
        {

            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
        }
        if (weaponModel.GetComponent<MeshFilter>().mesh.name == "pistol3 Instance")
        {
            Vector3 newPos = new Vector3(0.57f, -0.95f, 1.83f);
            weaponModel.transform.localPosition = newPos;

        }

        if (weaponModel.GetComponent<MeshFilter>().mesh.name == "assault4 Instance")
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;

        }
    }
    public void WeaponCameraFocus()
    {
        if (Input.GetButton("Zoom"))
        { Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, weaponZoomMax, Time.deltaTime * 3); }
        else if (Camera.main.fieldOfView <= wepZoomOrg)
        { Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, wepZoomOrg, Time.deltaTime * 6); }


    }
}


