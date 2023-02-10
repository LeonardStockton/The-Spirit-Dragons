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
    [Range(0, 100)][SerializeField] int playerSpeed;
    [Range(0, 100)][SerializeField] int HP;
    [Range(0, 10)][SerializeField] int jumpTimes;
    [Range(0, 100)][SerializeField] int jumpSpeed;
    [Range(0, 100)][SerializeField] int gravity;

    [Header("~~~~~~~Gun Stats~~~~~~~~")]
    [SerializeField] List<gunStats> weaponList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] int weaponAmmo;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] AudioSource gunSound;
    [SerializeField] GameObject weaponModel;

    public int ammo;

    Vector3 move;
    Vector3 playerVelocity;
    int jumpCurrent;
    int HPorg;
    bool isShooting;
    public TextMeshProUGUI ammoDisplay;


    // Start is called before the first frame update
    void Start()
    {
      
        HPorg = HP;
        stolenHealth();
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = ammo.ToString();

        movement();
        if (!isShooting && Input.GetButton("Shoot") && ammo > 0)
        {           
                StartCoroutine(shoot());        
        }

        if (ammo <= 0)
        {
            ammo = 0;
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

    IEnumerator shoot()
    {
        isShooting = true;
        gunSound.Play();
        muzzleFlash.SetActive(true);
        ammo--;
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
        ammo += rounds;
    }

    public void gunPick(gunStats gunStats)
    {
        shootRate = gunStats.shootRate;
        shootDist = gunStats.shootDist;
        shootDamage = gunStats.shootDamage;
        weaponAmmo = gunStats.weaponAmmo;


    }

}
