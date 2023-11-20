using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    //��¼ʯͷ��״̬
    public enum RockState {HitPlayer, HitEnemy,HitNothing,HitDefend};

    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;

    public int damage;

    public GameObject breakEffect;

    [HideInInspector]
    public GameObject target;

    private Vector3 direction;

    [HideInInspector]
    public RockState rockState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockState = RockState.HitPlayer;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        //sqrMagnitude��������Ϊ����
        if(rb.velocity.sqrMagnitude < 0.1f)
        {
            rockState = RockState.HitNothing;
        }
    }

    public void FlyToTarget()
    {
        //����Ҹպ���ս��ʯͷ��û�ɳ���ʱ����ֱ���ҵ����
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;


        Vector3 attackDistance = target.transform.position - transform.position;

        float tmp = attackDistance.sqrMagnitude;

        direction = (attackDistance).normalized; //+ Vector3.up * tmp * 0.02f

        //���˳�����ҵķ������÷�����һ�����Լ�һ��vector3.up,Ϊ��0,0,1��
        rb.AddForce(direction * force, ForceMode.Impulse);
        //������һ��������������ΪImpule����ͻ������

    }

    //Unity �Դ����������������ж���ײ����ʲô��Ȼ��������Ӧ�ķ�Ӧ
    private void OnCollisionEnter(Collision other)
    {
        GameObject tmpTarget = other.gameObject;
        switch ( rockState)
        {
            case RockState.HitPlayer:
                if (tmpTarget.CompareTag("Player"))
                {
                    if (!tmpTarget.GetComponent<CharacterStats>().isDefend) //û�ڷ���״̬�ͻ���
                    {
                        float outDistance = tmpTarget.GetComponent<NavMeshAgent>().stoppingDistance;

                        tmpTarget.GetComponent<NavMeshAgent>().destination = tmpTarget.GetComponent<Transform>().position
                                                         + new Vector3(outDistance + 0.1f, outDistance + 0.1f, outDistance + 0.1f);
                        tmpTarget.GetComponent<NavMeshAgent>().isStopped = true;
                        tmpTarget.GetComponent<NavMeshAgent>().velocity = direction * force * 0.5f;
                        tmpTarget.GetComponent<Animator>().SetTrigger("Dizzy");
                        tmpTarget.GetComponent<CharacterStats>().TakeDamage(damage, tmpTarget.GetComponent<CharacterStats>());

                        Instantiate(breakEffect, transform.position, Quaternion.identity);
                        Destroy(gameObject);
                    }
                    else //����ֱ��ʹ��ʧЧ
                    {
                       
                        rb.AddForce(- (direction - Vector3.down* 0.5f) * force, ForceMode.Impulse);
                        rockState = RockState.HitDefend;

                    }
                    
                }
                else if(tmpTarget.CompareTag("Ground"))
                {
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            case RockState.HitEnemy:
                if (tmpTarget.GetComponent<Golem>()) //getComponent���ص���boolֵ�Ƿ����
                {
                    tmpTarget.GetComponent<CharacterStats>().TakeDamage(damage, tmpTarget.GetComponent<CharacterStats>());
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            case RockState.HitNothing:
                break;
            case RockState.HitDefend:
                if (tmpTarget.CompareTag("Ground"))
                    rockState = RockState.HitNothing;
                if (tmpTarget.GetComponent<Golem>()) //getComponent���ص���boolֵ�Ƿ����
                {
                    tmpTarget.GetComponent<CharacterStats>().TakeDamage(damage, tmpTarget.GetComponent<CharacterStats>());
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}
