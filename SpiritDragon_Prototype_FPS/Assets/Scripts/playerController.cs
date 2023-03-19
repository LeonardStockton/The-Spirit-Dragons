using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.UIElements;
//using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("~~~~~~~Componets~~~~~~~~")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;


    [Header("~~~~~~Player Stats~~~~~~")]
    [Range(0, 10)][SerializeField] float playerSpeed;
    [Range(1.5f, 6)][SerializeField] float playerSprintSpeed;
    [Range(0, 100)][SerializeField] int HP;
    [Range(0, 10)][SerializeField] int jumpTimes;
    [Range(0, 100)][SerializeField] int jumpSpeed;
    [Range(0, 100)][SerializeField] int gravity;
    [Range(1, 5)][SerializeField] int pushbackResTime;

    [Header("~~~~~~~Gun Stats~~~~~~~~")]
    [SerializeField] public List<gunStats> weaponList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] GameObject barrel;
    [SerializeField] ParticleSystem shootEffect;
    [SerializeField] ParticleSystem impactEffect;
    [SerializeField] TrailRenderer bullTrail;
    [SerializeField] int shootDist;
    [SerializeField] float shootDamage;
    [SerializeField] float weaponZoomMax;
    [SerializeField] GameObject weaponModel;
    [SerializeField] public int weaponAmmo;
    [SerializeField] public int Grenades;

    [Header("-------Grenade-------")]
    [SerializeField] float grenTimer;
    [SerializeField] int blastRadius;
    [SerializeField] int throwSpeed;
    [SerializeField] GameObject gren;
    [SerializeField] GameObject exp;
    [SerializeField] Transform throwPos;

    [Header("-------Audio-------")]
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDmg;
    [Range(0, 1)][SerializeField] float audDmgVol;
    [SerializeField] AudioClip[] audGunShot;
    [Range(0, 1)][SerializeField] float audGunVol;

    Vector3 move;
    Vector3 playerVelocity;
    Vector3 pushBack;
    float wepZoomOrg;
    int jumpCurrent;
    int HPorg;
    public bool interact;
    public int selectedWeapon;
    public int rifleAmmo;
    public int shotgunAmmo;
    public int sniperAmmo;
    public int submachineAmmo;
    public int pistolAmmo;
    public bool isShooting;
    public bool isSprinting;
    bool isPlayingSteps;
    int firstGunPickup = 0;
    public float enemyMultiplyer;
    public float enemyDefaultValue=1;



    // Start is called before the first frame update
    void Start()
    {
        if(weaponList.Count > 0)
        {
            changeFirearm();
        }
        HPorg = HP;
        wepZoomOrg = Camera.main.fieldOfView;
        stolenHealth();
    }

    // Update is called once per frame
    void Update()
    {
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushbackResTime);
        StartCoroutine(throwGrenade());
        movement();
        sprint();
        selectFirearm();
        if (!isShooting && weaponList.Count > 0 && Input.GetButton("Shoot") && weaponAmmo > 0)
        {
            StartCoroutine(shoot());
        }
        if (weaponAmmo <= 0)
        {
            weaponAmmo = 0;
        }
        if(weaponList.Count > 0)
        {
            updateAmmo();
        }
    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpCurrent = 0;
        }
        move = ((transform.right * Input.GetAxis("Horizontal") + (transform.forward * Input.GetAxis("Vertical"))));
        move = move.normalized;
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (Input.GetButtonDown("Jump") && jumpCurrent < jumpTimes)
        {
            jumpCurrent++;
            playerVelocity.y = jumpSpeed;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
        if (controller.isGrounded && move.normalized.magnitude > 0.5f && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= playerSprintSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= playerSprintSpeed;
        }
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingSteps = false;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        stolenHealth();
        StartCoroutine(youBeenShoot());
        aud.PlayOneShot(audDmg[Random.Range(0, audDmg.Length)], audDmgVol);
        if (HP <= 0)
        {
            gameManager.instance.playerDead();
        }
    }

    public int GetPlayerHealth() 
    {
        return HP;
    }

    public void stolenHealth()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)HP / (float)HPorg;
    }

    //for when we add health, weapons, and ammo. *do not erase*
    public void healthPack(int heals)
    {
        HP += heals;
        if (HP > HPorg)
        {
            HP = HPorg;
        }
        stolenHealth();
    }
    IEnumerator youBeenShoot()
    {
        gameManager.instance.playerDamageFlashScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlashScreen.SetActive(false);
    }

    public void ammoPack(int pis, int shot, int rif, int smg, int snp)
    {
            shotgunAmmo += shot;  
            pistolAmmo += pis;
            rifleAmmo += rif;
            sniperAmmo += snp;
            submachineAmmo += smg;

    UpdateGunUI(selectedWeapon);
    }

    public void grenPack(int nades)
    {
        Grenades += nades;
    }

    IEnumerator recoil()
    {
        weaponModel.transform.localPosition = new Vector3(weaponModel.transform.localPosition.x, weaponModel.transform.localPosition.y, weaponModel.transform.localPosition.z - (float)(.05 * shootDamage));
        weaponModel.transform.localRotation = Quaternion.Lerp(weaponModel.transform.localRotation, new Quaternion(0, 5, 0, (float)(20 * shootDamage)), shootRate/2);
        yield return new WaitForSeconds(shootRate / 2);
        weaponModel.transform.localPosition = new Vector3(weaponModel.transform.localPosition.x , weaponModel.transform.localPosition.y, weaponModel.transform.localPosition.z + (float)(.05 * shootDamage));
        weaponModel.transform.localRotation = Quaternion.Lerp(weaponModel.transform.localRotation, new Quaternion(0, -5, 0, (float)(20 * shootDamage)), shootRate / 2);
    }
    IEnumerator shoot()
    {
        isShooting = true;
        aud.PlayOneShot(audGunShot[Random.Range(0, audGunShot.Length)], audGunVol);
        StartCoroutine(recoil());
        Instantiate(shootEffect, barrel.transform.position, Quaternion.LookRotation(barrel.transform.forward));
        weaponAmmo--;
       if (weaponList[selectedWeapon].gunName.Contains("pistol"))
        {
            pistolAmmo--;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("shotgun"))
        {
            shotgunAmmo--;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("rifle"))
        {
            rifleAmmo--;
        }
        if (Physics.Raycast(barrel.transform.position, barrel.transform.forward, out RaycastHit hit, shootDist))
        {
            TrailRenderer trail = Instantiate(bullTrail, barrel.transform.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
            

            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage((int)shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
   public IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        if (hit.collider.GetComponent<IDamage>() == null)
             Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
    public void gunPick(gunStats gunStats, string gunName)
    {
        weaponList.Add(gunStats);
        shootRate = gunStats.shootRate;
        shootDist = gunStats.shootDist;
        shootDamage = (gunStats.shootDamage * gameManager.instance.GameDifficultyValue);
        if(gunName.Contains("pistol"))
        {
            pistolAmmo = pistolAmmo + gunStats.weaponAmmo;
        }
        else if (gunName.Contains("shotgun"))
        {
            shotgunAmmo = shotgunAmmo + gunStats.weaponAmmo;
        }
        else if (gunName.Contains("rifle"))
        {
            rifleAmmo = rifleAmmo + gunStats.weaponAmmo;
        }

        weaponModel.GetComponentInChildren<MeshFilter>().sharedMesh = gunStats.weaponSkin.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponentInChildren<MeshRenderer>().sharedMaterial = gunStats.weaponSkin.GetComponent<MeshRenderer>().sharedMaterial;
        selectedWeapon = weaponList.Count - 1;

        if (weaponList[selectedWeapon].gunName.Contains("shotgun"))
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
            weaponAmmo = shotgunAmmo;
            barrel.transform.localPosition = new Vector3(1.326f, -1.029f, 6.418f);
        }
        if (weaponList[selectedWeapon].gunName.Contains("pistol"))
        {
            Vector3 newPos = new Vector3(0.57f, -0.95f, 1.83f);
            weaponModel.transform.localPosition = newPos;
            weaponAmmo = pistolAmmo;
            barrel.transform.localPosition = new Vector3(0.561f, -0.539f, 2.345f);
        }
        if (weaponList[selectedWeapon].gunName.Contains("rifle"))
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
            weaponAmmo = rifleAmmo;
            barrel.transform.localPosition = new Vector3(1.311f, -0.878f, 6.714f);
        }
        UpdateGunUI(selectedWeapon, firstGunPickup);
        ++firstGunPickup;
    }

    public void UpdateGunUI(int GunPos, int condition = 0)
    {
        if (weaponList[selectedWeapon].gunName.Contains("pistol"))
        {
            gameManager.instance.CurrentGunImagePistol.enabled = true;
            gameManager.instance.CurrentGunImageShotgun.enabled = false;
            gameManager.instance.CurrentGunImageAssaultRifle.enabled = false;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("shotgun"))
        {
            gameManager.instance.CurrentGunImagePistol.enabled = false;
            gameManager.instance.CurrentGunImageShotgun.enabled = true;
            gameManager.instance.CurrentGunImageAssaultRifle.enabled = false;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("rifle"))
        {
            gameManager.instance.CurrentGunImagePistol.enabled = false;
            gameManager.instance.CurrentGunImageShotgun.enabled = false;
            gameManager.instance.CurrentGunImageAssaultRifle.enabled = true;
        }
        gameManager.instance.ammoDisplay.text = weaponAmmo.ToString(); 

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
        if (weaponList[selectedWeapon].gunName.Contains("shotgun"))
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
            weaponAmmo = shotgunAmmo;
            barrel.transform.localPosition = new Vector3(1.326f, -1.029f, 6.418f);
        }
        if (weaponList[selectedWeapon].gunName.Contains("pistol"))
        {
            Vector3 newPos = new Vector3(0.57f, -0.95f, 1.83f);
            weaponModel.transform.localPosition = newPos;
            weaponAmmo = pistolAmmo;
            barrel.transform.localPosition = new Vector3(0.561f, -0.539f, 2.345f);
        }
        if (weaponList[selectedWeapon].gunName.Contains("rifle"))
        {
            Vector3 newPos = new Vector3(1.33f, -1.58f, 3.8f);
            weaponModel.transform.localPosition = newPos;
            weaponAmmo = rifleAmmo;
            barrel.transform.localPosition = new Vector3(1.311f, -0.878f, 6.714f);
        }
    }
    
    public void WeaponCameraFocus()
    {
        if (Input.GetButton("Zoom"))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, weaponZoomMax, Time.deltaTime * 3);
        }
        else if (Camera.main.fieldOfView <= wepZoomOrg)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, wepZoomOrg, Time.deltaTime * 6);

        }
    }
    
    public void PushBackDir(Vector3 dir)
    {
        pushBack += dir;
    }

    IEnumerator throwGrenade()
    {
        if (Input.GetButtonDown("Grenade") && Grenades > 0)
        {
            Grenades--;
            GameObject grenadeClone = Instantiate(gren, throwPos.position, gren.transform.rotation);
            grenadeClone.GetComponent<Rigidbody>().velocity = ((Camera.main.transform.forward * throwSpeed) + new Vector3(0, .5f, 0) * throwSpeed);
            yield return new WaitForSeconds(grenTimer);
            GameObject explosion = Instantiate(exp, grenadeClone.transform.position, grenadeClone.transform.rotation);
            explosion.GetComponent<SphereCollider>().radius = blastRadius;
            Destroy(grenadeClone);
            Destroy(explosion, (float).5);
        }
    }
   
    public void Spawner()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpwanPOS.transform.position;

        HP = HPorg;
        stolenHealth();

        controller.enabled = true;
    }

    public void Interact()
    {
        if(Input.GetKeyDown("Interact"))
        {
            interact = true;
        }
        if (Input.GetKeyUp("Interact"))
        {
            interact = false;
        }
    }

    public void updateAmmo()
    {
        if (weaponList[selectedWeapon].gunName.Contains("pistol"))
        {
            weaponAmmo = pistolAmmo;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("shotgun"))
        {
            weaponAmmo = shotgunAmmo;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("rifle"))
        {
            weaponAmmo = rifleAmmo;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("sniper"))
        {
            weaponAmmo = sniperAmmo;
        }
        else if (weaponList[selectedWeapon].gunName.Contains("SMG"))
        {
            weaponAmmo = submachineAmmo;
        }
    }
}
