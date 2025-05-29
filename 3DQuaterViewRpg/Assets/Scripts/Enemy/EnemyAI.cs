using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Chase, Attack, Die }

    public int enemyID;
    private EnemyData enemyData;

    private State state;
    private Transform target;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private float currentHp;
    private bool isAttacking = false;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player")?.transform;
        
        enemyData = EnemyDataLoader.instance.data.enemies.Find(e => e.id == enemyID);
        if (enemyData == null)
        {
            Debug.Log($"EnemyData not found for ID {enemyID}");
            enabled = false;
            return;
        }

        navMeshAgent.speed = enemyData.moveSpeed;
        currentHp = enemyData.maxHp;
        state = State.Idle;
    }

    private void Update()
    {
        if (state == State.Die || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        switch (state)
        {
            case State.Idle:
                if (distance <= enemyData.detectRange)
                    state = State.Chase;
                break;

            case State.Chase:
                navMeshAgent.SetDestination(target.position);
                animator.SetBool("IsMoving", true);

                if (distance <= enemyData.attackRange)
                {
                    navMeshAgent.ResetPath();
                    state = State.Attack;
                }
                break;
            case State.Attack:
                if (!isAttacking)
                    StartCoroutine(AttackRoutine());
                if (distance > enemyData.attackRange)
                {
                    state = State.Chase;
                    animator.SetBool("IsMoving", true);
                }
                break;
            case State.Die:
                break;
        }

    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); // 타격 타이밍 설정
        // TODO : 데미지 판정
        yield return new WaitForSeconds(enemyData.attackCooldown);
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        if (state == State.Die) return;

        currentHp -= damage;
        if (currentHp < 0)
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        state = State.Die;
        navMeshAgent.enabled = false;
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(1.5f); // 죽음 연출 후 제거 
        Destroy(gameObject); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);

    }


}
