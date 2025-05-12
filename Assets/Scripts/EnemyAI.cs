using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] patrolPoints;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 50;
    bool isDead;

    Vector3 walkPoint;
    bool walkPointSet;

    public float timeBetweenAttacks = 1;
    bool alreadyAttacked;
    public GameObject shootingPoint;
    public GameObject projectile;
    public GameObject bulletFleshGraphic;
    public GameObject bulletHoleGraphic;

    public float sightRange = 30, attackRange = 20;
    bool playerInSightRange, playerInAttackRange;

    private Animator animator;
    private RaycastHit rayHit;

    Transform player;
    CapsuleCollider collider;
    
    private int patrolPointIndex = 0;
    public float waitAtPatrolPointTime = 3f;
    private bool isWaiting = false;

    public enum State { Patrol, Combat };
    public State currentState = State.Patrol;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        agent.destination = patrolPoints[0].position;

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (agent.velocity == Vector3.zero && currentState != State.Combat)
        {
            animator.SetBool("IsIdle", true);
        }
        if (agent.velocity != Vector3.zero && currentState != State.Combat)
        {
            animator.SetBool("IsIdle", false);
        }
    }

    void Patrolling()
    {
        if (!walkPointSet && !isWaiting) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f && !isWaiting)
        {
            walkPointSet = false;
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    void SearchWalkPoint()
    {
        walkPoint = patrolPoints[patrolPointIndex].position;
        walkPointSet = true;
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitAtPatrolPointTime);
        isWaiting = false;
        patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
        SearchWalkPoint();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void ChasePlayer()
    {
        agent.speed = 5f;
        animator.SetBool("IsDetected", true);
        currentState = State.Combat;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            Vector3 direction = transform.forward;

            Debug.DrawRay(shootingPoint.transform.position, direction * 1000, Color.red);

            animator.SetTrigger("Shoot");

            if(Physics.Raycast(shootingPoint.transform.position, direction, out rayHit, 1000, whatIsPlayer))
            {
                Debug.Log(rayHit.collider.name);

                if(rayHit.collider.CompareTag("Player"))
                {
                    rayHit.collider.GetComponent<PlayerHealth>().TakeDamage(10);
                    Instantiate(bulletFleshGraphic, rayHit.point, Quaternion.Euler(0, 100, 0));
                }
                else
                {
                    Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 100, 0));
                }

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        if(!isDead)
        {
            animator.SetTrigger("IsHit");

            health -= damage;

            if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        if(!isDead)
        {
            isDead = true;
            //animator.SetLayerWeight(1, 0);
            animator.SetBool("IsDead", true);
            //Destroy(gameObject);
            collider.enabled = false;
            agent.enabled = false;
        }
    }
}
