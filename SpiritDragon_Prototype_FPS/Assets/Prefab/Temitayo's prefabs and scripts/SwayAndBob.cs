using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayAndBob : MonoBehaviour
{
    public Transform weaponParent;
    private Vector3 weaponParentO;
    private Vector3 targetBobPosition;
    private float movementC;
    private float idleC;

    private void Start()
    {
        weaponParentO = weaponParent.localPosition;
    }

    private void Update()
    {
       if (gameManager.instance.player.GetComponent<playerController>().isShooting == false)
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                Bob(idleC, 0.025f, 0.025f);
                idleC += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBobPosition, Time.deltaTime * 2f);
            }

            else
            {
                Bob(movementC, 0.035f, 0.035f);
                movementC += Time.deltaTime * 3f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBobPosition, Time.deltaTime * 6f);
            }
        }
        //else if (!isSprinting)
        //{
        //    Bob(movementC, 0.035f, 0.035f);
        //    movementC += Time.deltaTime * 3f;
        //    weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBobPosition, Time.deltaTime * 6f);
        //}
        //else
        //{
        //    Bob(movementC, .15f, 0.075f);
        //    movementC += Time.deltaTime * 5f;
        //    weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetBobPosition, Time.deltaTime * 10f);
        //}

    }
    void Bob(float n, float x, float y)
    {

       targetBobPosition = weaponParentO +  new Vector3( Mathf.Cos(n) * x, Mathf.Sin(n * 2) * y, 0);
    }
}
