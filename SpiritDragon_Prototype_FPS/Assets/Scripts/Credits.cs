using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] float CredWaitTime;
    // Start is called before the first frame update
    void Start()
    {
        creditsWait();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator creditsWait()
    {

        yield return new WaitForSeconds(CredWaitTime);
        SceneManager.LoadScene(0);
    }
}
