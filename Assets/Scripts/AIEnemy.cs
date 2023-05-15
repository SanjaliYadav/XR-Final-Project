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
    public GameObject muzzleFlash;
    private bool isHit;
    private bool isDead;
    private Vector3 walkPoint;
    public bool walkPointSet;
    float walkPointRange;
    float health;

    float timeBetweenAttacks;
    bool alreadyAttacked;

    float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, aboutToAttack;

    private float fireRate = 2f;
    private float nextTimeToFire = 0.0f;
    private float startInstructionTime = 10f;
    RaycastHit playerhit;

    // Start is called before the first frame update
    void Start()
    {
        // walkPoint = new Vector3(0, -1, 0);
        // walkPointRange = 6;
        timeBetweenAttacks = 1000f;
        sightRange = 15f;
        attackRange = 5f;
        // walkPointSet = false;
        playerInSightRange = false;
        alreadyAttacked = false;
        playerInAttackRange = false;
        nextTimeToFire = Time.time + 1f / fireRate;
        agent.height = -1.0f;
        agent.baseOffset = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {   isHit = GetComponent<Enemy>().isHit;
        isDead = GetComponent<Enemy>().isDead;
        Debug.Log("Enemy says: Am I hit? " + isHit);
         if (!isDead && Time.time > startInstructionTime)
        {
            // playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            Debug.Log("Enemy says: Player in attack range: " + playerInAttackRange);
            Debug.Log("Enemy says: Calling Rotating");

            Rotating();

            if (playerInAttackRange)
            {
                aboutToAttack = true;
                if (Physics.Linecast(transform.position, player.transform.position, out playerhit))
                {   
                    if (playerhit.transform.tag != "Player")
                    {
                        Debug.Log("Enemy says: I see the wall");
                    } 
                    else
                    {   
                        if (Time.time >= nextTimeToFire)
                        {   
                            Debug.Log("Enemy says: I see the player");

                            // aboutToAttack = true; 
                            if (!isHit)
                                {

                                Debug.Log("Enemy says: I'm not hit");
                                AttackPlayer();
                                nextTimeToFire = Time.time + fireRate;
                                }
                            
                        }
                    
                        Debug.Log("Enemy says: I dont' see the wall");
                    }
                } 
                // else
                // {
                //     aboutToAttack = false;
                // }   
            }
        }
        else if (isDead)
        {
            // make sure the soldier doesn't move after dying 
            Debug.Log("Enemy says: Dead");
            //keep the enemy in place
            // agent.SetDestination(transform.position);
        }
        

    // //set y position to -1
    // transform.position = new Vector3(transform.position.x, -1, transform.position.z);

    }

   

    void Rotating(){
        if (!isDead)
        {
            Debug.Log("Enemy says: Rotating");
        transform.LookAt(player);
        }
        else
        {
            Debug.Log("Enemy says: Rot: Dead or bug");
        }
        //make the enemy upright
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    }

    void AttackPlayer()
    {
        Rotating();
        Debug.Log("Enemy says: Attack");

        GameObject currentMuzzle = Instantiate(muzzleFlash, gunPoint.position, gunPoint.rotation);
        currentMuzzle.transform.parent = gunPoint;
        var bulletRotationVector = bullet.transform.rotation.eulerAngles;
        bulletRotationVector.y = -75f; 
        GameObject bulletObject2 = Instantiate(bullet, gunPoint.position, Quaternion.Euler(bulletRotationVector));
        bulletObject2.GetComponent<ProjectileController>().hitpoint = player.transform.position;

    }

    void ResetAttack()
    {
        alreadyAttacked = false;

    }


}


