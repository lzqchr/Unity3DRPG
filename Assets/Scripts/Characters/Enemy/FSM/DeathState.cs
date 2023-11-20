using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        enemy.isDead = true;
    }

    public override void OnUpdate(EnemyController enemy)
    {
        enemy.GetComponent<Collider>().enabled = false;
        //enemy.agent.enabled = false; //��agent�ر�,�ᵼ����������״̬ת�����״̬ʱ��Ϊ�ַ�����һ��agent���±���
        enemy.agent.radius = 0; //���뾶����Ϊ0
        Object.Destroy(enemy.gameObject, 2f); //���ٵ��˵����壬�ӳ�����
    }
}
