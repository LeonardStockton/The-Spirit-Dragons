using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicChange : MonoBehaviour
{
    public AudioClip nextMusic;
    public AudioClip currentMusic;
    public int playOnce = 1;

   

    // Update is called once per frame
    void Start()
    {
        
        GetComponent<AudioSource>().clip = currentMusic;
        GetComponent<AudioSource>().Play();

    }

    void Update()
    {
        BossAlive();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.bossAlive == true)
        {
            
            GetComponent<AudioSource>().clip = nextMusic;
            GetComponent<AudioSource>().Play();  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    public void BossAlive()
    {
        if (gameManager.instance.bossAlive == false && playOnce == 1)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = currentMusic;
            GetComponent<AudioSource>().Play();
            playOnce++;
        }
    }
    
}
