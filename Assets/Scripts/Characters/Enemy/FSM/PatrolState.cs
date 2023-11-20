using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyBaseState
{
    Vector3 wayPoint; //��¼Patrol״̬ʱ��������ƶ�Ŀ���
    Vector3 OrgPos;

    float patrolRange; //����Ѳ�ߵķ�Χ
    private float remainLookAtTime;
    public override void EnterState(EnemyController enemy)
    { 
        GetNewWayPoint(enemy); //һ��ʼ��һ��Ѳ�ߵ����Ŀ���
        enemy.agent.speed = enemy.speed * 0.5f; //��agent���ٶ�����Ϊԭ���ٶȵ�һ�� Unity�˷�����С
        patrolRange = enemy.patrolRange; //��ȡ��Ѳ�ߵİ뾶
        enemy.isChase = false; //û�н���׷�� ����׷��Ϊfalse
        OrgPos = enemy.OrgPos;
        remainLookAtTime = enemy.lookAtTime;
    }

    public override void OnUpdate(EnemyController enemy)
    {
        if(Vector3.Distance(wayPoint, enemy.transform.position) <= enemy.agent.stoppingDistance)
        {
            //����������Ŀ����Ѿ���stoppingdistancce֮�ڣ��ʹ����ҵ��µĵ�
            enemy.isWalk = false;
            if (remainLookAtTime > 0) //ͣ����Ѳ��һ��
                remainLookAtTime -= Time.deltaTime;
            else
                GetNewWayPoint(enemy);
        }
        else
        {
            //�����û������ô���˾ͻ���������ƶ���ͬʱ����walk�Ķ���
           // Debug.Log(wayPoint);
            enemy.isWalk = true;
            enemy.agent.destination = wayPoint;
        }


        //����ҵ���ң����л���׷��״̬
        if (enemy.FoundPlayer())
        {
            //Debug.Log("found Player");
            enemy.SwitchStates(enemy.chaseState);
        }
    }



    /// <summary>
    /// ��ȡ������һ�������ƶ���Ŀ���
    /// </summary>
    void GetNewWayPoint(EnemyController enemy)
    {
        remainLookAtTime = enemy.lookAtTime;
        //����y�᲻�䣬�ڹ涨��Χ�ڵõ�һ������ĵ�
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(OrgPos.x + randomX, enemy.transform.position.y,OrgPos.z + randomZ);


        //wayPoint = randomPoint;
        //���ֱ��������ȡ���ƶ���Ŀ��㡣���ܻ����Ŀ���ѡ��non walkable�������ڣ���������Զ�ƶ����������������Ҫ�ܿ������ĵ�
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1)? hit.position : enemy.transform.position;

    }
}
