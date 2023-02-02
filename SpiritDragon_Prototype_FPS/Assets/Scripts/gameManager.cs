using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;
    [Header("----- Player -----")]

    [Header("----- U.I. -----")]

    [Header("----- Game Goals -----")]
    public int enemiesRemaining;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        if(enemiesRemaining <= 0)
        {
            //pause, set active menu and activate win menu
        }
    }
}
