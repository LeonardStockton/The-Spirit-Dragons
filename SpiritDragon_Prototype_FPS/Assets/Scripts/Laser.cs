using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    [SerializeField] int lsrdmg;

    void Start()
    {
        lr.GetComponent<LineRenderer>();
        //lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lr.enabled == true)
        {
            lr.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider)
                {
                    lr.SetPosition(1, hit.point);
                    if (hit.collider.GetComponent<IDamage>() != null)
                    {
                        hit.collider.GetComponent<IDamage>().takeDamage(lsrdmg);
                    }
                    else if (hit.collider.GetComponent<playerController>() != null)
                    {
                        hit.collider.GetComponent<playerController>().takeDamage(lsrdmg);
                    }
                }

            }
            else lr.SetPosition(1, transform.forward * 1000);
        }
    }
}
