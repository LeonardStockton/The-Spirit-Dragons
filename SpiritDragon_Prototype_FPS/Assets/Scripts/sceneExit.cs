using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneExit : MonoBehaviour
{
    public string sceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.instance.GoalComplete == true)
        {
            SceneManager.LoadScene(sceneToLoad);

        }
    }

    

}
