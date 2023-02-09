using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ammoCount : MonoBehaviour
{
    public Text ammoDisplay;
    public int numofBullets;
    public bool isFiring;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        ammoDisplay.text = numofBullets.ToString();
        if (Input.GetMouseButtonDown(0) && numofBullets > 0)
        {
            isFiring = true;
            numofBullets -= 1;
            isFiring = false;
        }

    }
}
