using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gunPickUp : MonoBehaviour
{
    [SerializeField] gunStats weapon;
    [SerializeField] Vector3 rot;

    private void OnTriggerEnter(Collider weaponGrab)
    {
        Debug.Log(weaponGrab);
        if (weaponGrab.CompareTag("Player"))
        {
                gameManager.instance.playerScript.gunPick(weapon, "G 19");
                Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(rot, Space.Self);
    }

}
