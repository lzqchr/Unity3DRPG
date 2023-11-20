using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 15;

    public GameObject rockPrefab; //���ʯͷ��Ԥ����
    public Transform handPos; //����ֲ�������

    /// <summary>
    /// Animation Event KickOff
    /// </summary>
    public void KickOff()
    {
       
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            CharacterStats targetstats = attackTarget.GetComponent<CharacterStats>();
            targetstats.TakeDamage(characterStats, targetstats);
        }
        
        
        if(attackTarget!=null)
        {
            CharacterStats targetstats = attackTarget.GetComponent<CharacterStats>();
            //ת�����
            transform.LookAt(attackTarget.transform);

            //��������һ����ֹͣ��������ĵ�ΪĿ�����agent������
            float outDistance = attackTarget.GetComponent<NavMeshAgent>().stoppingDistance;
            attackTarget.GetComponent<NavMeshAgent>().destination = attackTarget.GetComponent<Transform>().position 
                                                                   + new Vector3(outDistance + 0.1f, outDistance + 0.1f, outDistance + 0.1f);
            if (!targetstats.isDefend)
            {
                Vector3 dir = attackTarget.transform.position - transform.position;
                dir.Normalize();

                attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
                attackTarget.GetComponent<NavMeshAgent>().velocity = dir * kickForce;
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            }
            else
            {
                Vector3 dir = attackTarget.transform.position - transform.position;
                dir.Normalize();
                attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
                attackTarget.GetComponent<NavMeshAgent>().velocity = dir * kickForce * 0.3f;
                attackTarget.transform.LookAt(transform);
            }
        }
    }

    /// <summary>
    /// Animation Event Throw Rock
    /// </summary>
    public void ThrowRock()
    {
        if(attackTarget!=null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, rockPrefab.transform.rotation);
            //��ʯͷ���ɳ��������ɵ���ʯͷԤ���壬λ���������ĵ����꣬����Ҫ��ת
            rock.GetComponent<Rock>().target = attackTarget;
            //��Сʯͷ�Ĺ�����������Ϊ��ǰ�Ĺ�������
        }
    }
}
