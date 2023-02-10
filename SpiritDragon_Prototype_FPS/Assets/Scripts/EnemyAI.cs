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
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;

    [Header("----- Gun -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;

    Vector3 plyrDir;
    bool Shooting, NRange;
    float angleToPlayer;
    Vector3 startPos;
    bool pickDest;
    float fromStartPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        fromStartPos = agent.stoppingDistance;
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        if (NRange)
        {
            if (!canSeePlayer() && !pickDest && agent.remainingDistance < 0.1)
            {
                roam();
            }
        }
        else if (!pickDest&& agent.remainingDistance < 0.1 && agent.destination != gameManager.instance.player.transform.position)
        {
            roam();
        }
    }

    bool canSeePlayer()
    {
        plyrDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(plyrDir.x, 0, plyrDir.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, plyrDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, plyrDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = fromStartPos;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!Shooting)
                    StartCoroutine(shoot());
                return true;
            }

        }
        agent.stoppingDistance = 0;
        return false;
    }

    void roam()
    {
        agent.stoppingDistance = 0;
        pickDest = true;


        Vector3 randDir = Random.insideUnitSphere * roamDist;
        randDir += startPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        if (hit.position != null)
        {
            agent.CalculatePath(hit.position, path);
        }
        agent.SetPath(path);
        pickDest = false;

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
            agent.stoppingDistance = 0;
            NRange = false;
        }
    }
}
