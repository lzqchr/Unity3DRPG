using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data", menuName ="Character States/Data")]

public class CharacterData_SO : ScriptableObject
{
    [Header("States Info")] //���Է�չ�����������������
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
    public float defendRange;

    [Header("Kill")]
    public int killPoint; //��ɱ������ľ���ֵ����Ҫ�ǵ������������

    [Header("Level")]
    public int currentLevel; //��ǰ�ȼ�

    public int maxLevel; //��ߵȼ�

    public int baseExp; // ��������ֵ

    public int currentExp; // ��ǰ����ֵ

    public float levelBuff; //����֮���������İٷֱ�

    public float LevelMultiplier //����
    {   //���ŵȼ����������ӵľ���Խ��Խ��
        get { return 1 + ( currentLevel - 1 ) * levelBuff; }
    }

    public void UpdateExp(int point) //����������õ��ľ���
    {
        currentExp += point;

        if (currentExp >= baseExp) //��ǰ����ֵ���ڻ�������ֵ������
            LevelUp();
    }

    private void LevelUp()
    {
        //�ȼ�����
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        //��һ���׶���Ҫ�ľ���ֵ
        baseExp += (int)(baseExp * LevelMultiplier);

        //Ѫ������
        maxHealth = (int)(maxHealth * LevelMultiplier);
        //Ѫ���ظ�
        currentHealth = maxHealth;


       // Debug.Log("LEVEL UP!" + currentLevel + "Max Health" + maxHealth);
    }
}

