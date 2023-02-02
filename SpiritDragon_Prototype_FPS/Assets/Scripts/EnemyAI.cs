using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Stats -----")]
    [Range(1,10)][SerializeField] int Hp;
    [SerializeField] int playerFaceSpeed;

    [Header("----- Gun -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;

    Vector3 plyrDir;
    bool Shooting, NRange;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        //plyrDir = gameManager.instance.player.transform.position = transform.position;
        if(NRange)
        {
            //agent.SetDestination(gameManager.instance.player.transfrom.postion);
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                facePlayer();
            }
            if (!Shooting)
                StartCoroutine(shoot());
        }
    }

    public void takeDamage(int dmg)
    {
        Hp -= dmg;
        StartCoroutine(flshDmg());

        if(Hp <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flshDmg()
    {
        Color basic = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(.2f);
        model.material.color = basic;
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(plyrDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator shoot()
    {
        Shooting = true;
        GameObject clone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        clone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        yield return new WaitForSeconds(fireRate);
        Shooting = false;
    }

    public void OnTriggerEnter(Collider obj)
    {
        if(obj.CompareTag("Player"))
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
