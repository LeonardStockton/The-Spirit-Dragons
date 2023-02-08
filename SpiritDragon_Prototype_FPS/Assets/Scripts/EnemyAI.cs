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
    [SerializeField] Transform headPos;
    [Range(0,100)][SerializeField] int Hp;
    [Range(0, 50)] [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;

    [Header("----- Gun -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;

    Vector3 plyrDir;
    bool Shooting, NRange;
    float angleToPlayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(NRange && canSeePlayer())
        {     
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                facePlayer();
            }
            if (!Shooting)
                StartCoroutine(shoot());
        }
    }

    bool canSeePlayer()
    {
        plyrDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(plyrDir, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(headPos.position, plyrDir);
        if (Physics.Raycast(headPos.position,plyrDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                return true;
            }
        }
        return false;
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
