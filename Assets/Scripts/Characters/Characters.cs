using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum CharState
{
    Idle,
    Walk,
    Attack,
    WalkToEnemy,
    Hit,
    Die
    
}

public abstract class Characters : MonoBehaviour
{
    protected NavMeshAgent navAgent;
    protected Animator anim;
    [SerializeField] protected GameObject ringSelection;

    public GameObject RingSelection
    {
        get { return ringSelection; }
    }

    [SerializeField] protected int curHP = 10;

    public int CurHP
    {
        get { return curHP; }
    }

    [SerializeField] protected Characters curCharTarget;

    public Characters CurCharTarget
    {
        get { return curCharTarget; }
        set { curCharTarget = value; }
    }

    [SerializeField] protected int attackDamage = 3;
    [SerializeField] protected float attackRange = 2f;

    public float AttackRange
    {
        get { return attackRange; }
    }

    [SerializeField] protected float attackCoolDown = 2f;
    [SerializeField] protected float attackTimer = 0f;
    [SerializeField] protected float findingRange = 20f;

    public float FindingRange
    {
        get { return findingRange; }
    }

    public Animator Anim
    {
        get { return anim; }
    }

    [SerializeField] protected CharState state;

    public CharState State
    {
        get { return state; }
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

    }

    public void ToggleRingSelection(bool flag)
    {
        ringSelection.SetActive(flag);
    }

    public void ReceiveDamage(Characters enemy)
    {
        if (curHP <= 0 || state == CharState.Die)
        {
            return;
        }

        curHP -= enemy.attackDamage;
        if (curHP <= 0)
        {
            curHP = 0;
            Die();
        }
    }

    protected void AttackLogic()
    {
        Characters target = curCharTarget.GetComponent<Characters>();
        if (target != null)
        {
            target.ReceiveDamage(this);
        }
    }

    public void ToAttackCharacter(Characters target)
    {
        if (curHP <= 0 || state == CharState.Die)
        {
            return;
        }

        curCharTarget = target;

        navAgent.SetDestination(target.transform.position);
        navAgent.isStopped = false;

        SetState(CharState.WalkToEnemy);
    }

    protected void Attack()
    {
        transform.LookAt(curCharTarget.transform);
        anim.SetTrigger("Attack");
        AttackLogic();
    }

    protected void WalkToEnemyUpdate()
    {
        if (curCharTarget == null)
        {
            SetState(CharState.Idle);
            return;
        }

        navAgent.SetDestination(curCharTarget.transform.position);
        float distance = Vector3.Distance(transform.position,
            curCharTarget.transform.position);
        if (distance <= attackRange)
        {
            SetState(CharState.Attack);
            Attack();
        }
    }

    protected void AttackUpdate()
    {
        if (curCharTarget == null)
        {
            return;
        }

        if (curCharTarget.CurHP <= 0)
        {
            SetState(CharState.Idle);
            return;
        }

        navAgent.isStopped = true;
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCoolDown)
        {
            attackTimer = 0f;
            Attack();
        }

        float distance = Vector3.Distance(transform.position,
            curCharTarget.transform.position);

        if (distance > attackRange)
        {
            SetState(CharState.WalkToEnemy);
            navAgent.SetDestination(curCharTarget.transform.position);
            navAgent.isStopped = false;
        }
    }

    public void SetState(CharState s)
    {
        state = s;
        if (state == CharState.Idle)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }
    }

    public void WalkToPosition(Vector3 dest)
    {
        if (navAgent != null)
        {
            navAgent.SetDestination(dest);
            navAgent.isStopped = false;

        }

        SetState(CharState.Walk);
    }

    protected void WalkUpdate()
    {
        float distance = Vector3.Distance(transform.position, navAgent.destination);
        Debug.Log(distance);

        if (distance <= navAgent.stoppingDistance)
        {
            SetState(CharState.Idle);
        }
    }

    protected virtual void Die()
    {
        navAgent.isStopped = true;
        SetState(CharState.Die);
        anim.SetTrigger("Die");

        StartCoroutine(DestroyObject());
    }

    protected virtual IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public bool IsMyEnemy(string targetTag)
    {
        string myTag = gameObject.tag;
        if ((myTag == "Hero" || myTag == "Player") && targetTag == "Enemy")
        {
            return true;
        }
        if (myTag == "Enemy" &&  (targetTag == "Hero" || targetTag == "Player"))
        {
            return true;
        }

        return false;

    }


}
