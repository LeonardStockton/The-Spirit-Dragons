using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
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
    [Range(0, 100)][SerializeField] float shootRate;
    [Range(0, 100)][SerializeField] int shootDist;
    [Range(0, 100)][SerializeField] int shootDamage;
    [Range(0, 100)][SerializeField] int weaponAmmo;

    public int ammo;

    Vector3 move;
    Vector3 playerVelocity;
    int jumpCurrent;
    int HPorg;
    bool isShooting;
    public Text ammoDisplay;


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
        if (!isShooting && Input.GetButton("Shoot"))
        {
            StartCoroutine(shoot());
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

        RaycastHit hit;

        if (ammo > 0)
        {
            ammo -= 1;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }
        }
        yield return new WaitForSeconds(shootRate);

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
    }
    public void ammoPack(int rounds)
    {
        weaponAmmo += rounds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AmmoPickup"))
        {
            ammo += 1;

        }
    }
}
