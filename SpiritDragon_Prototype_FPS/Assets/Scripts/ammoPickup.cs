using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    public int pistolAmmo;
    public int rifleAmmo;
    public int sniperAmmo;
    public int submachineAmmo;
    public int shotgunAmmo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 60 *Time.deltaTime, 0f, Space.Self);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ammo());
            other.GetComponent<playerController>().ammoPack(pistolAmmo, shotgunAmmo, rifleAmmo, sniperAmmo, submachineAmmo);
        }

    }


    IEnumerator ammo()
    {


        gameManager.instance.playerAmmoPickUpScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerAmmoPickUpScreen.SetActive(false);
        Destroy(gameObject);
    }
}
