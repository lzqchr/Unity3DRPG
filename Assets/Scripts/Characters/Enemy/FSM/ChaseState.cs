using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyBaseState
{
    private float remainLookAtTime; // ��ս��ͣ����ʱ��
    
    public override void EnterState(EnemyController enemy)
    {
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.speed;
        enemy.isWalk = false;
        enemy.isChase = true;
        remainLookAtTime = enemy.lookAtTime;
    }

    public override void OnUpdate(EnemyController enemy)
    {
             
        if (!enemy.FoundPlayer())
        {
            // ������ѷ�Χ�ͻص�����״̬
            //Debug.Log("Escaped!");
            enemy.isFollow = false;
            enemy.agent.destination = enemy.transform.position;

            //��������ԭ�ش�һ��
            if (remainLookAtTime > 0)
                remainLookAtTime -= Time.deltaTime;
            else if (enemy.isGuard)//����������ĵ��ˣ��ͻص�����״̬
                enemy.SwitchStates(enemy.guardState);
            else  // ����ص�Ѳ��״̬
                enemy.SwitchStates(enemy.patrolState);
        }
        else
        {
            // ׷������
            enemy.agent.destination = enemy.attackTarget.transform.position;
            enemy.isFollow = true;
        }

        if(enemy.TargetInAttackRange() || enemy.TargetInSkillRange()) // ���׷��������Χ�ھͽ��빥��״̬
        {
            enemy.SwitchStates(enemy.attackState);
        }
    }


}
