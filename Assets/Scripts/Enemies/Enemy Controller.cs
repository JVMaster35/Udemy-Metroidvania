using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackDistance; //Minimum Distance for Attack
    public float moveSpeed; //Enemy Movement Speed
    public float timerCool; //Cooldown Time between Attacks

    public bool shouldPatrol, shouldShoot; //Determines if an Enemy should patrol or shoot.
    public bool isDead; //Checks if Enemy GameObject is Dead

    public EnemyShot enemyShot;
    public Transform shootPoint;

    public Transform leftLimit; //Left Point Patrol Limit
    public Transform rightLimit; //Right Point Patrol Limit

    public GameObject hotZone; //Alarm Trigger for detecting Player
    public GameObject triggerArea;

    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    public Transform target;
    public bool inRange; //If Player is in Range Checker
    private Animator enemyAnim;
    private float distance; //Distance between Enemy and Player
    private bool attackMode;
    private bool attackCooling; //Enemy Attack Cooldown Checker
    private float initTimer; //Initial Timer

    private void Awake()
    {
        initTimer = timerCool;
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask);

        if (!isDead)
        {
            if (!attackMode && isGrounded)
            {
                Move();
            }
            else if (!isGrounded)
            {
                Patrol();
                Move();
            }

            if (!InsideOfLimits() && !inRange && !enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Enemy" + 
                "_Attack"))
            {
                Patrol();
            }

            if (inRange)
            {
                EnemyLogic();
            }
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }

        else if (attackDistance >= distance && attackCooling == false)
        {
            Attack();
        }

        if (attackCooling)
        {
            CoolDown();
            enemyAnim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        if (target != null)
        {
            if (shouldPatrol)
            {
                enemyAnim.SetBool("Walking", true);
                if (!enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") && !inRange)
                {
                    Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                }
                else
                {
                    Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, (moveSpeed + 2f) * Time.deltaTime);
                }
            }
        }
    }

    void Attack()
    {
        isGrounded = true;
        timerCool = initTimer; //reset timer when player enter attack range
        attackMode = true;//to check if enemy can still attack or not

        enemyAnim.SetBool("Walking", false);
        enemyAnim.SetBool("Attack", true);
    }

    public void Shooting()
    {
        if (shouldShoot)
        {
            Instantiate(enemyShot, shootPoint.position, shootPoint.rotation).moveDirection = new Vector2(transform.localScale.x, 0);
        }
    }

    void CoolDown()
    {
        timerCool -= Time.deltaTime;
        if (timerCool <= 0 && attackCooling && attackMode)
        {
            attackCooling = false;
            timerCool = initTimer;
        }
    }

    void StopAttack()
    {
        attackCooling = false;
        attackMode = false;
        enemyAnim.SetBool("Attack", false);
    }

    public void TriggerCooling()
    {
        attackCooling = true;
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void Patrol()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        if (target != null)
        {

            Vector3 rotation = transform.eulerAngles;
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
