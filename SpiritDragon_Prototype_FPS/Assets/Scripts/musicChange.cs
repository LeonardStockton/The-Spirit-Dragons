using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicChange : MonoBehaviour
{
    public AudioClip nextMusic;
    public AudioClip currentMusic;

   

    // Update is called once per frame
    void Start()
    {
        
        GetComponent<AudioSource>().clip = currentMusic;
        GetComponent<AudioSource>().Play();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            GetComponent<AudioSource>().clip = nextMusic;
            GetComponent<AudioSource>().Play();
            
        }
    }


    public void BossAlive()
    {
        if (gameManager.instance.bossAlive == false)
        {
            GetComponent<AudioSource>().clip = currentMusic;
            GetComponent<AudioSource>().Play();
        }
    }
    
}
