using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform Player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float HP;

    string PlayAniName;

    //����
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    private float SearchPointTime;

    //����
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //����
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    bool isDead;
    Animator ani;

    BreakTreeToAxe btta;

    private void Awake()
    {
        agent.GetComponent<NavMeshAgent>();
        Player = GameObject.Find("XR Rig").transform;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit hit;
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

        //�׾�����
        if(isDead)
        {
            sightRange = 0;
            agent.speed = 0;
            ani.SetBool("isStanding", false);
            ani.SetBool("isRunning", false);
            ani.SetBool("isAttacking", false);
            ani.SetBool("isWalking", false);
        }
    }

    //�����޼ҵ�
    private void Patrol()
    {
        ani.SetBool("isRunning", false);
        agent.speed = 1;
        SearchPointTime = ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
        
        if (!walkPointSet)
        {
            ani.SetBool("isWalking", false);
            ani.SetBool("isStanding", true);
            Invoke(nameof(SearchWalkPoint), SearchPointTime);
        }

        if (walkPointSet && SearchPointTime >= 1.0f)
        {
            ani.SetBool("isStanding", false);
            ani.SetBool("isWalking", true);
            agent.SetDestination(walkPoint);
        }
            

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkPoint�� ����������
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    //�̵� ��ġ ���
    private void SearchWalkPoint()
    {
        agent.speed = 0;
        //���� ����Ʈ ���
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    //���� �޼ҵ�
    private void ChasePlayer()
    {
        agent.speed = 3.5f;
        ani.SetBool("isStanding", false);
        ani.SetBool("isWalking", false);
        ani.SetBool("isAttacking", false);
        ani.SetBool("isRunning", true);
        agent.SetDestination(Player.position);
    }

    //�÷��̾� ����
    private void AttackPlayer()
    {
        //���� �������� �ʵ��� �Ѵ�.
        agent.SetDestination(transform.position);
        ani.SetBool("isWalking", false);
        ani.SetBool("isRunning", false);
        transform.LookAt(Player);
        

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            ani.SetBool("isAttacking", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    //���� ����
    private void ResetAttack()
    {
        ani.SetBool("isRunning", false);
        ani.SetBool("isAttacking", false);
        alreadyAttacked = false;
    }

    //����� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, walkPoint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    //������
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("axe"))
        {
            btta = collision.gameObject.GetComponent<BreakTreeToAxe>();
            TakeDamage(btta.power);
        }
    }


    //������ �޼ҵ�
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log("Animal: " + HP);
        
        if (HP <= 0)
        {
            Invoke(nameof(Death), 0.1f);
        }
    }

    //���� ����
    private void Death()
    {
        isDead = true;
        ani.SetBool("isDead", true);
    }
}
