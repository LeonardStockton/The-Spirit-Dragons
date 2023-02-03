using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("~~~~~~~Componets~~~~~~~~")]
    [SerializeField] CharacterController controller;

    [Header("~~~~~~Player Stats~~~~~~")]
    [Range(1, 5)][SerializeField] int playerSpeed;
    [Range(5, 15)][SerializeField] int HP;
    [Range(1, 2)][SerializeField] int jumpTimes;
    [Range(1, 5)][SerializeField] int jumpSpeed;
    [Range(1, 5)][SerializeField] int gravity;

    [Header("~~~~~~~Gun Stats~~~~~~~~")]
    [Range(1, 5)][SerializeField] float shootRate;
    [Range(1, 5)][SerializeField] int shootDist;
    [Range(1, 5)][SerializeField] int shootDamage;
    [Range(1, 5)][SerializeField] int weaponAmmo;

    Vector3 move;
    Vector3 playerVelocity;
    int jumpCurrent;
    int HPorg;
    bool isShooting;


    // Start is called before the first frame update
    void Start()
    {
        HPorg = HP;

    }

    // Update is called once per frame
    void Update()
    {
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
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }


    public void takeDamage(int dmg)
    {
        gameManager.instance.playerDead();
    }


}
