using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        enemy.isChase = false;
    }

    public override void OnUpdate(EnemyController enemy)
    {
        if (enemy.transform.position != enemy.OrgPos) //�����ڳ�ʼԭ��Ļ�
        {
            enemy.isWalk = true;
            enemy.agent.isStopped = false;
            enemy.agent.destination = enemy.OrgPos;

            if (Vector3.SqrMagnitude(enemy.OrgPos - enemy.transform.position) <= enemy.agent.stoppingDistance) // �ѹ�λ
            {
                enemy.isWalk = false;
                enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, enemy.OrgRotation, 0.01f);
            }
                
        }

        //����ҵ���ң����л���׷��״̬
        if (enemy.FoundPlayer())
        {
            //Debug.Log("found Player");
            enemy.SwitchStates(enemy.chaseState);
        }
    }
}
