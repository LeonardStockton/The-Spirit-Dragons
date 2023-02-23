using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
//using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("~~~~~~~Componets~~~~~~~~")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    //[SerializeField] Animator animes;


    [Header("~~~~~~Player Stats~~~~~~")]
    [Range(0, 10)] [SerializeField] float playerSpeed;
    [Range(1.5f, 6)] [SerializeField] float playerSprintSpeed;
    [Range(0, 100)] [SerializeField] int HP;
    [Range(0, 10)] [SerializeField] int jumpTimes;
    [Range(0, 100)] [SerializeField] int jumpSpeed;
    [Range(0, 100)] [SerializeField] int gravity;
    [Range(1, 5)] [SerializeField] int pushbackResTime;

    [Header("~~~~~~~Gun Stats~~~~~~~~")]
    [SerializeField] List<gunStats> weaponList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] float weaponZoomMax;
    [SerializeField] GameObject weaponModel;
    [SerializeField] AudioSource gunSound;
    [SerializeField] public int weaponAmmo;
    [SerializeField] public int Grenades;

    [Header("-------Weapon Extra-------")]
    //[SerializeField] GameObject muzzleFlash;
    //[SerializeField] GameObject hitEffect;
    //[SerializeField] GameObject bloodEffect;

    [Header("-------Grenade-------")]
    [SerializeField] float grenTimer;
    [SerializeField] int blastRadius;
    [SerializeField] int throwSpeed;
    [SerializeField] GameObject gren;
    [SerializeField] GameObject exp;
    [SerializeField] Transform throwPos;

    [Header("-------Audio-------")]
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)] [SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)] [SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDmg;
    [Range(0, 1)] [SerializeField] float audDmgVol;


    Vector3 move;
    Vector3 playerVelocity;
    Vector3 pushBack;
    float wepZoomOrg;
    int jumpCurrent;
    int HPorg;
    int selectedWeapon;
    public bool isShooting;
    public bool isSprinting;
    bool isPlayingSteps;


    // Start is called before the first frame update
    void Start()
    {
        HPorg = HP;
        wepZoomOrg = Camera.main.fieldOfView;
        stolenHealth();
    }

    // Update is called once per frame
    void Update()

    {
        
        //if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        //{

        //    animes.SetBool("run", true);
        //}
        //else if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        //{
        //    animes.SetBool("run", true);
        //}
        //else
        //{
        //    animes.SetBool("run", false);
        //}

        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushbackResTime);
        StartCoroutine(throwGrenade());
        //animes.SetFloat("Speed", playerVelocity.normalized.magnitude);
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
            //animes.SetBool("Jump", true);
            jumpCurrent++;
            playerVelocity.y = jumpSpeed;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);

        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        if (controller.isGrounded &&  move.normalized.magnitude > 0.5f && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
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

    IEnumerator shoot()
    {
        isShooting = true;
        
        //muzzleFlash.SetActive(true);

        weaponAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.GetComponent<IDamage>() != null)
            {

                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                //GameObject temp = Instantiate(bloodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //Destroy(temp, 2);
            }
            else
            {
                //GameObject temp = Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //Destroy(temp, 2);
            }


        }
        yield return new WaitForSeconds(shootRate);
        //muzzleFlash.SetActive(false);
        isShooting = false;
    }


    public void takeDamage(int dmg)
    {
        HP -= dmg;
        stolenHealth();
        StartCoroutine(youBeenShoot());
        aud.PlayOneShot(audDmg[Random.Range(0, audDmg.Length)], audDmgVol);
        if (HP <= 0)
        {
            //animes.SetBool("Dead", true);
            gameManager.instance.playerDead();
        }
    }

    public void stolenHealth()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)HP / (float)HPorg;
    }

    IEnumerator youBeenShoot()
    {
        gameManager.instance.playerDamageFlashScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlashScreen.SetActive(false);
    }
    //for when we add health, weapons, and ammo. *do not erase*
    public void healthPack(int heals)
    {
        HP += heals;
        stolenHealth();
    }

    public void ammoPack(int rounds)
    {
        weaponAmmo += rounds;
    }

    public void grenPack(int nades)
    {
        Grenades += nades;
    }

    public void gunPick(gunStats gunStats)
    {
        weaponList.Add(gunStats);
        shootRate = gunStats.shootRate;
        shootDist = gunStats.shootDist;
        shootDamage = gunStats.shootDamage;
        weaponAmmo = gunStats.weaponAmmo;

        

        weaponModel.GetComponentInChildren<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponSkin.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponentInChildren<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponSkin.GetComponent<MeshRenderer>().sharedMaterial;
        selectedWeapon = weaponList.Count - 1;

    }

    void selectFirearm()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weaponList.Count - 1)
        {
            selectedWeapon++;
            changeFirearm();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon > 0)
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

        weaponModel.GetComponentInChildren<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponSkin.GetComponentInChildren<MeshFilter>().sharedMesh;
        weaponModel.GetComponentInChildren<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponSkin.GetComponentInChildren<MeshRenderer>().sharedMaterial;

    }

    public void Spawner()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpwanPOS.transform.position;

        HP = HPorg;
        stolenHealth();

        controller.enabled = true;
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
            GameObject grenadeClone = Instantiate(gren, throwPos.position, gren.transform.rotation);
            grenadeClone.GetComponent<Rigidbody>().velocity = ((Camera.main.transform.forward * throwSpeed) + new Vector3(0, .5f, 0) * throwSpeed);
            yield return new WaitForSeconds(grenTimer);
            GameObject explosion = Instantiate(exp, grenadeClone.transform.position, grenadeClone.transform.rotation);
            explosion.GetComponent<SphereCollider>().radius = blastRadius;
            Destroy(grenadeClone);
            Destroy(explosion, (float).5);
            Grenades--;
        }
    }
}
