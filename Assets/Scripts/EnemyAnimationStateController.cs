using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// public class EnemyAnimationStateController : MonoBehaviour
// {   
//     // Start is called before the first frame update
//     private Animator animator;
//     public bool playerInAttackRange, playerInSightRange, isDead, walkPointSet, aboutToAttack;
//     private bool stopAnimation;
//     private float startInstructionTime = 10f; 

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//         stopAnimation = false; 
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (Time.time > startInstructionTime)
//         {
//             //get the bools from AIEnemy.cs
//             playerInAttackRange = GetComponent<AIEnemy>().playerInAttackRange;
//             playerInSightRange = GetComponent<AIEnemy>().playerInSightRange;
//             aboutToAttack = GetComponent<AIEnemy>().aboutToAttack;
//             isDead = GetComponent<Enemy>().isDead;
//             walkPointSet = GetComponent<AIEnemy>().walkPointSet;
//             if (stopAnimation)
//             {
//                 Invoke(nameof(KillAnimator), 200f);

//             }

//             if (isDead)
//             {
//                 animator.SetBool("isChase", false);
//                 animator.SetBool("isShooting", false);
//                 animator.SetBool("isPatrol", false);
//                 animator.SetBool("isDead", true);
//                 animator.SetBool("isStand", false);
//                 stopAnimation = true;
//             }
//             else if (!playerInAttackRange && playerInSightRange)
//             {
//                 animator.SetBool("isChase", true);
//                 animator.SetBool("isShooting", false);
//                 animator.SetBool("isPatrol", false);
//                 animator.SetBool("isDead", false);
//                 animator.SetBool("isStand", false);
//             }
//             else if (playerInAttackRange && playerInSightRange && aboutToAttack)
//             {
//                 animator.SetBool("isChase", false);
//                 animator.SetBool("isShooting", true);
//                 animator.SetBool("isPatrol", false);
//                 animator.SetBool("isDead", false);
//                 animator.SetBool("isStand", false);
//             }
//             else if (!playerInAttackRange && !playerInSightRange)
//             {
//                 animator.SetBool("isChase", false);
//                 animator.SetBool("isShooting", false);
//                 animator.SetBool("isDead", false);

//                 if (walkPointSet)
//                 {
//                     animator.SetBool("isPatrol", true);
//                     animator.SetBool("isStand", false);
//                 }
//                 else
//                 {
//                     animator.SetBool("isPatrol", false);
//                     animator.SetBool("isStand", true);
//                 }
//             } else
//             {
//                 animator.SetBool("isPatrol", false);
//                 animator.SetBool("isStand", true);
//             }

//         }

//     }

//     void KillAnimator()
//     {
//         this.GetComponent<Animator>().enabled = false;
//         this.GetComponent<EnemyAnimationStateController>().enabled = false;
//         this.GetComponent<NavMeshAgent>().enabled = false;

//     }
// }

public class EnemyAnimationStateController : MonoBehaviour
{   
    // Start is called before the first frame update
    private Animator animator;
    public bool playerInAttackRange, playerInSightRange, isDead, walkPointSet, aboutToAttack, isHit;
    private bool stopAnimation;
    private float startInstructionTime = 10f; 

    void Start()
    {
        animator = GetComponent<Animator>();
        stopAnimation = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startInstructionTime)
        {
            //get the bools from AIEnemy.cs
            playerInAttackRange = GetComponent<AIEnemy>().playerInAttackRange;
            // playerInSightRange = GetComponent<AIEnemy>().playerInSightRange;
            aboutToAttack = GetComponent<AIEnemy>().aboutToAttack;
            isDead = GetComponent<Enemy>().isDead;
            isHit = GetComponent<Enemy>().isHit;
            // walkPointSet = GetComponent<AIEnemy>().walkPointSet;
            if (stopAnimation)
            {
                Invoke(nameof(KillAnimator), 200f);

            }

            if (isDead)
            {
                animator.SetBool("isChase", false);
                animator.SetBool("isShooting", false);
                animator.SetBool("isPatrol", false);
                animator.SetBool("isDead", true);
                animator.SetBool("isStand", false);
                animator.SetBool("isHit", false);
                
                stopAnimation = true;
            }
            else if (isHit)
            {
                animator.SetBool("isHit", true);
                animator.SetBool("isChase", false);
                animator.SetBool("isShooting", false);
                animator.SetBool("isPatrol", false);
                animator.SetBool("isDead", false);
                animator.SetBool("isStand", false);
            }
            // else if (!playerInAttackRange && playerInSightRange)
            // {
            //     animator.SetBool("isChase", true);
            //     animator.SetBool("isShooting", false);
            //     animator.SetBool("isPatrol", false);
            //     animator.SetBool("isDead", false);
            //     animator.SetBool("isStand", false);
            // }
            else if (playerInAttackRange && aboutToAttack)
            {
                animator.SetBool("isChase", false);
                animator.SetBool("isShooting", true);
                animator.SetBool("isPatrol", false);
                animator.SetBool("isDead", false);
                animator.SetBool("isStand", false);
                animator.SetBool("isHit", false);

            }
            // else if (!playerInAttackRange && !playerInSightRange)
            // {
            //     animator.SetBool("isChase", false);
            //     animator.SetBool("isShooting", false);
            //     animator.SetBool("isDead", false);

            //     if (walkPointSet)
            //     {
            //         animator.SetBool("isPatrol", true);
            //         animator.SetBool("isStand", false);
            //     }
            //     else
            //     {
            //         animator.SetBool("isPatrol", false);
            //         animator.SetBool("isStand", true);
            //     }
            // } 
            else
            {   
                animator.SetBool("isPatrol", false);
                animator.SetBool("isStand", true);
                animator.SetBool("isChase", false);
                animator.SetBool("isShooting", false);
                animator.SetBool("isDead", false);
                animator.SetBool("isHit", false);
            }

        }

    }

    void KillAnimator()
    {
        this.GetComponent<Animator>().enabled = false;
        this.GetComponent<EnemyAnimationStateController>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;

    }
}
