using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            StartCoroutine(health());
            other.GetComponent<playerController>().healthPack(50);
        }
    }


    IEnumerator health()
    {
        
       
        gameManager.instance.playerHealthPickUpScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerHealthPickUpScreen.SetActive(false);
        Destroy(gameObject);
    }
}