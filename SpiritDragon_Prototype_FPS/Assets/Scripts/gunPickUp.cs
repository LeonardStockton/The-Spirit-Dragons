using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickUp : MonoBehaviour
{
    [SerializeField] gunStats weapon;

    private void OnTriggerEnter(Collider weaponGrab)
    {
        if (weaponGrab.CompareTag("Player"))
        {
            gameManager.instance.playerScript.gunPick(weapon);
            Destroy(gameObject);
        }
    }
}
