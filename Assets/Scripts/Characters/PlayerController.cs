using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //��ȡ���
    private NavMeshAgent agent;
    private Animator anim;
    private CharacterStats characterStats;

    //����Ŀ����Ҫ�ı���
    private GameObject attackTarget;
    private float lastAttackTime; // ��¼��һ�ι�����ʱ�䣬�������ü�����ȴ
    private float stopDistance;
    private float orgSpeed;

    //������
    private bool isDead;

    private bool isGuard;




    private void Awake() 
    {
        //�����components��awake��ʱ����ã���ΪAwake������Ϸ�ʼ��ʱ�����Ȼ����Щ��ֵ��
        //������ֿ����õ����
        agent = GetComponent<NavMeshAgent>(); //agent����NavMeshAgent�����
        anim = GetComponent<Animator>(); //��ȡ��Animator���
        characterStats = GetComponent<CharacterStats>();
        stopDistance = agent.stoppingDistance;
        orgSpeed = agent.speed;


    }
    private void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget; //ʹ��+=��MoveToTarget��һ����ע���OnMouseClicked�¼���
        MouseManager.Instance.OnEnemyClicked += EventAttack; //��EventAttack����ע�ᵽOnEnemyClicked�¼���
        GameManager.Instance.RegisterPlayer(characterStats); //����ҵ����ݴ���ȥ
    }
    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }
    private void OnDisable()
    {
        //���MouseManager��û��ʵ�������Ͳ�ִ��
        if (!MouseManager.IsInitialized) return;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }


    private void Update()
    {
        isGuard = Input.GetButton("Fire2");
        characterStats.isDefend = isGuard;
        isDead = characterStats.CurrentHealth == 0;
        if (isDead) //������˾ͽ��й㲥
            GameManager.Instance.NotifyObservers();

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);//��animator�е�peremeter ��Speed������ֵ����Ϊagent���ٶȣ�velocityͨ��ʹ��sqrmagnitudeת��Ϊfloat���͵���ֵ
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDead);
        anim.SetBool("Defend", isGuard);
    }

    public void MoveToTarget(Vector3 target)
    {
        if (isDead) return;

        agent.stoppingDistance = stopDistance; //move to target��ʱ��Ļ�ԭ����ֹͣ����
        agent.speed = orgSpeed;

        StopAllCoroutines(); //�������Я��
        agent.isStopped = false; //��һ�ε�������ʱ������agent
        agent.destination = target;//�޸������������ͨ��ֵΪtarget  
           
    }

    //�����
    //private void EventPushOff(GameObject target)
    //{
    //    //��������һ����ֹͣ��������ĵ�ΪĿ�����agent������
    //    agent.destination = transform.position + new Vector3(1, 1, 1);
    //    agent.isStopped = false; 
    //    Vector3 dir = new Vector3(1, 1, 1).normalized;
    //    agent.velocity = dir * 15;
    //    anim.SetTrigger("Dizzy");
    //}

    private void EventAttack(GameObject target) //һ���¼����õĺ�������
    {
        if (isDead) return;

        //ִ�й���֮ǰ�ȼ����Ƿ��Ǳ���
        characterStats.isCritical = UnityEngine.Random.value < characterStats.CriticalChance;
        if (target!=null)//����������ʱ��ʧȥĿ�꣬Ҫ�����ж�
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget()); //�����ƶ���Ŀ����˵�Я��
        }
    }

    IEnumerator MoveToAttackTarget() //����һ��Э��
    {

        agent.isStopped = false;//player������
        agent.speed = 1.5f * orgSpeed;

        transform.LookAt(attackTarget.transform); //transform���õĺ�������������������ת��Ŀ��
        agent.stoppingDistance = characterStats.AttackRange; //��StoppingDistance���ô��ڵ��˵�Radius
        

        //������λ�ú�player��ǰλ�õľ�����ڹ�������ʱ�����ƶ�������λ��
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.AttackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null; //����һ֡�ٴ�ִ�����������      
        }

        agent.isStopped = true;//playerͣ����

        //attack
        if(lastAttackTime < 0)//cd��ȴ���� ������һ�ι���
        {
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetTrigger("Attack");
            //������ȴʱ��
            lastAttackTime = characterStats.CoolDown;
        }
    }

    //Animation event
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            //����ɹ���������ʯͷ���ͽ�ʯͷ��״̬����Ϊ�������
            if(attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockState == Rock.RockState.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockState = Rock.RockState.HitEnemy;

                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;

                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);

            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }


}
