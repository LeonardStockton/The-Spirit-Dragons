using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.interactShield.SetActive(true);
            if (Input.GetKeyDown("Interact"))
            {
                gameManager.instance.interactShield.SetActive(false);
                if (gameManager.instance.enemiesRemaining > 0)
                {
                    StartCoroutine(Danger());
                }
                else
                {
                    gameManager.instance.otherReq = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.interactShield.SetActive(false);
        }
    }

    private IEnumerator Danger()
    {
        gameManager.instance.interactShiDanger.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameManager.instance.interactShiDanger.SetActive(false);
    }
}
