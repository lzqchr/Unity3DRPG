using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10;

    public void KickOff()
    {
        if (attackTarget!=null)
        {
            transform.LookAt(attackTarget.transform);

            //��������һ����ֹͣ��������ĵ�ΪĿ�����agent������
            float outDistance = attackTarget.GetComponent<NavMeshAgent>().stoppingDistance;
            attackTarget.GetComponent<NavMeshAgent>().destination = attackTarget.GetComponent<Transform>().position 
                                                                  + new Vector3(outDistance+0.1f, outDistance + 0.1f, outDistance + 0.1f);

            //��ó�����˵ķ�������
            Vector3 dir = (attackTarget.transform.position - transform.position).normalized;
            //ֹͣ���������agent������
            attackTarget.GetComponent<NavMeshAgent>().isStopped = false;
            //ͨ�����һ������ָ������������л���

            attackTarget.GetComponent<NavMeshAgent>().velocity = dir * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            
        }
    }
}
