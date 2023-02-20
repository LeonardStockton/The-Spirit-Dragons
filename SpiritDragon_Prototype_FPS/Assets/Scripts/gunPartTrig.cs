using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPartTrig : MonoBehaviour
{
    [SerializeField] gunPartTrigger trigger;
    [SerializeField] gunPartTrigger slide;
    [SerializeField] gunPartTrigger reciver;
    [SerializeField] gunPartTrigger mag;
    [SerializeField] gunPartTrigger fireSelect;

    private void OnTriggerEnter(Collider weaponGrab)
    {
        if (weaponGrab.CompareTag("Player"))
        {
            gameManager.instance.playerScript.gunPartTirgger(trigger,slide, reciver, mag,fireSelect) ;
            Destroy(gameObject);
        }
    }
}
