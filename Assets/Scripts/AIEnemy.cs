using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class AIEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject bullet;
    public Transform gunPoint; 

    private Vector3 walkPoint;
    public bool walkPointSet;
    float walkPointRange;
    float health;

    float timeBetweenAttacks;
    bool alreadyAttacked;

    float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private float fireRate = 10000f;
    private float nextTimeToFire = 0.0f;
    RaycastHit playerhit;

    // Start is called before the first frame update
    void Start()
    {
        walkPoint = new Vector3(0, -1, 0);
        walkPointRange = 6;
        timeBetweenAttacks = 700f;
        sightRange = 15f;
        attackRange = 10f;
        walkPointSet = false;
        playerInSightRange = false;
        alreadyAttacked = false;
        playerInAttackRange = false;
        nextTimeToFire = Time.time + 1f / fireRate;
        agent.height = -1.0f;
        agent.baseOffset = 0f;
    }

    // Update is called once per frame
    void Update()
    {  
         if (!GetComponent<Enemy>().isDead)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);



            if (!playerInAttackRange && !playerInSightRange)
            {
                Patrolling();
            } else if (!playerInAttackRange && playerInSightRange)
            {
                ChasePlayer();
            }
            else if (playerInAttackRange && playerInSightRange)
            {
                if (Physics.Linecast(transform.position, player.transform.position, out playerhit))
                {
                    if (playerhit.transform.tag == "Wall")
                    {
                        Debug.Log("I see the wall"); 
                    

                    } else
                    {
                        if (Time.time >= nextTimeToFire)
                        {
                            AttackPlayer();
                            nextTimeToFire = Time.time + 1f / fireRate;

                        }
                    
                        Debug.Log("I dont' see the wall");
                    }
                }   
            }
        }
        else
        {
            Debug.Log("Dead");
            //keep the enemy in place
            // agent.SetDestination(transform.position);
        }
        

    // //set y position to -1
    // transform.position = new Vector3(transform.position.x, -1, transform.position.z);

    }

    void Patrolling()
    {
        Debug.Log("Patrolling");
        if (!walkPointSet)
        {
            SearchWalkingPoint();

        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false; 
        }

    }

    void SearchWalkingPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true; 
        } 
    }

    void ChasePlayer()
    {
        Debug.Log("Chasing");
        agent.SetDestination(player.position);

    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        { 
            

            Debug.Log("Attack");
            GameObject bulletObject2 = Instantiate(bullet, gunPoint.position + transform.forward * 0.5f + transform.up * 0.3f, Quaternion.identity);
            bulletObject2.GetComponent<ProjectileController>().hitpoint = player.transform.position;

            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    void ResetAttack()
    {
        alreadyAttacked = false;

    }
}
