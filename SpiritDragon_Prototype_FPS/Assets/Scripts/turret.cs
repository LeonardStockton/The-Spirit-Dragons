using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;

    [Header("----- Stats -----")]
    [SerializeField] Transform headPos;
    [Range(0, 100)] [SerializeField] int Hp;
    [Range(0, 50)] [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;

    [Header("----- Gun -----")]
    [SerializeField] List<Transform> barrels;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;

    Vector3 plyrDir;
    bool Shooting, NRange;
    float angleToPlayer;
    int barrelNum;
    Color basic;

    // Start is called before the first frame update
    void Start()
    {
       basic = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(NRange && canSeePlayer())
        {
            facePlayer();
           if(!Shooting)
                StartCoroutine(shoot());
        }
    }
     void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(plyrDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    bool canSeePlayer()
    {
        plyrDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(plyrDir, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(headPos.position, plyrDir);
        if (Physics.Raycast(headPos.position, plyrDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                return true;
            }
        }
        return false;
    }
    public void takeDamage(int dmg)
    {
        Hp -= dmg;
        StartCoroutine(flshDmg());

        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flshDmg()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.2f);
        model.material.color = basic;
    }
    IEnumerator shoot()
    {
        Shooting = true;
        barrelNum = barrels.Count;
        for (int i = 0; i < barrelNum; i++)
        {
            GameObject clone = Instantiate(bullet, barrels[i].position, bullet.transform.rotation);
            Transform bulletForm = clone.transform;
            bulletForm.transform.GetComponent<turBull>().SendBull(plyrDir);
            clone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            yield return new WaitForSeconds(fireRate / barrelNum);
        }
        Shooting = false;
    }

    public void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            NRange = true;
        }
    }

    public void OnTriggerExit(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            NRange = false;
        }
    }

}
