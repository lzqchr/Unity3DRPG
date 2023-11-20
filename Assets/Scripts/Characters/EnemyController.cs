using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Ҫ���Agent�Ŀ���Ȩ

[RequireComponent(typeof(NavMeshAgent))]// ����������û�и����ʱ�Զ�������
[RequireComponent(typeof(CharacterStats))]


public class EnemyController : MonoBehaviour,IEndGameObserver
{
    [HideInInspector]//״̬������
    EnemyBaseState currentState;

    [HideInInspector]//�洢���ݵı���
    public CharacterStats characterStats;

    [HideInInspector]//��ȡEnemy��Agent
    public NavMeshAgent agent;

    [HideInInspector]//��ȡ���ص�animator controller
    public Animator anim;

    //Agent����Ұ��Χ
    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public float speed; //��¼�ٶ�
    public float lookAtTime; //��¼��Ѳ��״̬ʱ�ڱ߽��鿴���

    [Header("Patrol States")]
    public float patrolRange; //���õ�������Ѳ�ߵķ�Χ

    [HideInInspector] //��ȡ����Ŀ��
    public GameObject attackTarget; 
    [HideInInspector]//��ȡ���˵ĳ�ʼ���� 
    public Vector3 OrgPos;
    [HideInInspector]//��ȡ���˵ĳ�ʼ����Ƕ� 
    public Quaternion OrgRotation;
    [HideInInspector] // ����ֵ�жϿ��ƶ���״̬ת��
    public bool isFollow, isWalk, isChase, isDead;
    [HideInInspector] // �������
    public float lastAttackTime;

    bool playerDead;


    //״̬������
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public GuardState guardState = new GuardState();
    public AttackState attackState = new AttackState();
    public DeathState deathState = new DeathState();

    void Awake()
    {
        //��������ĸ�ֵͨ����Awake����
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        speed = agent.speed;
        OrgPos = transform.position;
        OrgRotation = transform.rotation;
        lastAttackTime = 0;
        playerDead = false;
        characterStats.isDefend = false;
    }

    private void Start()
    {  
        if(isGuard) //վ׮���˽�������״̬
        {
            SwitchStates(guardState);
        }
        else
        {
            //Ѳ�ߵ��˽���Ѳ��״̬
            SwitchStates(patrolState);
        }

        //FIXME: ��֮�󳡾�����ʱ�ŵ�OnEnable()����
        GameManager.Instance.AddObserver(this);
    }

    //void onenable()
    //{
    //    //�ѹ۲��������б�
    //    gamemanager.instance.addobserver(this);
    //}

    void OnDisable() //OnDisable���������֮��ŵ���
    {
        //���GameManagerû�б���ʼ������ֱ��return
        if (!GameManager.IsInitialized) return;
        //����ʱ�Ƴ�
        GameManager.Instance.RemoveObserver(this);
    }

    void Update()
    {
        if (characterStats.CurrentHealth == 0)
        {
            SwitchStates(deathState);
        }
        
        if (!playerDead)
        {
            currentState.OnUpdate(this); //��ÿһ֡���¸�ģʽ�µ�OnUpdate����
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation() //��ÿһ֡�����ű��еı������������������parameters
    {
        anim.SetBool("Death", isDead);
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Critical", characterStats.isCritical);
    }

    /// <summary>
    /// ״̬ת��
    /// </summary>
    /// <param name="state"></param>
    public void SwitchStates(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    /// <summary>
    /// �ƶ���Ŀ��λ��
    /// </summary>
    /// <param name="position"></param>
    public void MoveToTarget(Vector3 position)
    {
        agent.destination = position;
    }
   
    /// <summary>
    /// ����Ѱ�ҵ����
    /// </summary>
    /// <returns></returns>
    public bool FoundPlayer()
    {
        //��ȡ��������õ��˵��ӽ��ص���collider
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        //����ÿһ��collider���������ң��ͽ�AttackTarget������
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    /// <summary>
    /// �ж��Ƿ��ڹ�����Χ���ܷ�Χ֮��
    /// </summary>
    /// <returns></returns>
    public bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.AttackRange;
        else 
            return false;
    }

    public bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
           // Debug.Log(Vector3.Distance(attackTarget.transform.position, transform.position));
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.SkillRange;
        }    
        else
            return false;
    }

    /// <summary>
    /// ��ʾGizmo�������
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //��Scene����ʾ�趨��Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    /// <summary>
    /// ������Animation Event
    /// </summary>
    void Hit()
    {
        //��Ϊ���˹����Ǳ������������Ե�������ʱ��������Ѿ��׿�����ȡ����attackTarget����ʱ�ᱨ������Ҫ�Ƚ����ж�
        if(attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            CharacterStats targetstats = attackTarget.GetComponent<CharacterStats>();
            targetstats.TakeDamage(characterStats, targetstats);
        }

    }

    /// <summary>
    /// ���ǵ����յ���Ϸ����֪ͨʱ��ִ�еĴ��룬���������ʱ����Ϸ����
    /// </summary>
    public void EndNotify()
    {
        //��ʤ����
        
        anim.SetBool("Win", true);
        isWalk = false;
        isFollow = false;
        isChase = false;
        attackTarget = null;
        playerDead = true;
        //ֹͣ�ƶ�
        //ֹͣAgent
    }
}
