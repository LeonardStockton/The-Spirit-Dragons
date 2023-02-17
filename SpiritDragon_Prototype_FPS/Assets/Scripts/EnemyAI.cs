using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anime;

    [Header("----- Stats -----")]
    [SerializeField] Transform headPos;
    [Range(0,100)][SerializeField] int Hp;
    [Range(0, 50)] [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
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
    float speedOrig;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        fromStartPos = agent.stoppingDistance;
        speedOrig = agent.speed;
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anime.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (NRange)
            {
                if (!canSeePlayer())
                {
                    StartCoroutine(roam());
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }
        }
    }

    bool canSeePlayer()
    {
        plyrDir = (gameManager.instance.player.transform.position - headPos.position).normalized;
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

    IEnumerator roam()
    {
        if (!pickDest && agent.remainingDistance < .01f)
        {
            agent.stoppingDistance = 0;
            pickDest = true;
            yield return new WaitForSeconds(waitTime);
            pickDest = false;

            if (agent.isActiveAndEnabled)
            {
                Vector3 randDir = Random.insideUnitSphere * roamDist;
                randDir += startPos;

                NavMeshHit hit;
                NavMesh.SamplePosition(randDir, out hit, 1, 1);

                agent.SetDestination(hit.position);
            }
        }
    }

    public void takeDamage(int dmg)
    {
        if (agent.isActiveAndEnabled)
        {
            Hp -= dmg;
            StartCoroutine(flshDmg());
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (Hp <= 0)
            {
                gameManager.instance.updateGameGoal(-1);
                Destroy(gameObject);
            }
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
        plyrDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(plyrDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator shoot()
    {
        Shooting = true;

        anime.SetTrigger("Shoot");
        yield return new WaitForSeconds(fireRate);
        Shooting = false;
    }

    public void createBullet()
    {
        GameObject clone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        clone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
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
    public void agentStop()
    {
        agent.enabled = false;
    }
    public void agentStart()
    {
        agent.enabled = true; ;
    }
}
