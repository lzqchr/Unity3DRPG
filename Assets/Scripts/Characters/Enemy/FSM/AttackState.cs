using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        enemy.isFollow = false;

    }

    public override void OnUpdate(EnemyController enemy)
    {
        enemy.agent.isStopped = true;
        if (enemy.FoundPlayer())
        {
            if (enemy.lastAttackTime < 0)
            {
                enemy.lastAttackTime = enemy.characterStats.CoolDown;

                enemy.characterStats.isCritical = Random.value < enemy.characterStats.CriticalChance;
                //ִ�й���
                if (enemy.attackTarget != null)
                    Attack(enemy);
            }

            if (!enemy.TargetInAttackRange() && !enemy.TargetInSkillRange()) //���ڹ�����Χ�ͻص�׷��״̬
            {

                enemy.SwitchStates(enemy.chaseState);
            }
        }
        else
        {
            if (enemy.isGuard)
                enemy.SwitchStates(enemy.guardState);
            else
                enemy.SwitchStates(enemy.patrolState);
        }
    }

    void Attack(EnemyController enemy)
    {
        //������ת�򹥻�Ŀ��
        enemy.transform.LookAt(enemy.attackTarget.transform);

        if (enemy.TargetInAttackRange())//��ս����
        {
            enemy.anim.SetTrigger("Attack");
        }

        if (enemy.TargetInSkillRange())//���ܹ���
        {
            enemy.anim.SetTrigger("Skill");
        }

        

        
    }
}
