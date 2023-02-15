using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("~~~~~~~Componets~~~~~~~~")]
    [SerializeField] CharacterController controller;

    [Header("~~~~~~Player Stats~~~~~~")]
    [Range(0, 10)][SerializeField] float playerSpeed;
    [Range(1.5f, 6)][SerializeField] float playerSprintSpeed;
    [Range(0, 100)][SerializeField] int HP;
    [Range(0, 10)][SerializeField] int jumpTimes;
    [Range(0, 100)][SerializeField] int jumpSpeed;
    [Range(0, 100)][SerializeField] int gravity;

    [Header("~~~~~~~Gun Stats~~~~~~~~")]
    [SerializeField] List<gunStats> weaponList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject weaponModel;
    [SerializeField] AudioSource gunSound;
    [SerializeField] public int weaponAmmo;

    [Header("-------Weapon Extra-------")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject bloodEffect;
 


    Vector3 move;
    Vector3 playerVelocity;
    int jumpCurrent;
    int HPorg;
    public bool isShooting;
    public bool isSprinting;
    int selectedWeapon;



    // Start is called before the first frame update
    void Start()
    {
      
        HPorg = HP;
        stolenHealth();
    }

    // Update is called once per frame
    void Update()
    {
       

        movement();
        sprint();
        selectFirearm();
        if (!isShooting && Input.GetButton("Shoot") && weaponAmmo > 0)
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
        move = (transform.right * Input.GetAxis("Horizontal") + (transform.forward * Input.GetAxis("Vertical")));
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (Input.GetButtonDown("Jump") && jumpCurrent < jumpTimes)
        {
            jumpCurrent++;
            playerVelocity.y = jumpSpeed;

        }
        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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
        gunSound.Play();
        muzzleFlash.SetActive(true);
        
        weaponAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);
           
                if (hit.collider.GetComponent<IDamage>() != null)
                {

                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                   GameObject temp =  Instantiate(bloodEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    Destroy(temp, 2);
                }
                else
                {
                    GameObject temp =  Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    Destroy(temp, 2);
                }
               
          
        }
        yield return new WaitForSeconds(shootRate);
        muzzleFlash.SetActive(false);
        isShooting = false;
    }


    public void takeDamage(int dmg)
    {
        HP-=dmg;
        stolenHealth();
        StartCoroutine(youBeenShoot());
        if (HP <= 0)
        {
            
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
        HP+=heals;
        stolenHealth();
    }

    public void ammoPack(int rounds)
    {
        weaponAmmo += rounds;
    }

    public void gunPick(gunStats gunStats)
    {
        weaponList.Add(gunStats);
        shootRate = gunStats.shootRate;
        shootDist = gunStats.shootDist;
        shootDamage = gunStats.shootDamage;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = gunStats.weaponSkin.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats.weaponSkin.GetComponent<MeshRenderer>().sharedMaterial;
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

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponSkin.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponSkin.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
