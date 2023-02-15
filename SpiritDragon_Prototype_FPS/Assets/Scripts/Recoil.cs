using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRot;
    private Vector3 currentPos;
    private Vector3 targetRot;
    private Vector3 targetPos;
    private Vector3 initalGunPos;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;

    [SerializeField] float KickBackZ;
    public float snappiness, returnAmount;

    void Start()
    {
        initalGunPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * returnAmount);
        currentRot = Vector3.Lerp(currentRot, targetRot, Time.fixedDeltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRot);
        if (gameManager.instance.player.GetComponent<playerController>().isShooting == true)
        {
            ReturnToPosition();
        }
            
    }

    public void Rec()
    {
        targetPos += new Vector3(0, 0, KickBackZ);
        targetRot += new Vector3(recoilX, Random.Range(-recoilY, recoilZ), Random.Range(-recoilZ, recoilZ));
       
    }
    public void ReturnToPosition()
    {
        targetPos = Vector3.Lerp(targetPos, initalGunPos, Time.deltaTime * returnAmount);
        currentPos = Vector3.Lerp(currentPos, targetPos, Time.fixedDeltaTime * snappiness);
        transform.localPosition = currentPos;

    }
}
