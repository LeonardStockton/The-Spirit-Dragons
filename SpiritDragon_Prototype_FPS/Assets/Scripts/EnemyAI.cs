using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject miniMapObj;

    [Header("----- Stats -----")]
    [SerializeField] Transform headPos;
    [Range(1, 100)] [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int speedChase;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] int bdyClnWait;

    [Header("----- Gun -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject Gren;
    [SerializeField] GameObject explo;
    [SerializeField] int bulletSpeed;
    [SerializeField] float fireRate;
    [SerializeField] Collider weaponCollider;
    [SerializeField] float bullVelY;

    [Header("-------Audio-------")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)] [SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDmg;
    [Range(0, 1)] [SerializeField] float audDmgVol;

    [Header("----- BossParts -----")]
    [SerializeField] GameObject Laser;
    [SerializeField] Transform lsrFirePt;
    [SerializeField] int lsrDmg;

    Vector3 playerDir;
    bool isShooting;
    public bool playerNRange;
    float angleToPlayer;
    Vector3 startPos;
    bool destinationChosen;
    float stoppingDistanceOrig;
    float speedOrg;
    Color orgColor;
    int shootLsr;

    // Start is called before the first frame update
    void Start()
    {
        agentStart();
        startPos = transform.position;
        stoppingDistanceOrig = agent.stoppingDistance;
        speedOrg = agent.speed;
        roam();
        orgColor = model.material.color;
        if (this.CompareTag("Boss"))
        {
            gameManager.instance.updateGameGoal(1);
            Laser.GetComponent<LineRenderer>().enabled = false;
            gameManager.instance.bossAlive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (playerNRange)
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

    IEnumerator roam()
    {

        if (!destinationChosen && agent.remainingDistance < .01f)
        {
            agent.stoppingDistance = 0;
            destinationChosen = true;
            agent.speed = speedOrg;
            yield return new WaitForSeconds(waitTime);
            destinationChosen = false;
            
            if (agent.isActiveAndEnabled)
            {
                Vector3 randDir = Random.insideUnitSphere * roamDist;
                randDir += startPos;

                NavMeshHit hit;
                NavMesh.SamplePosition(randDir, out hit, roamDist, NavMesh.AllAreas);
                agent.SetDestination(hit.position);
            }
        }
    }

    bool canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward); 

       
        Debug.DrawRay(headPos.position,playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistanceOrig;
                agent.speed = speedChase;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                    StartCoroutine(shoot());
                return true;
            }

        }
        agent.stoppingDistance = 0;
        return false;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            gameManager.instance.updateGameGoal(-1);
            anim.SetBool("DED", true);
            agent.enabled = false;
            StartCoroutine(deathCleanup());
            miniMapObj.SetActive(false);
            if (this.CompareTag("Boss"))
            {
                Laser.GetComponent<LineRenderer>().enabled = false;
                gameManager.instance.bossAlive = false;
            }
        }
        else
        {
            anim.SetTrigger("Dmg");
            StartCoroutine(flashDmg());
            aud.PlayOneShot(audDmg[Random.Range(0, audDmg.Length)], audDmgVol);


            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    IEnumerator flashDmg()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        model.material.color = orgColor;
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        if(Gren != null)
        {
            GameObject grenClone = Instantiate(Gren, shootPos.position, Gren.transform.rotation);
            grenClone.GetComponent<Rigidbody>().velocity = (playerDir + new Vector3(Random.Range(-.3f, .3f), bullVelY, Random.Range(-.3f, .3f))) * bulletSpeed;
            Instantiate(explo, grenClone.transform.position, grenClone.transform.rotation);
            Destroy(grenClone, 1);
            anim.SetTrigger("Shoot");
        }
        if (this.CompareTag("Boss"))
        {
            createBullet();

            yield return new WaitForSeconds(fireRate * 2);
            shootLsr = Random.Range(1, 5);
            if (shootLsr == 2)
            {
                agent.SetDestination(transform.position);
                anim.SetTrigger("Laser");
            }
        }
        isShooting = false;
    }

    public void createBullet()
    {
        {
            GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = playerDir * bulletSpeed;
        }
    }

    public void Beam()
    {
        facePlayer();
        StartCoroutine(laserbeam());
    }

    IEnumerator laserbeam()
    {
        Laser.GetComponent<LineRenderer>().enabled = true;
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(.5f);
        Laser.GetComponent<LineRenderer>().enabled = false;
    }
    public void agentStop()
    {
        agent.enabled = false;
    }
    public void agentStart()
    {
        agent.enabled = true; ;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public void stepSound()
    {
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);
    }

    IEnumerator deathCleanup()
    {
        yield return new WaitForSeconds(bdyClnWait);
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
        model.enabled = false;
    }

}

