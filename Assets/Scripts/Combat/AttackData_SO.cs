using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack Data", menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    //������Χ�����ܷ�Χ
    public float attackRange;
    public float skillRange;

    //cd��ȴ
    public float coolDown;

    //��ͨ����
    public int minDamage;
    public int maxDamage;

    //����
    public float criticalMultiplier;
    public float criticalChance;
}
